﻿using AutoMapper;
using OzonCard.BizClient.Services.Interfaces;
using OzonCard.Common;
using OzonCard.Context.Interfaces;
using OzonCard.Data.Models;
using OzonCard.Excel;
using OzonCardService.Models.DTO;
using OzonCardService.Models.View;
using OzonCardService.Services.Interfaces;
using OzonCardService.Services.TasksManagerProgress.Implementation;
using OzonCardService.Services.TasksManagerProgress.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
            
            var session = await _client.GetSession(IdentityOrganization.Email, IdentityOrganization.Password);
            var organizations = _mapper.Map<IEnumerable<Organization>>(await _client.GetOrganizations(session));
            
            var Tasks = new List<Task>();
            foreach (var organization in organizations)
            {
                organization.Login = IdentityOrganization.Email;
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





        public async Task<bool> AddUser(UserCreate_vm _user)
        {
            var user = new User
            {
                Mail = _user.Email,
                Password = UserHelper.GetHash(_user.Password),
                CreatedDate = DateTime.UtcNow,
                Rules = string.Join(',', _user.Rules)
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


        public async Task<IEnumerable<File_dto>> GetFiles(Guid userId)
        {
            var files = await _repository.GetFiles(userId);
            return _mapper.Map<IEnumerable<File_dto>>(files);
        }



        /// <summary>
        /// Выгрузка списка пользователей в биз
        /// </summary>
        public async Task UploadCustomers(Guid userId, InfoCustomersUpload_vm infoUpload, List<ShortCustomerInfo_excel> customers_excel, IProgress<ProgressInfo> progress, CancellationToken token)
        {
            var info = new InfoDataUpload_dto();
            info.CountCustomersAll = customers_excel.Count();
            progress.Report(new ProgressInfo(info));
            var organization = await _repository.GetOrganization(infoUpload.OrganizationId) ??
                throw new ArgumentException($"Organization with {infoUpload.OrganizationId} not found");
            if (!organization.Users.Any(x => x.Id == userId))
                throw new ArgumentException($"Organization with {infoUpload.OrganizationId} not found in current user");
            var customers_rep = _repository.GetCustomersForCardNumber(customers_excel.Select(x => x.Card))
                .Result.ToList();

            var category = organization.Categories.First(x=>x.Id == infoUpload.CategoryId);
            var corporateNutrition = organization.CorporateNutritions.First(x=>x.Id == infoUpload.CorporateNutritionId);

            if (token.IsCancellationRequested)
                return;

            var session = await _client.GetSession(organization.Login, organization.Password);
            ///1. Находим разницу customers - rep_customers = получим новых пользователей, которых нет в бд
            var customers_rep_cards = customers_rep.SelectMany(x => x.Cards).Select(x => x.Track);
            var customers_excel_new = customers_excel.ToList();
            customers_excel_new.RemoveAll(x => customers_rep_cards.Contains(x.Card));

            var new_customers = new List<Customer>();
            ///ебучая задержка для отладки фронта
            //System.Threading.Thread.Sleep(1000 *300);//5 min

            foreach (var customer_excel in customers_excel_new)
            {
                if (token.IsCancellationRequested)
                    break;
                ///=> для каждого из нового списка выполняем
                ///0.1 Ищем гостя в бизе по карте
                ///0.2 Если нашли, то берем все данные оттуда
                ///0.3 Если не нашли то -> 1.1
                ///1.1 Создаем нового гостя в бизе с картой
                ///1.2 Присваиваем гостю категорию
                ///1.3 Назначаем гостю кошелек в программе
                ///1.4 Изменяем при необходимости баланс
                var res = await UploadCustomerInBiz(infoUpload, session, customer_excel, organization, category, corporateNutrition);
                info += res.Value;
                progress.Report(new ProgressInfo(info));

                if (res.Key != null)
                    new_customers.Add(res.Key);
            }

            ///1.5 Сохраняем новых в бд
            if (new_customers.Count > 0)
                await _repository.AttachRangeCustomer(new_customers);
            if (token.IsCancellationRequested)
                return;
            ///2. Проходимся по списку rep_customers
            ///3. Сохраняем в базу rep_customers со всеми изменениями

            foreach (var customer in customers_rep)
            {
                if (token.IsCancellationRequested)
                    break;

                var excel = customers_excel.FirstOrDefault(x => customer.Cards.Any(c => c.Track == x.Card));
                customer.TabNumber = customer.TabNumber == String.Empty
                    ? excel.TabNumber 
                    : customer.TabNumber ;
                customer.Position = customer.Position == String.Empty
                  ? excel.Position
                  : customer.Position;

                ///2.0 Изменяем имя если необходимо
                if (infoUpload.Options.Rename)
                {
                    customer.Name = excel.Name;
                    await _client.UpdateCustomer(session, customer.Name, customer.iikoBizId, organization.Id);
                }
                

                ///2.1 Если у пользователя нет категории присваиваем ее
                if (!customer.Categories.Any(x=>x.CategoryId == category.Id) )
                {
                    await _client.AddCategotyCustomer(session, customer.iikoBizId, organization.Id, category.Id);
                    //category.Customers.Add(customer);
                    customer.Categories.Add(new CategoryCustomer { Category = category, Customer = customer});
                    info.CountCustomersCategory++;
                }
                ///2.2 Если у пользователя нет кошелька создаем
                var wallet = corporateNutrition.Wallets.First();
                if (!customer.Wallets.Select(x=>x.Wallet).Any(x => wallet.Equals(x)))
                {
                    CustomerWallet customerWallet = new CustomerWallet();
                    await _client.AddCorporateNutritionCustomer(session, customer.iikoBizId, organization.Id, corporateNutrition.Id);
                    info.CountCustomersCorporateNutritions++;
                    customerWallet.Wallet = wallet;
                    customer.Wallets.Add(customerWallet);
                }
                ///2.3 Изменяем при необходимости баланс
                if (infoUpload.Options.RefreshBalance)
                {
                    await UpdateBalance(infoUpload.Balance, session, organization, customer.iikoBizId, wallet.Id);
                    info.CountCustomersBalance++;
                }
                progress.Report(new ProgressInfo(info));

                await _repository.UpdateCustomer(customer);
            }
            //await _repository.UpdateCategory(category);
        }

        async Task<KeyValuePair<Customer, InfoDataUpload_dto>> UploadCustomerInBiz(InfoCustomersUpload_vm infoUpload,
            OzonCard.BizClient.Models.Data.Session session, ShortCustomerInfo_excel customer_excel, 
            Organization organization, Category category, CorporateNutrition corporateNutrition)
        {
            var customer = new Customer();
            var info = new InfoDataUpload_dto();

            //1.1
            customer.iikoBizId = await _client.CreateCustomer(session, customer_excel.Name, customer_excel.Card, organization.Id);
            if (customer.iikoBizId == Guid.Empty)
            {
                info.CountCustomersFail++;
                log.Error($"Customer {customer_excel.Name} {customer_excel.Card} not create in biz");
                return new KeyValuePair<Customer, InfoDataUpload_dto>(null, info);
            }

            customer.Name = customer_excel.Name;
            customer.TabNumber = customer_excel.TabNumber;
            customer.Position = customer_excel.Position;
            customer.Organization = organization;
            customer.Cards.Add(new Card(customer_excel.Card));

            info.CountCustomersNew++;
            //1.2
            if (await _client.AddCategotyCustomer(session, customer.iikoBizId, organization.Id, category.Id))
            {
                customer.Categories.Add(new CategoryCustomer { Category = category, Customer = customer });
                info.CountCustomersCategory++;
            }
            //1.3
            CustomerWallet customerWallet = new CustomerWallet();
            await _client.AddCorporateNutritionCustomer(session, customer.iikoBizId, organization.Id, corporateNutrition.Id);
            info.CountCustomersCorporateNutritions++;
            customerWallet.Wallet = corporateNutrition.Wallets.First();
            //1.4
            if (infoUpload.Options.RefreshBalance)
            {
                await UpdateBalance(infoUpload.Balance, session, organization, customer.iikoBizId, customerWallet.Wallet.Id);
                customerWallet.Balance = infoUpload.Balance;
                info.CountCustomersBalance++;
            }
            customer.Wallets.Add(customerWallet);
            return new KeyValuePair<Customer, InfoDataUpload_dto>(customer, info);
        }

        async Task UpdateBalance(double new_balance,
            OzonCard.BizClient.Models.Data.Session session, Organization organization, Guid iikoBizId, Guid WalletId)
        {
            var customer_balance = await _client.GetCustomerBalanceForId(session, iikoBizId, organization.Id, WalletId);
            if (customer_balance != null && customer_balance != new_balance)
            {
                var balance = (double)customer_balance;
                if (customer_balance != new_balance)
                {
                    if (customer_balance < new_balance)
                        await _client.AddBalanceByCustomer(session, iikoBizId, organization.Id, WalletId, new_balance - balance);
                    else
                        await _client.DelBalanceByCustomer(session, iikoBizId, organization.Id, WalletId, balance - new_balance);
                }
            }
        }




        public async Task<IEnumerable<ReportCN_dto>> PeriodReportBiz(Guid userId, ReportOption_vm reportOption, CancellationToken token)
        {
            //reportOption.DateTo = Convert.ToDateTime(reportOption.DateTo).AddDays(1).ToString("yyyy-MM-dd");

            var organization = await _repository.GetOrganization(reportOption.OrganizationId) ??
                throw new ArgumentException($"Organization with {reportOption.OrganizationId} not found");
            if (!organization.Users.Any(x => x.Id == userId))
                throw new ArgumentException($"Organization with {reportOption.OrganizationId} not found in current user");
            if (!organization.CorporateNutritions.Any(x => x.Id == reportOption.CorporateNutritionId))
                throw new ArgumentException($"CorporateNutrition with {reportOption.CorporateNutritionId} not found");


            var session = await _client.GetSession(organization.Login, organization.Password);
            var report = _mapper.Map<IEnumerable<ReportCN_dto>>(
                await _client.GerReportCN(
                    session,
                    reportOption.OrganizationId, reportOption.CorporateNutritionId,
                    reportOption.DateFrom,
                    reportOption.DateTo));
            if (token.IsCancellationRequested)
                return new List<ReportCN_dto>();
            //фильтрация пользователей по запрашиваемой категории, если необходимо
            if (reportOption.CategoryId != Guid.Empty)
            {
                var categoryName = organization.Categories.FirstOrDefault(x => x.Id == reportOption.CategoryId)?.Name;
                if (categoryName != String.Empty)
                {
                    report = report.Where(x=>x.guestCategoryNames?.Contains(categoryName) == true).ToList();
                }
            }


            var customers = await _repository.GetCustomersForOrganization(organization.Id);
            foreach (var row in report)
            {
                var customer = customers.FirstOrDefault(x => x.iikoBizId == row.guestId);
                if (customer != null)
                {
                    row.position = customer.Position;
                    row.employeeNumber = customer.TabNumber;
                }
            }
            return report;
        }


        public async Task SaveFile(Guid id, string format, string name, Guid UserId)
        {
            var file = new FileReport()
            {
                Id = id,
                Format = format,
                Name = name + "." + format,
                UserId = UserId
            };
            await _repository.AddFile(file);
        }

        public async Task RemoveFile(string url)
        {
            await _repository.RemoveFile(url);
        }

        public async Task<IEnumerable<InfoSearchCustomer_dto>> SearchCustomers(SearchCustomer_vm customer)
        {
            var organization = await _repository.GetOrganization(customer.OrganizationId) ??
                throw new ArgumentException($"Organization with {customer.OrganizationId} not found");
            if (!organization.CorporateNutritions.Any(x => x.Id == customer.CorporateNutritionId))
                throw new ArgumentException($"CorporateNutrition with {customer.CorporateNutritionId} not found");


            var customers_db = new List<Customer>();
            var customers_db_names = new List<Customer>();
            var customers_db_cards = new List<Customer>();
            if (customer.Name != String.Empty)
                customers_db_names.AddRange(await _repository.GetCustomersForName(customer.Name));
            if (customer.Card != String.Empty)
                customers_db_cards.AddRange(await _repository.GetCustomersForCardNumber(customer.Card));
            if (customer.Name != String.Empty && customer.Card != String.Empty)
                customers_db = customers_db_names.Intersect(customers_db_cards).ToList();
            else
            {
                customers_db.AddRange(customers_db_names);
                customers_db.AddRange(customers_db_cards);
            }
            var customer_dto = new List<InfoSearchCustomer_dto>();
            customers_db = customers_db.Where(x => x.Organization.Id == customer.OrganizationId).ToList();
            if (customers_db.Count == 0)
                return customer_dto;
            
            var session = await _client.GetSession(organization.Login, organization.Password);
            foreach(var c in customers_db)
            {
                var customer_biz = await _client.GetCustomerForId(session, c.iikoBizId, organization.Id);
                customer_biz.comment = organization.Name;
                customer_dto.Add(_mapper.Map<InfoSearchCustomer_dto>(customer_biz));
                    
            };
            DateTime now = DateTime.Now;
            var dateFrom = new DateTime(now.Year, now.Month, 1);
            var dateTo = dateFrom.AddMonths(1);
            //переделать но запрос отчета
            //ид организации есть
            //ид корпита передавать из веб формы
            //даты подставлять автоматически с начала месяца
            var report = await _client.GerReportCN(session, organization.Id, customer.CorporateNutritionId,
                dateFrom.ToString("yyyy-MM-dd"), dateTo.ToString("yyyy-MM-dd"));
            foreach (var c in customer_dto)
                c.SetMetrics(report.FirstOrDefault(x => x.guestId == c.Id));

            return customer_dto;

        }

        public async Task<TransactionsReport> TransactionsReportBiz(Guid userId, ReportOption_vm reportOption, CancellationToken token)
        {
            //reportOption.DateTo = Convert.ToDateTime(reportOption.DateTo).AddDays(1).ToString("yyyy-MM-dd");

            var organization = await _repository.GetOrganization(reportOption.OrganizationId) ??
                throw new ArgumentException($"Organization with {reportOption.OrganizationId} not found");
            if (!organization.Users.Any(x => x.Id == userId))
                throw new ArgumentException($"Organization with {reportOption.OrganizationId} not found in current user");
            var TransactionsReport = new TransactionsReport();


            var session = await _client.GetSession(organization.Login, organization.Password);
            var dateTo_transaction = DateTime.Parse(reportOption.DateTo).AddDays(-1).ToString("yyyy-MM-dd");
            var biz_transactions =
                await _client.GerTransactionsReport(session, reportOption.OrganizationId, reportOption.DateFrom, dateTo_transaction);
            if (token.IsCancellationRequested || !biz_transactions.Any())
                return TransactionsReport;
            var biz_report = await _client.GerReportCN(
                    session, reportOption.OrganizationId, reportOption.CorporateNutritionId,
                    reportOption.DateFrom, reportOption.DateTo);
            var customers = await _repository.GetCustomersForOrganization(organization.Id);
            if (token.IsCancellationRequested)
                return TransactionsReport;


            TransactionsReport.Transactions = 
                (from t in biz_transactions
                join r in biz_report on t.cardNumbers equals r.guestCardTrack
                join c in customers on r.guestId equals c.iikoBizId
                select new TransactionsReport_dto()
                {
                    Date = t.transactionCreateDate.ToString("yyyy-MM-dd"),
                    Time = t.transactionCreateDate.ToString("HH:mm:ss"),
                    TabNumber = c.TabNumber,
                    Name = c.Name,
                    Division = c.Position,
                    Categories = r.guestCategoryNames,
                    СardNumbers = r.guestCardTrack,
                    Eating = GetNameEating(t.transactionCreateDate)
                }).ToList();

            var reportSummary = new List<TransactionsSummaryReport_dto>();
            foreach (var group_customer in TransactionsReport.Transactions.GroupBy(x=>x.Name))
            {
                var customer = group_customer.First();
                reportSummary.Add(new TransactionsSummaryReport_dto()
                {
                    Name = group_customer.Key,
                    Categories = customer.Categories,
                    Division = customer.Division,
                    CountDay = group_customer.GroupBy(x=>x.Date).Count()
                });

            }
            TransactionsReport.TransactionsSummary = reportSummary;
            return TransactionsReport;
        }
        string GetNameEating(DateTime date)
        {
            var time = date.TimeOfDay;
            if (time > new TimeSpan(3, 0, 0) && time <= new TimeSpan(11, 0, 0))
                return "Завтрак";
            if (time > new TimeSpan(11, 0, 0) && time <= new TimeSpan(16, 0, 0))
                return "Обед";
            if (time > new TimeSpan(16, 0, 0) && time <= new TimeSpan(21, 0, 0))
                return "Ужин";
            return "Ночной ужин";
        }
    }
}
