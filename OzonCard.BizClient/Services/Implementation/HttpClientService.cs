using OzonCard.BizClient.HttpClients;
using OzonCard.BizClient.Models;
using OzonCard.BizClient.Models.Data;
using OzonCard.BizClient.Models.DTO;
using OzonCard.BizClient.Services.Interfaces;

namespace OzonCard.BizClient.Services.Implementation
{

    public class HttpClientService : IHttpClientService
    {
        readonly IClient _client;
        static Dictionary<Identification, Session> TokenOrganizations = new Dictionary<Identification, Session>();
        const int TimeLife = 14;
        public const string URL = "https://iiko.biz:9900/api/0/";
        public HttpClientService(IClient httpClient)
        {
            _client = httpClient;
        }

        #region Сервис методы
        async Task<Session> SessionAlive(Session session)
        {
            if (session.Created.AddMinutes(TimeLife) > DateTime.UtcNow)
                return session;
            //сессия истекла во время очередного запроса

            var key = TokenOrganizations.FirstOrDefault(x => x.Value == session).Key;
            if (key == null)
                return session;
            //если данные нашли, то обновляемся
            session = new Session()
            {
                Created = DateTime.UtcNow,
                Token = await CreateToken(key)
            };
            TokenOrganizations.Remove(key);
            TokenOrganizations.Add(key, session);
            return session;
        }
        public async Task<Session?> GetSession(Identification identification)
        {
            Session? session;
            try
            {
                session = TokenOrganizations[identification];
                if (session.Created.AddMinutes(TimeLife) > DateTime.UtcNow)
                    return session;
                //истек срок жизни, запрашиваем новый
                TokenOrganizations.Remove(identification);
                throw new KeyNotFoundException();
            }
            catch (ArgumentNullException) {  return null; }
            catch (KeyNotFoundException)
            {
                session = new Session()
                {
                    Token = await CreateToken(identification),
                    Created = DateTime.UtcNow
                };
                TokenOrganizations.Add(identification, session);
                return session;
            }
        }

        public async Task<string> CreateToken(Identification identification)
        {
            
            var token = await _client.Send<string>($"auth/access_token?user_id={identification.Login}&user_secret={identification.Password}") 
                ?? throw new HttpRequestException();
            return token;   
        }

        public async Task<Session?> GetSession(string login, string password)
        {
            return await GetSession(new Identification()
            {
                Login = login,
                Password = password
            });
        }
        #endregion


        public async Task<IEnumerable<Organization>> GetOrganizations(Session access_session)
        {
            try
            {
                return await _client.Send<IEnumerable<Organization>>($"organization/list?access_token={access_session.Token}")
                    ?? throw new HttpRequestException();
            }
            catch (UnauthorizedAccessException)
            {
                access_session = await SessionAlive(access_session);
                return await GetOrganizations(access_session);
            }
        }


        public async Task<IEnumerable<Category>> GetOrganizationCategories(Session access_session, Guid organizationId)
        {
            try
            {
                return await _client.Send<IEnumerable<Category>>($"organization/{organizationId}/guest_categories?access_token={access_session.Token}")
                    ?? throw new HttpRequestException();
            }
            catch (UnauthorizedAccessException)
            {
                access_session = await SessionAlive(access_session);
                return await GetOrganizationCategories(access_session, organizationId);
            }
        }

        public async Task<IEnumerable<CorporateNutrition>> GetOrganizationCorporateNutritions(Session access_session, Guid organizationId)
        {
            try
            {
                return await _client.Send<IEnumerable<CorporateNutrition>>($"organization/{organizationId}/corporate_nutritions?access_token={access_session.Token}")
                    ?? throw new HttpRequestException();
            }
            catch (UnauthorizedAccessException)
            {
                access_session = await SessionAlive(access_session);
                return await GetOrganizationCorporateNutritions(access_session, organizationId);
            }
        }





