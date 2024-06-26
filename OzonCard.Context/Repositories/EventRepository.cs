﻿using Microsoft.EntityFrameworkCore;
using OzonCard.Context.Interfaces;
using OzonCard.Data.Models;
using System.Text;

namespace OzonCard.Context.Repositories
{
    public class EventRepository : BaseRepository, IEventRepository
    {
        public EventRepository(string connectionString, IRepositoryContextFactory contextFactory) : base(connectionString, contextFactory)
        {
        }

        public async Task<IEnumerable<CustomerTransactions>> GetLastVisit(Guid organizationId, IEnumerable<string> cards)
        {
            using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                var r = await context.Events
                    .Where(x => x.TransactionType == "PayFromWallet" 
                                && x.OrganizationId == organizationId
                                && cards.Contains(x.CardNumbers))
                    .GroupBy(x => x.CardNumbers)
                    .Select(x=> new CustomerTransactions(x.Key, x.Max(r=>r.Create),x.Select(e=>e.Create)))
                    .ToListAsync();
                return r;
            }
        }

        public async Task<IEnumerable<(string card, int days)>> GetDaysGrants(Guid organizationId, 
            DateTime from, DateTime to,
            IEnumerable<string> cards)
        {
            using var context = ContextFactory.CreateDbContext(ConnectionString);
            var r = await context.Events
                .Where(x => x.TransactionType == "PayFromWallet"
                            && x.Create >= from && x.Create <= to
                            && x.OrganizationId == organizationId
                            && cards.Contains(x.CardNumbers))
                .GroupBy(x => x.CardNumbers)
                .Select(x=> new
                {
                    card = x.Key,
                    days = x.GroupBy(e=>e.Create.Date).Count()
                })
                .ToListAsync();
            return r.Select(x=>(x.card,x.days));
        }

