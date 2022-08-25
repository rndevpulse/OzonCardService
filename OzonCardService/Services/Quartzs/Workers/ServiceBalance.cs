using AutoMapper;
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
    public interface IServiceBalance
    {
        Task UpdateAllBalanceCustomers();

    }
    public class ServiceBalance : IServiceBalance
    {
        private readonly static ILogger log = Log.ForContext(typeof(ServiceBalance));
        private readonly IBalanceRepository _repository;
        private readonly IHttpClientService _client;

        public ServiceBalance(IBalanceRepository repository, IHttpClientService httpClientService)
        {
            this._repository = repository;
            this._client = httpClientService;
        }
        public async Task UpdateAllBalanceCustomers()
        {
            log.Information("UpdateAllBalanceCustomers");
            var organizations = await _repository.GetOrganizations();
            //organizations = organizations.Where(x => x.Name.Contains("икро")).ToList();

            var tasks = new List<Task>();

            tasks.AddRange(organizations.ToList().Select(organization => Task.Run(async () =>
            //organizations.ToList().ForEach(async organization => 
            {
                  var customers = await _repository.GetCustomersOrganization(organization.Id);
                  customers = customers
                      .Where(x => !x.Categories
                          .Any(c => c.Category.Name.Contains("Удален") || c.Category.Name.Contains("Уволен"))
                      ).ToArray();
                log.Information($"UpdateAllBalanceCustomers: for {customers.Count()} customers in '{organization.Name}' START: current summ customers = {customers.Sum(x => x.Wallets.FirstOrDefault()?.Balance ?? 0)}");
                var session = await _client.GetSession(organization.Login,organization.Password);
                if (!customers.Any())
                    return;
                int size = 200;
                int i = 0;
                var wallet = organization.CorporateNutritions.First().Wallets.First();
                foreach(var g_customers in customers.GroupBy(s => i++ / size).Select(s => s.ToArray()).ToArray())
                {
                    var biz_balance = await _client.GetCustomersBalanceForIds(session, g_customers.Select(x=>x.iikoBizId), organization.Id, wallet.Id);
                    var timeUpdate = DateTime.UtcNow;
                    var wallets = g_customers.Join(biz_balance, c => c.iikoBizId, b => b.GuestId,
                        (c, b) => c.Wallets.Select(w => new CustomerWallet
                        {
                            Id = w.Id,
                            Balance = b.Balance,
                            Wallet = wallet,
                            Update = timeUpdate
                        })).SelectMany(x=>x).ToList();
                    await _repository.UpdateBalansCustomers(wallets);
                }
                log.Information($"UpdateAllBalanceCustomers: for {customers.Count()} customers in '{organization.Name}' COMPLETED");

            })));
            Task.WaitAll(tasks.ToArray());
            log.Information($"UpdateAllBalanceCustomers: COMPLETED");

        }
    }
}