        public async Task<Guid> CreateCustomer(Session access_session, string name, string card, Guid organizationId)
        {
            try
            {
                var customer = new CustomerBiz_dto
                {
                    customer = new Customer_dto
                    {
                        name = name,
                        magnetCardNumber = card,
                        magnetCardTrack = card
                    }
                };
                return await _client.Send<Guid>($"customers/create_or_update?access_token={access_session.Token}&organization={organizationId}", "POST", customer);
            }
            catch (UnauthorizedAccessException)
            {
                access_session = await SessionAlive(access_session);
                return await CreateCustomer(access_session, name, card, organizationId);
            }
        }

        public async Task<Guid> UpdateCustomer(Session access_session, string name, Guid iikoBizId, Guid organizationId)
        {
            try
            {
                var customer = new CustomerBiz_dto
                {
                    customer = new Customer_dto
                    {
                        name = name,
                        id = iikoBizId
                    }
                };
                return await _client.Send<Guid>($"customers/create_or_update?access_token={access_session.Token}&organization={organizationId}", "POST", customer);
            }
            catch (UnauthorizedAccessException)
            {
                access_session = await SessionAlive(access_session);
                return await UpdateCustomer(access_session, name, iikoBizId, organizationId);
            }
        }


        public async Task<bool> AddCategotyCustomer(Session access_session, Guid iikoBizId, Guid organizationId, Guid categoryId)
        {
            try
            {
                await _client.Send<object>($"customers/{iikoBizId}/add_category?access_token={access_session.Token}&organization={organizationId}&categoryId={categoryId}", "POST");
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                access_session = await SessionAlive(access_session);
                return await AddCategotyCustomer(access_session, iikoBizId, organizationId, categoryId);
            }
        }
        public async Task<bool> DelCategotyCustomer(Session access_session, Guid iikoBizId, Guid organizationId, Guid categoryId)
        {
            try
            {
                await _client.Send<object>($"customers/{iikoBizId}/remove_category?access_token={access_session.Token}&organization={organizationId}&categoryId={categoryId}", "POST");
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                access_session = await SessionAlive(access_session);
                return await DelCategotyCustomer(access_session, iikoBizId, organizationId, categoryId);
            }
        }
        public async Task<Guid> AddCorporateNutritionCustomer(Session access_session, Guid iikoBizId, Guid organizationId, Guid corporateNutritionId)
        {
            try
            {
                return await _client.Send<Guid>($"customers/{iikoBizId}/add_to_nutrition_organization?access_token={access_session.Token}&organization={organizationId}&corporate_nutrition_id={corporateNutritionId}", "POST");
            }
            catch (UnauthorizedAccessException)
            {
                access_session = await SessionAlive(access_session);
                return await AddCorporateNutritionCustomer(access_session, iikoBizId, organizationId, corporateNutritionId);
            }
        }

        public async Task<Customer?> GetCustomerForId(Session access_session, Guid iikoBizId, Guid organizationId)
        {
            try
            {
                return await _client.Send<Customer>($"customers/get_customer_by_id?access_token={access_session.Token}&organization={organizationId}&id={iikoBizId}");
            }
            catch (UnauthorizedAccessException)
            {
                access_session = await SessionAlive(access_session);
                return await GetCustomerForId(access_session, iikoBizId, organizationId);
            }
        }

        public async Task<double?> GetCustomerBalanceForId(Session access_session, Guid iikoBizId, Guid organizationId, Guid walletId)
        {
            try
            {
                var customer = await GetCustomerForId(access_session, iikoBizId, organizationId);
                return customer?.walletBalances?.
                    FirstOrDefault(x => x.wallet.id == walletId)
                    ?.balance ?? null;
            }
            catch (UnauthorizedAccessException)
            {
                access_session = await SessionAlive(access_session);
                return await GetCustomerBalanceForId(access_session, iikoBizId, organizationId, walletId);
            }
        }

        public async Task<Customer?> GetCustomerForCard(Session access_session, string card, Guid organizationId)
        {
            try
            {
                return await _client.Send<Customer>($"customers/get_customer_by_card?access_token={access_session.Token}&organization={organizationId}&card={card}");
            }
            catch (UnauthorizedAccessException)
            {
                access_session = await SessionAlive(access_session);
                return await GetCustomerForCard(access_session, card, organizationId);
            }
        }



