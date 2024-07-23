using System.Reflection;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OzonCard.Common.Infrastructure.Extensions;
using OzonCard.Common.Logging;
using OzonCard.Excel;
using OzonCard.Files;

var assemblies = new[]
{
    Assembly.GetExecutingAssembly(),
    Assembly.Load("OzonCard.Common.Infrastructure"), 
    Assembly.Load("OzonCard.Common.Domain"), 
    Assembly.Load("OzonCard.Common.Application"), 
};
var builder = WebApplication.CreateBuilder(args);

builder.UseDefaultLogging();


builder.Services.AddAutoMapper(assemblies);
builder.Services.AddMemoryCache();

builder.Services.AddInfrastructure(opt =>
    opt.SetAssemblies(assemblies)
        .SetConnection(builder.Configuration.GetConnectionString("service"))
        .SetDevEnvironment(builder.Environment.IsDevelopment())
        .SetServerWorker()
);


#region OtherStaff

builder.Services.AddScoped<IFileManager, FileManager>();
builder.Services.AddScoped<IExcelManager, ExcelManager>();

#endregion

var app = builder.Build();

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    IsReadOnlyFunc = _ => true 
});
app.Run();

