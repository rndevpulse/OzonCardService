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
    }
}
