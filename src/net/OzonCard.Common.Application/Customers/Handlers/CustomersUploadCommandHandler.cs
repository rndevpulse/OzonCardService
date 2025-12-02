using Medallion.Threading;
using Microsoft.Extensions.Logging;
using OzonCard.Cloud.Client.Data.Customers;
using OzonCard.Common.Application.Customers.Commands;
using OzonCard.Common.Application.Organizations;
using OzonCard.Common.Core;
using OzonCard.Common.Core.Exceptions;
using OzonCard.Common.Domain.Organizations;
using OzonCard.Excel;
using OzonCard.Files;
using Customer = OzonCard.Common.Domain.Customers.Customer;

namespace OzonCard.Common.Application.Customers.Handlers;

public class CustomersUploadCommandHandler(
    ILogger<CustomersUploadCommandHandler> logger,
    IDistributedLockProvider locks,
    IOrganizationRepository orgRepository,
    ICustomerRepository customerRepository,
    IExcelManager excelManager,
    IEventBus events,
    IFileManager fileManager
) : CustomerBaseHandler(events), ICommandHandler<CustomersUploadCommand, IEnumerable<Customer>>
{

    public async Task<IEnumerable<Customer>> Handle(CustomersUploadCommand request, CancellationToken cancellationToken)
    {
        var token = $"CUSTOMERS_${request.OrganizationId}";
        var timeout = TimeSpan.FromSeconds(5);
        await using (await locks.AcquireLockAsync(token, timeout, cancellationToken))
        {
            var fileCustomers = request.Customer != null
                ?
                [
                    new()
                    {
                        Card = request.Customer.Card,
                        Name = request.Customer.Name,
                    }
                ]
                : excelManager.GetCustomers(fileManager.GetFile(request.FileReport)).ToList();

            var org = await orgRepository.GetItemAsync(request.OrganizationId, cancellationToken);
            // if (org.Members.All(x => x.Name != request.User))
            //     throw new BusinessException($"Organization for '{request.User}' not found");
            var program = org.Programs.FirstOrDefault(x => x.ProgramId == request.ProgramId)
                          ?? throw EntityNotFoundException.For<Program>(request.ProgramId, $"in org '{org.Name}'");

            Progress.CountAll = fileCustomers.Count;
            logger.LogInformation($"Try upload by {request.User} '{fileCustomers.Count}' customers");
            ReportProgress(request.Tracking, Progress);

            var customers = (await customerRepository.GetCustomersByCardsAsync(
                org.Id,
                fileCustomers.Select(x => x.Card),
                cancellationToken)).ToList();
           
            var result = new List<Customer>();

            foreach (var fileCustomer in fileCustomers)
            {
                var customer = customers.FirstOrDefault(x => x.Cards.Any(c => c.Track == fileCustomer.Card));
                var isNewCustomer = false;
                if (customer == null)
                {
                    //create if new customer
                    isNewCustomer = true;
                    customer = await TryCreateCustomer(org, fileCustomer, cancellationToken);
                    if (customer == null)
                        continue;
                    await customerRepository.AddAsync(customer);
                }

                //rename if enable option and not new customer
                if (request.Options.Rename && !isNewCustomer)
                {
                    customer.Name = fileCustomer.Name;
                    await org.CloudClient.CreateOrUpdateCustomerAsync(
                        new CreateOrUpdateCustomer(org.TransportId)
                        {
                            Id = customer.BizId,
                            Name = fileCustomer.Name,
                            CardNumber = fileCustomer.Card,
                            CardTrack = fileCustomer.Card
                        }, cancellationToken);
                }

                //update second fields
                customer.TabNumber = fileCustomer.TabNumber;
                customer.Position = fileCustomer.Position;
                customer.Division = fileCustomer.Division;

                //update customers categories
                await UpdateCategories(org, customer, org, request.CategoriesId, cancellationToken);

                //try create wallet
                try
                {
                    await org.CloudClient.AddToProgramAsync(
                        customer.BizId,
                        program.ProgramId,
                        org.TransportId,
                        cancellationToken);
                    // customer.TryAddWallet(wallet.Id, wallet.Name, wallet.ProgramType, wallet.Type);
                }
                finally
                {
                    Progress.CountProgram++;
                }

                if (request.Options.RefreshBalance && program.WalletId is {} walletId)
                    await TryRefreshBalance(org, customer.BizId, walletId, request.Balance,
                        cancellationToken);

                result.Add(customer);
                //update progress task
                ReportProgress(request.Tracking, Progress);
            }

            return result;
        }
    }



   

   

    private async Task UpdateCategories(Organization organization, Customer customer, Organization org, 
        IEnumerable<Guid> categoriesId, CancellationToken ct)
    {
        foreach (var categoryId in categoriesId)
        {
            var category = org.Categories.FirstOrDefault(x=>x.CategoryId == categoryId);
            if (category == null)
            {
                logger.LogError($"Category '{categoryId}' not found in '{org.Name}'");
                continue;
            }
            try
            {
                await organization.CloudClient.AddCustomerCategoryAsync(
                    organization.TransportId,
                    customer.BizId,
                    category.CategoryId,
                    ct
                );
                customer.AddCategory(category);
            }
            finally
            {
                Progress.CountCategory++;
            }
        }
    }


    private async Task<Customer?> TryCreateCustomer(Organization organization, Excel.Models.Customer fileCustomer, CancellationToken ct)
    {
        try
        {
            var bizCustomer = await organization.CloudClient.CreateOrUpdateCustomerAsync(
                new CreateOrUpdateCustomer(organization.TransportId)
                {
                    Name = fileCustomer.Name,
                    CardNumber = fileCustomer.Card,
                    CardTrack = fileCustomer.Card,
                }, ct);
            if (bizCustomer != Guid.Empty)
                Progress.CountNew++;
            else
            {
                Progress.CountFail++;
                logger.LogError($"Customer {fileCustomer.Name} {fileCustomer.Card} not create in biz");
                return null;
            }

            var customer = new Customer(Guid.NewGuid(),
                fileCustomer.Name, bizCustomer, organization.Id, true,
                string.Empty, fileCustomer.TabNumber, fileCustomer.Position, fileCustomer.Division
            )
            {
                CreatedBiz = DateTimeOffset.Now
            };
            customer.TryAddCard(fileCustomer.Card, fileCustomer.Card);
            return customer;
        }
        catch (HttpRequestException)
        {
            throw;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error while trying to create customer: {Customer} {Card}", fileCustomer.Name, fileCustomer.Card);
            Progress.CountFail++;
            return null;
        }
    }
}