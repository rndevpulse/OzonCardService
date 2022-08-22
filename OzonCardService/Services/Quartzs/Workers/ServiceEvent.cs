using OzonCard.BizClient.Services.Interfaces;
using OzonCard.Context.Interfaces;
using OzonCard.Data.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OzonCardService.Services.Quartzs.Workers
{
    public interface IServiceEvent
    {
        Task UpdateEvents();

    }


    public class ServiceEvent : IServiceEvent
    {
        private readonly static ILogger log = Log.ForContext(typeof(ServiceEvent));
        private readonly IEventRepository _repository;
        private readonly IHttpClientService _client;

        public ServiceEvent(IHttpClientService client, IEventRepository repository)
        {
            _client = client;
            _repository = repository;
        }

        public async Task UpdateEvents()
        {
            log.Information("Start download events from biz");
            var organizations = await _repository.GetOrganizations();
            var tasks = new List<Task>();
            tasks.AddRange(organizations.ToList().Select(organization => Task.Run(async () =>
            //organizations.ToList().ForEach(async organization =>
            {
                var oldEvent = await _repository.GetLastEventOrganization(organization.Id);
                var now = DateTime.Now;
                //var now = new DateTime(2022, 8, 14);
                var first = new DateTime(now.Year, now.Month, 1);
                var dateFrom = oldEvent?.Create ?? first;
                log.Information($"Get report from biz organization '{organization.Name}'");
                var session = await _client.GetSession(organization.Login, organization.Password);
                var events = await _client.GerTransactionsReport(session, organization.Id,
                    dateFrom.ToString("yyyy-MM-dd"),
                    now.ToString("yyyy-MM-dd"),
                    null);
                log.Information($"download '{events.Count()}' events in '{organization.Name}' from {dateFrom.ToString("yyyy-MM-dd")} to {now.ToString("yyyy-MM-dd")}");
                var rows = await _repository.AppendEventsOrganization(events?.Select(x =>
                    new OzonCard.Data.Models.Event
                    {
                        OrganizationId = x?.organizationId ?? organization.Id,
                        Create = x.transactionCreateDate,
                        OrderCreate = x?.orderCreateDate ?? null,
                        OrderNumber = x?.orderNumber ?? 0,
                        TransactionSum = x?.transactionSum ?? 0,
                        TransactionType = x?.transactionType ?? string.Empty,
                        OrderSum = x?.orderSum ?? 0,
                        CardNumbers = x?.cardNumbers ?? string.Empty,
                        Comment = x?.comment ?? string.Empty,
                        ProgramName = x?.programName ?? string.Empty,
                        MarketingCampaignName = x?.marketingCampaignName ?? string.Empty,
                    }));
                log.Information($"add to database '{rows}' events in '{organization.Name}' from {dateFrom.ToString("yyyy-MM-dd")} to {now.ToString("yyyy-MM-dd")}");
                var customers = await _repository.GetCustomersOrganization(organization.Id);
                var newCustomersCard = customers.SelectMany(x => x.Cards.Select(c => c.Number));
                newCustomersCard = events?.Select(x => x.cardNumbers).Distinct().Except(newCustomersCard).ToArray();
                if (newCustomersCard.Any())
                {
                    log.Information($"In '{organization.Name}' find {newCustomersCard.Count()} new customers from {dateFrom.ToString("yyyy-MM-dd")} to {now.ToString("yyyy-MM-dd")}");
                    var newCustomers = new List<Customer>();
                    //запрашиваем у биза потеряшек
                    foreach (var card in newCustomersCard)
                    { 
                        var biz = await _client.GetCustomerForCard(session, card, organization.Id);
                        if (biz == null)
                            continue;
                        var customer = new Customer();
                        customer.iikoBizId = biz.id;
                        customer.Name = biz.name;
                        customer.TabNumber = String.Empty;
                        customer.Position = String.Empty;
                        customer.Organization = organization;
                        customer.Cards.Add(new Card(card));
                        foreach( var bizCategory in biz.categories)
                            customer.Categories.Add(new CategoryCustomer { 
                                Category = organization.Categories.FirstOrDefault(c => c.Id == bizCategory.id), 
                                Customer = customer 
                            });
                        foreach (var wallet in biz.walletBalances)
                        {
                            CustomerWallet customerWallet = new CustomerWallet();
                            customerWallet.Wallet = organization.CorporateNutritions
                                .Select(x=>x.Wallets.FirstOrDefault(w=>w.Id == wallet.wallet.id)).First();
                            customer.Wallets.Add(customerWallet);
                        }

                        newCustomers.Add(customer);
                    }
                    await _repository.AttachRangeCustomer(newCustomers);
                }

            })));
            Task.WaitAll(tasks.ToArray());
            log.Information("COMPLETED download events from biz");
        }
    }
}