        public async Task<int> AppendEventsOrganization(IEnumerable<Event> events)
        {
            if (events == null || events.Count() == 0)
                return 0;
            var organizationId = events.First().OrganizationId;
            var @event = await GetLastEventOrganization(organizationId);
            using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                var result = events.ToList();
                result.RemoveAll(x =>
                    x.TransactionType.Contains("DiscountSum")
                    || x.TransactionType.Contains("IncreaseSum")
                    );

                if (@event != null)
                    result.RemoveAll(x=>x.Create<=@event.Create);
                
                await context.AddRangeAsync(result);
                await context.SaveChangesAsync();
                return result.Count;
            }
        }

        public async Task<IEnumerable<Event>> GetEventsOrganization(Guid organizationId, DateTime dateFrom, DateTime dateTo)
        {
            dateFrom = new DateTime(dateFrom.Year, dateFrom.Month, dateFrom.Day, 0, 0, 1);
            dateTo = new DateTime(dateTo.Year, dateTo.Month, dateTo.Day, 23, 59, 59);
            using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                var @events = await context.Events
                    .Where(x=>x.OrganizationId == organizationId
                    && x.TransactionType == "PayFromWallet"
                    && x.Create>= dateFrom
                    && x.Create<=dateTo)
                    .ToListAsync();
                return @events;
            }
        }

        public async Task<IEnumerable<CustomerReport>> GetCustomersReportOrganization(Guid organizationId, DateTime dateFrom, DateTime dateTo, string programmName)
        {
            dateFrom = new DateTime(dateFrom.Year, dateFrom.Month, dateFrom.Day, 0, 0, 1);
            dateTo = new DateTime(dateTo.Year, dateTo.Month, dateTo.Day, 23, 59, 59);
            var report = new List<CustomerReport>();
            using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                var events = await context.Events
                    .Where(x => x.OrganizationId == organizationId
                    && x.ProgramName == programmName
                    && x.Create >= dateFrom
                    && x.Create <= dateTo)
                    .ToListAsync();
                if (!events.Any())
                    return report;

                var customers = await context.Customers
                    .Include(x=>x.Categories)
                    .ThenInclude(x=>x.Category)
                    .Include(x => x.Cards)
                    .Include(x => x.Organization)
                    .Where(x => x.Organization.Id == organizationId)
                    .ToListAsync();

                report = customers.Select(x => new CustomerReport { 
                    guestId = x.iikoBizId,
                    guestName = x.Name,
                    employeeNumber = x.TabNumber,
                    position = x.Position,
                    guestPhone = x.Phone,
                    guestCardTrack = string.Join(",", x.Cards.Select(c=>c.Track).OrderBy(x => x)),
                    guestCategoryNames = string.Join(",", x.Categories
                        .Where(c=>c.Category.isActive)
                        .Select(c => c.Category.Name).OrderBy(x=>x))
                }).ToList();

                var group = events.GroupBy(x => x.CardNumbers);
                report = (from g in @group
                        join c in report on g.Key equals c.guestCardTrack into rs
                        from c in rs.DefaultIfEmpty()
                        select new CustomerReport()
                        {
                            guestId = c?.guestId ?? Guid.Empty,
                            guestName = c?.guestName ?? string.Empty,
                            employeeNumber = c?.employeeNumber ?? string.Empty,
                            position = c?.position ?? string.Empty,
                            guestPhone = c?.guestPhone ?? string.Empty,
                            guestCardTrack = g.Key,
                            guestCategoryNames = c?.guestCategoryNames ?? string.Empty,
                            balanceRefillSum = g.Where(t => t.TransactionType.Contains("Refill"))
                                .Sum(x => x.TransactionSum),
                            payFromWalletSum = Math.Abs(g.Where(t => t.TransactionType.Contains("PayFromWallet"))
                                .Sum(x => x.TransactionSum)),
                            paidOrdersCount = g.Where(t => t.TransactionType.Contains("PayFromWallet")).Count() -
                               g.Where(t => t.TransactionType.Contains("CancelPayFromWallet")).Count()
                        }).ToList();



               
                return report;
            }
        }

        public async Task<Event?> GetLastEventOrganization(Guid organizationId)
        {
            using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                var @event = await context.Events
                    .OrderByDescending(x=>x.Create)
                    .FirstOrDefaultAsync(x=>x.OrganizationId == organizationId);
                return @event;
            }
        }

        public async Task<IEnumerable<Organization>> GetOrganizations()
        {
            using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                var organizations = await context.Organizations
                    .Include(x=>x.Categories)
                    .Include(x=>x.CorporateNutritions)
                    .ThenInclude(x=>x.Wallets)
                    .Include(x => x.Categories)
                    .ThenInclude(x => x.Customers)
                   .ToListAsync();
                return organizations;
            }
        }
        public async Task<IEnumerable<Customer>> GetCustomersOrganization(Guid organizationId)
        {
            using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                var customers = await context.Customers
                    .Where(x => x.Organization.Id == organizationId)
                    .Include(x => x.Cards)
                    .ToListAsync();
                return customers;
            }
        }
        public async Task AttachRangeCustomer(IEnumerable<Customer> customers)
        {
            using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                context.Customers.AttachRange(customers);

                await context.SaveChangesAsync();
            }
        }
        public async Task UpdateCategory(IEnumerable<CategoryCustomer> categories,bool isRemove)
        {
            var sb = new StringBuilder();
            foreach (var category in categories)
                if (isRemove)
                    sb.AppendLine($"DELETE FROM [CategoryCustomer] WHERE  [CategoryId] = '{category.CategoryId}' and [CustomerId] = '{category.CustomerId}';");
                else
                    sb.AppendLine($"INSERT INTO [CategoryCustomer] ([CustomerId], [CategoryId]) VALUES ('{category.CustomerId}','{category.CategoryId}');");
            using var context = ContextFactory.CreateDbContext(ConnectionString);
            await context.Database.ExecuteSqlRawAsync(sb.ToString());
        }

        public async Task SetCategories(IEnumerable<(Guid guestId, string guestCategoryNames)> enumerable, Guid organizationId)
        {
            var customers = await GetCustomersOrganization(organizationId);
            var categories = GetOrganizations().Result.FirstOrDefault(x=>x.Id == organizationId)!.Categories;

            var sb = new StringBuilder();
            enumerable = enumerable.Join(customers, e=>e.guestId, c=>c.iikoBizId,
                (e,c)=> (c.Id, e.guestCategoryNames)).ToArray();
            foreach (var custom in enumerable)
            {
                sb.AppendLine($"DELETE FROM [CategoryCustomer] WHERE [CustomerId] = '{custom.guestId}';");
                foreach (var c in categories.Where(x=>custom.guestCategoryNames.Contains(x.Name)).ToArray())
                {
                    sb.AppendLine($"INSERT INTO [CategoryCustomer] ([CustomerId], [CategoryId]) VALUES ('{custom.guestId}','{c.Id}');");
                }
            }
            using var context = ContextFactory.CreateDbContext(ConnectionString);
            await context.Database.ExecuteSqlRawAsync(sb.ToString());
        }
    }
}
