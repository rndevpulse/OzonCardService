// See https://aka.ms/new-console-template for more information

using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OzonCard.Common.Application.Customers;
using OzonCard.Common.Application.Organizations;
using OzonCard.Common.Infrastructure.Database;
using OzonCard.Common.Infrastructure.Extensions;
using OzonCard.Common.Logging;
using OzonCard.Tools.Categories;
using OzonCard.Tools.Categories.Models;
using Serilog;

var services = new ServiceCollection();
var assemblies = new[]
{
    Assembly.GetExecutingAssembly(),
    Assembly.Load("OzonCard.Common.Worker"), 
    Assembly.Load("OzonCard.Common.Infrastructure"), 
    Assembly.Load("OzonCard.Common.Domain"), 
    Assembly.Load("OzonCard.Common.Application"), 
    Assembly.Load("OzonCard.Identity"), 
};
var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json")
    .Build();
services.AddInfrastructure(opt =>
{
    opt.Assemblies = assemblies;
    opt.Connection = configuration.GetConnectionString("service") ?? "";
    opt.Provider = configuration.GetConnectionString("provider") ?? "";
    opt.IsDevelopment = true;
});
services.UseDefaultLogging(configuration);

var serviceProvider = services.BuildServiceProvider();


var files = Directory.GetFiles(
    Path.Combine(AppContext.BaseDirectory, "content"),
    "*.csv");

var transaction = serviceProvider.GetRequiredService<ITransactionManager>();
var organizations = serviceProvider.GetRequiredService<IOrganizationRepository>();
var customers = serviceProvider.GetRequiredService<ICustomerRepository>();

foreach (var file in files)
{
    if (!Guid.TryParse(Path.GetFileNameWithoutExtension(file), out Guid guid))
    {
        Log.Warning("File '{File}' is not a valid GUID", file);
        continue;
    }

    var customersFile = Reader.Load<Customer>(file);
    Log.Information("Processing file '{File}' found {Count} cards", file, customersFile.Count());
    
    transaction.StartTransaction();

    var organization = await organizations.GetItemAsync(guid);
    var domainCustomers = await customers.GetItemsAsync(organization.Id);
    
    Log.Information("Processing organization '{Organization}'", organization.Name);
    var notFound = 0;
    var allCategories = 0;
    var changeCategories = 0;
    // var categories = organization.Categories.Where(x=>x.IsActive).ToList();
    var categories = organization.Categories.Where(x=>x.IsActive).ToList();
    foreach (var customer in customersFile)
    {
        var domainCustomer = domainCustomers.FirstOrDefault(x => x.Cards.Any(c => c.Number == customer.Card));
        if (domainCustomer == null)
        {
            Log.Warning("Customer {Customer} is not found", customer.Card);
            notFound++;
            continue;
        }
        allCategories += customer.Categoties.Count();
        categories.Where(c =>
            customer.Categoties.Contains(c.Name.Replace("\"",""))).ToList().ForEach(c =>
        {
            changeCategories++;
            domainCustomer.AddCategory(c);
        });
        if (domainCustomer.Categories.Count() != customer.Categoties.Count())
        {
            Log.Warning("One or more categories not processed: {Categories}", string.Join(",", customer.Categoties));
        }
    }
    await transaction.CommitAsync();
    Log.Information("Processing organization '{Organization}': change categories {Change}/{All}. Skip customers {Count}",
        organization.Name,
        changeCategories,
        allCategories,
        notFound);
}
Log.Information("Finished processing files");
Console.WriteLine();