        public async Task<bool> AddBalanceByCustomer(Session access_session, Guid iikoBizId, Guid organizationId, Guid walletId, double balance)
        {
            try
            {
                var balance_dto = new Balance_dto()
                {
                    organizationId = organizationId,
                    walletId = walletId,
                    customerId = iikoBizId,
                    sum = balance
                };
                await _client.Send<object>($"customers/refill_balance?access_token={access_session.Token}", "POST", balance_dto);
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                access_session = await SessionAlive(access_session);
                return await AddBalanceByCustomer(access_session, iikoBizId, organizationId, walletId, balance);
            }
        }

        public async Task<bool> DelBalanceByCustomer(Session access_session, Guid iikoBizId, Guid organizationId, Guid walletId, double balance)
        {
            try
            {
                var balance_dto = new Balance_dto()
                {
                    organizationId = organizationId,
                    walletId = walletId,
                    customerId = iikoBizId,
                    sum = balance
                };
                await _client.Send<object>($"customers/withdraw_balance?access_token={access_session.Token}", "POST", balance_dto);
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                access_session = await SessionAlive(access_session);
                return await AddBalanceByCustomer(access_session, iikoBizId, organizationId, walletId, balance);
            }
        }

        public async Task<IEnumerable<ReportCN>> GerReportCN(Session access_session, Guid organizationId, Guid corporateNutritionId, string dateFrom, string dateTo)
        {
            try
            {
                return await _client.Send<IEnumerable<ReportCN>>($"organization/{organizationId}/corporate_nutrition_report?corporate_nutrition_id={corporateNutritionId}&date_from={dateFrom}&date_to={dateTo}&access_token={access_session.Token}")
                ?? new List<ReportCN>();
            }
            catch (UnauthorizedAccessException)
            {
                access_session = await SessionAlive(access_session);
                return await GerReportCN(access_session, organizationId, corporateNutritionId, dateFrom, dateTo);
            }
        }
        public async Task<IEnumerable<TransactionsReport>> GerTransactionsReport(Session access_session, Guid organizationId, string dateFrom, string dateTo)
        {
            try
            {
                var type = TransactionType.PayFromWallet.ToString();
                var list = await _client.Send<IEnumerable<TransactionsReport>>($"organization/{organizationId}/transactions_report?date_from={dateFrom}&date_to={dateTo}&access_token={access_session.Token}");
                return list?.Where(x => x.transactionType == type).ToList()
                ?? new List<TransactionsReport>();
            }
            catch (UnauthorizedAccessException)
            {
                access_session = await SessionAlive(access_session);
                return await GerTransactionsReport(access_session, organizationId, dateFrom, dateTo);
            }
        }
        public async Task<IEnumerable<MetricCustomer>> GetMetricsCustomers(Session access_session, Guid organizationId, IEnumerable<Guid> guids)
        {
            try
            {
                MetricsCustomers_dto metrics = new MetricsCustomers_dto()
                {
                    guestIds = guids,
                    periods = new[] {CounterPeriod.Month},
                    metrics = new[] {CounterMetric.OrdersSum, CounterMetric.OrdersCount}
                };
                return await _client.Send<IEnumerable<MetricCustomer>>($"customers/get_counters_by_guests?organization={organizationId}&access_token={access_session.Token}", "POST", metrics)
                ?? new List<MetricCustomer>();
            }
            catch (UnauthorizedAccessException)
            {
                access_session = await SessionAlive(access_session);
                return await GetMetricsCustomers(access_session, organizationId, guids);
            }
        }


        public async Task<IEnumerable<GuestBalance>> GetCustomersBalanceForIds(Session access_session, IEnumerable<Guid> iikoBizIds, Guid organizationId, Guid walletId)
        {
            try
            {
                var ids = new { guestIds = iikoBizIds };
                return await _client.Send<IEnumerable<GuestBalance>>($"customers/get_balances_by_guests_and_wallet?organization={organizationId}&wallet={walletId}&access_token={access_session.Token}", "POST", ids)
                ?? new List<GuestBalance>();
            }
            catch (UnauthorizedAccessException)
            {
                access_session = await SessionAlive(access_session);
                return await GetCustomersBalanceForIds(access_session, iikoBizIds, organizationId, walletId);
}
        }
    }
}
