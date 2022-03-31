using OzonCard.BizClient.HttpClients;
using OzonCard.BizClient.Models;
using OzonCard.BizClient.Models.Data;
using OzonCard.BizClient.Services.Interfaces;

namespace OzonCard.BizClient.Services.Implementation
{

    public class HttpClientService : IHttpClientService
    {
        readonly IClient _client;
        static Dictionary<Identification, Session> TokenOrganizations = new Dictionary<Identification, Session>();
        const int TimeLife = 14;
        public const string URL = "https://iiko.biz:9900//api/0/";
        public HttpClientService(IClient httpClient)
        {
            _client = httpClient;
        }

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


        public async Task<IEnumerable<Organization>> GetOrganizations(Session session)
        {
            session = await SessionAlive(session);
            return await _client.Send<IEnumerable<Organization>>($"organization/list?access_token={session.Token}")
                ?? throw new HttpRequestException();
        }

        public async Task<Session?> GetSession(string login, string password)
        {
            return await GetSession(new Identification()
            {
                Login = login,
                Password = password
            });
        }
    }
}
