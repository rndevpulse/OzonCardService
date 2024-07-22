using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OzonCard.Common.Core;
using OzonCard.Common.Infrastructure.Buses;
using OzonCard.Common.Logging;

var assemblies = new[]
{
    Assembly.GetExecutingAssembly(),
    Assembly.Load("OzonCard.Common.Infrastructure"), 
    Assembly.Load("OzonCard.Common.Domain"), 
    Assembly.Load("OzonCard.Common.Application"), 
};
var builder = Host.CreateApplicationBuilder(args);

builder.UseDefaultLogging();


builder.Services.AddAutoMapper(assemblies);
builder.Services.AddMemoryCache();
builder.Services.AddMediatR(configuration => configuration.RegisterServicesFromAssemblies(assemblies));
builder.Services.AddScoped<ICommandBus, MediatrCommandBus>();
builder.Services.AddScoped<IQueryBus, MediatrQueryBus>();

