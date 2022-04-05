using AutoMapper;
using OzonCard.BizClient.Services.Interfaces;
using OzonCard.Common;
using OzonCard.Context.Interfaces;
using OzonCard.Data.Models;
using OzonCard.Excel;
using OzonCardService.Models.DTO;
using OzonCardService.Models.View;
using OzonCardService.Services.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OzonCardService.Services.Implementation
{
    public class RepositoryService : IRepositoryService
    {
        readonly IOrganizationRepository _repository;
        readonly IMapper _mapper;
        readonly IHttpClientService _client;
        private readonly ILogger log = Log.ForContext(typeof(RepositoryService));

        public RepositoryService(IOrganizationRepository repository, IMapper mapper, IHttpClientService httpClientService)
        {
            _repository = repository;
            _mapper = mapper;
            _client = httpClientService;
        }

     

        //взаимодействие с организациями
        public async Task<IEnumerable<Organization_dto>> GetMyOrganizations(Guid userId)
        {
            var organizations = await _repository.GetMyOrganizations(userId);
            return _mapper.Map<IEnumerable<Organization_dto>>(organizations);
        }
        public async Task<IEnumerable<Organization_dto>> AddOrganizations(Identity_vm IdentityOrganization, Guid userId)
        {
            //request to iikobiz to get the category and cn (IdentityOrganization.login, IdentityOrganization.pass)
            
            var session = await _client.GetSession(IdentityOrganization.Login, IdentityOrganization.Password);
            var organizations = _mapper.Map<IEnumerable<Organization>>(await _client.GetOrganizations(session));
            
            var Tasks = new List<Task>();
            foreach (var organization in organizations)
            {
                organization.Login = IdentityOrganization.Login;
                organization.Password = IdentityOrganization.Password;
                Tasks.Add(Task.Run(async () => organization.Categories =
                    _mapper.Map<List<Category>>(await _client.GetOrganizationCategories(session, organization.Id))));
                Tasks.Add(Task.Run(async () => organization.CorporateNutritions =
                    _mapper.Map<List<CorporateNutrition>>(await _client.GetOrganizationCorporateNutritions(session, organization.Id))));
            };
            Task.WaitAll(Tasks.ToArray());

            //adding
            await _repository.AddOrganizations(organizations, userId);
            return _mapper.Map<IEnumerable<Organization_dto>>(organizations);

        }

        public async Task<Organization_dto> UpdateOrganization(Guid userId, Guid organizationId)
        {
            var organization = await _repository.GetMyOrganization(userId, organizationId) ??
                throw new ArgumentException($"Organization with {organizationId} not found");
            var session = await _client.GetSession(organization.Login, organization.Password);
            var categories = _mapper.Map<IEnumerable<Category>>(await _client.GetOrganizationCategories(session, organizationId));
            await _repository.AddRangeCategory(categories, organization.Id);
            organization.Categories = categories.Where(x=>x.isActive).ToList();

            var corporateNutrition = _mapper.Map<IEnumerable<CorporateNutrition>>(await _client.GetOrganizationCorporateNutritions(session, organizationId));
            await _repository.AddRangeCorporateNutrition(corporateNutrition, organization.Id);
            organization.CorporateNutritions = corporateNutrition.Where(x => x.isActive).ToList();

            return _mapper.Map<Organization_dto>(organization);
        }





        public async Task<bool> AddUser(Identity_vm identity, string rules)
        {
            var user = new User
            {
                Mail = identity.Login,
                Password = UserHelper.GetHash(identity.Password),
                CreatedDate = DateTime.UtcNow,
                Rules = rules
            };
            await _repository.AddUser(user);
            return true;
        }

        public async Task<IEnumerable<User_dto>> GetUsers()
        {
            return _mapper.Map<IEnumerable<User_dto>>(await _repository.GetUsers());
        }

        public async Task<bool> AddUserForOrganization(Guid userId, Guid organizationId)
        {
            await _repository.AddUserForOrganization(userId, organizationId);
            return true;
        }

        public async Task<bool> DelUserForOrganization(Guid userId, Guid organizationId)
        {
            await _repository.DelUserForOrganization(userId, organizationId);
            return true;
        }

        public async Task SaveFile(Guid id, string format)
        {
            var file = new FileReport
            {
                Id = id,
                Format = format,
                Created = DateTime.UtcNow
            };
            await _repository.AddFile(file);
        }

        public async Task RemoveFiles(DateTime dateTime)
        {
            await _repository.RemoveFiles(dateTime);
        }


        /// <summary>
        /// Выгрузка списка пользователей в биз
        /// </summary>
        public async Task<InfoDataUpload_dto> UploadCustomers(Guid userId, InfoCustomersUpload_vm infoUpload, List<ShortCustomerInfo_excel> customers_excel)
        {
            var info = new InfoDataUpload_dto();
            info.CountCustomersAll = customers_excel.Count();
            var organization = await _repository.GetOrganization(infoUpload.OrganizationId) ??
                throw new ArgumentException($"Organization with {infoUpload.OrganizationId} not found");
            if (!organization.Users.Any(x => x.Id == userId))
                throw new ArgumentException($"Organization with {infoUpload.OrganizationId} not found in current user");
            var rep_customers = _repository.GetCustomersForTabNumber(customers_excel.Select(x => x.TabNumber))
                .Result.ToList();

            var category = organization.Categories.First(x=>x.Id == infoUpload.CategoryId);
            var corporateNutrition = organization.CorporateNutritions.First(x=>x.Id == infoUpload.CorporateNutritionId);
            var wallet = corporateNutrition.Wallets.First();

            var session = await _client.GetSession(organization.Login, organization.Password);
            ///1. Находим разницу customers - rep_customers = получим новых пользователей, которых нет в бд
            ///=> для каждого из нового списка выполняем
            ///1.1 Создаем нового гостя в бизе с картой
            ///1.2 Присваиваем гостю категорию
            ///1.3 Назначаем гостю кошелек в программе
            ///1.4 Изменяем при необходимости баланс
            ///1.5 Сохраняем его в бд
            ///2. Проходимся по списку rep_customers
            ///2.1 Если у пользователя нет категории присваиваем ее
            ///2.2 Если у пользователя не кашелька создаем
            ///2.3 Изменяем при необходимости баланс
            ///2.4 Изменяем имя если необходимо
            ///3. Сохраняем в базу rep_customers со всеми изменениями
            
            //1
            var rep_customers_tabNumbers = rep_customers.Select(x => x.TabNumber).ToList();
   
            customers_excel.RemoveAll(x => rep_customers_tabNumbers.Contains(x.TabNumber));
            var new_customers = new List<Customer>();
            foreach (var customer_excel in customers_excel)
            {
                var customer = new Customer();
                //1.1
                customer.iikoBizId = await _client.CreateCustomer(session, customer_excel.Name, customer_excel.Card, organization.Id);
                if (customer.iikoBizId == Guid.Empty)
                {
                    info.CountCustomersFail++;
                    log.Error($"Customer {customer_excel.Name} {customer_excel.Card} not create in biz");
                    continue;
                }
                customer.Name = customer_excel.Name;         
                customer.TabNumber = customer_excel.TabNumber;
                customer.Position = customer_excel.Position;
                customer.Organization = organization;
                customer.Cards.Add(new Card(customer_excel.Card));
                
                info.CountCustomersUpload++;
                //1.2
                if (await _client.AddCategotyCustomer(session, customer.iikoBizId, organization.Id, category.Id))   
                {
                    customer.Categories.Add(category);         
                    info.CountCustomersCategory++;
                }
                //1.3
                var customerWallet = new CustomerWallet();  
                customerWallet.Id = await _client.AddCorporateNutritionCustomer(session, customer.iikoBizId, organization.Id, corporateNutrition.Id);
                info.CountCustomersCorporateNutritions++;
                customerWallet.Wallet = wallet;
                //1.4
                if (infoUpload.Options.RefreshBalance)
                {
                    var customer_balance = await _client.GetCustomerBalanceForId(session, customer.iikoBizId, organization.Id, wallet.Id);
                    if (customer_balance != null && customer_balance != infoUpload.Balance)
                    {
                        var balance = (double)customer_balance;
                        if (customer_balance < infoUpload.Balance)
                            await _client.AddBalanceByCustomer(session, customer.iikoBizId, organization.Id, wallet.Id, (infoUpload.Balance - balance));
                        else
                            await _client.DelBalanceByCustomer(session, customer.iikoBizId, organization.Id, wallet.Id, (balance - infoUpload.Balance));
                        info.CountCustomersBalance++;
                        customerWallet.Balance = infoUpload.Balance;
                    }
                }
                customer.Wallets.Add(customerWallet);
                new_customers.Add(customer);
            }
            await _repository.AddRangeCustomer(new_customers);

            return info;
        }

        
    }
}
