using Microsoft.Extensions.Logging;
using OzonCard.Biz.Client;
using OzonCard.Common.Application.BackgroundTasks;
using OzonCard.Common.Application.Customers.Commands;
using OzonCard.Common.Application.Organizations;
using OzonCard.Common.Core;
using OzonCard.Common.Core.Exceptions;
using OzonCard.Common.Domain.Customers;
using OzonCard.Common.Domain.Organizations;
using OzonCard.Excel;
using OzonCard.Files;

namespace OzonCard.Common.Application.Customers.Handlers;

public class CustomersUploadCommandHandler(
    ILogger<CustomersUploadCommandHandler> logger,
    IOrganizationRepository orgRepository,
    ICustomerRepository customerRepository,
    IBackgroundQueue queue, 
    IExcelManager excelManager,
    IFileManager fileManager
) : CustomerBaseHandler, ICommandHandler<CustomersUploadCommand, IEnumerable<Customer>>
{

    public async Task<IEnumerable<Customer>> Handle(CustomersUploadCommand request, CancellationToken cancellationToken)
    {
        var fileCustomers = request.Customer != null
            ? [ new()
                {
                    Card = request.Customer.Card,
                    Name = request.Customer.Name,
                }]
            : excelManager.GetCustomers(fileManager.GetFile(request.FileReport)).ToList();

        var org = await orgRepository.GetItemAsync(request.OrganizationId, cancellationToken);
        if (org.Members.All(x => x.Name != request.User))
            throw new BusinessException($"Organization for '{request.User}' not found");
        var program = org.Programs.FirstOrDefault(x => x.Id == request.ProgramId)
            ?? throw EntityNotFoundException.For<Program>(request.ProgramId, $"in org '{org.Name}'");
        var wallet = program.Wallets.FirstOrDefault()
            ?? throw new BusinessException($"Program '{program.Name}' not contains wallet");
        
        Progress.CountAll = fileCustomers.Count;
        logger.LogInformation($"Try upload by {request.User} '{fileCustomers.Count}' customers");
        queue.UpdateProgress(request.TaskId, Progress);

        var customers = (await customerRepository.GetCustomersByCardsAsync(
            org.Id,
            fileCustomers.Select(x => x.Card),
            cancellationToken)).ToList();
        var client = new BizClient(org.Login, org.Password);
        var result = new List<Customer>();
        
        foreach (var fileCustomer in fileCustomers)
        {
            var customer = customers.FirstOrDefault(x => x.Cards.Any(c => c.Track == fileCustomer.Card));
            var isNewCustomer = false;
            if (customer == null)
            {
                //create if new customer
                isNewCustomer = true;
                customer = await TryCreateCustomer(client, org.Id, fileCustomer, cancellationToken);
                if (customer == null) continue;
            }
            //rename if enable option and not new customer
            if (request.OptionsModel.Rename && !isNewCustomer)
            {
                customer.Name = fileCustomer.Name;
                await client.UpdateCustomerAsync(customer.BizId, customer.Name, org.Id, cancellationToken);
            }
            
            //update second fields
            customer.TabNumber = fileCustomer.TabNumber;
            customer.Position = fileCustomer.Position;
            customer.Division = fileCustomer.Division;
            
            //update customers categories
            await UpdateCategories(client, customer, org, request.CategoriesId, cancellationToken);
            
            //try create wallet
            await TryCreateWallet(client, customer, wallet, org.Id, program.Id, cancellationToken);

            if (request.OptionsModel.RefreshBalance)
                await TryRefreshBalance(client, customer.BizId, org.Id, wallet.Id, request.Balance, cancellationToken);
            
            result.Add(customer);
            //update progress task
            queue.UpdateProgress(request.TaskId, Progress);
        }

        return result;
    }

    

    private async Task TryCreateWallet(BizClient client, Customer customer, Wallet wallet, Guid orgId, Guid programId, CancellationToken ct)
    {
        if (customer.Wallets.All(x => x.WalletId != wallet.Id))
        {
            try
            {
                if (await client.AddCustomerToProgramAsync(customer.BizId, orgId, programId, ct))
                {
                    customer.TryAddWallet(wallet.Id, wallet.Name, wallet.ProgramType, wallet.Type);
                }
            }
            finally
            {
                Progress.CountProgram++;
            }
            
        }
    }

   

    private async Task UpdateCategories(BizClient client, Customer customer, Organization org, 
        IEnumerable<Guid> categoriesId, CancellationToken ct)
    {
        foreach (var categoryId in categoriesId)
        {
            var category = org.Categories.FirstOrDefault(x=>x.Id == categoryId);
            if (category == null)
            {
                logger.LogError($"Category '{categoryId}' not found in '{org.Name}'");
                continue;
            }
            try
            {
                await client.AppendCategoryToCustomerAsync(customer.BizId, org.Id, categoryId, ct);
            }
            finally
            {
                Progress.CountCategory++;
            }
        }
    }


    private async Task<Customer?> TryCreateCustomer(BizClient client, Guid orgId, Excel.Models.Customer fileCustomer, CancellationToken ct)
    {
        var bizCustomer = await client.CreateCustomerAsync(fileCustomer.Name, fileCustomer.Card, orgId, ct);
        if (bizCustomer != Guid.Empty)
            Progress.CountNew++;
        else
        {
            Progress.CountFail++;
            logger.LogError($"Customer {fileCustomer.Name} {fileCustomer.Card} not create in biz");
            return null;
        }
        return new Customer(Guid.NewGuid(), 
            fileCustomer.Name, bizCustomer, orgId, true,
            string.Empty, fileCustomer.TabNumber, fileCustomer.Position, fileCustomer.Division
        );
    }
}