using Hellang.Middleware.ProblemDetails;
using System.Globalization;
using System.Reflection;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using OzonCard.Common.Core;
using OzonCard.Common.Core.Exceptions;
using OzonCard.Common.Infrastructure;
using OzonCard.Common.Infrastructure.Buses;
using OzonCard.Common.Infrastructure.Database;
using OzonCard.Common.Infrastructure.Database.Materialization;
using OzonCard.Common.Infrastructure.Piplines;
using OzonCard.Common.Infrastructure.Security;
using OzonCard.Customer.Api.Services.Bootstrap;

var assemblies = new[]
{
    Assembly.GetExecutingAssembly(),
    Assembly.Load("OzonCard.Common.Infrastructure"), 
    Assembly.Load("OzonCard.Common.Domain"), 
    Assembly.Load("OzonCard.Common.Application"), 
};
var cultures = new[]
{
    new CultureInfo("ru"),
    new CultureInfo("en-US")
};


var builder = WebApplication.CreateBuilder(args);

#region Identity

builder.Services.AddIdentityCore<User>()
    .AddEntityFrameworkStores<SecurityContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false; //set false 20.12.23
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;

    // SignIn settings
    options.SignIn.RequireConfirmedEmail = false;
});

#endregion

builder.Services.AddLocalization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(assemblies);
builder.Services.AddMemoryCache();
builder.Services.AddMediatR(configuration => configuration.RegisterServicesFromAssemblies(assemblies));
builder.Services.AddScoped<ICommandBus, MediatrCommandBus>();
builder.Services.AddScoped<IQueryBus, MediatrQueryBus>();

#region Context

builder.Services.AddDbContext<InfrastructureContext>(b =>
    (builder.Environment.IsDevelopment() ? b.EnableSensitiveDataLogging() : b).UseSqlServer(
        builder.Configuration.GetConnectionString("service"),
        optionsBuilder => optionsBuilder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))
    .AddInterceptors(ContextMaterializationInterceptor.Instance));
builder.Services.AddDbContext<SecurityContext>(b =>
    (builder.Environment.IsDevelopment() ? b.EnableSensitiveDataLogging() : b).UseSqlServer(
        builder.Configuration.GetConnectionString("service")));

builder.Services.AddScoped<ITransactionManager>(sp => sp.GetRequiredService<InfrastructureContext>());
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionPipeline<,>));

#endregion


#region Problem details

builder.Services.AddProblemDetails(options =>
{
    options.Map<BusinessException>(exception => new ProblemDetails
    {
        Status = StatusCodes.Status400BadRequest,
        Title = "Bad Request",
        Detail = exception.Message,
    });
});

#endregion

#region Api versioning

builder.Services.AddApiVersioning(o =>
{
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.DefaultApiVersion = new ApiVersion(1, 0);
});

builder.Services.AddVersionedApiExplorer(o =>
{
    o.GroupNameFormat = "'v'VVV";
    o.SubstituteApiVersionInUrl = true;
});

#endregion

#region Auth

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(jwt =>
    {
        jwt.Audience = builder.Configuration.GetValue<string>("jwt:issuer");
        jwt.TokenValidationParameters = new TokenValidationParameters()
        {
            // укзывает, будет ли валидироваться издатель при валидации токена
            ValidateIssuer = true,
            // строка, представляющая издателя
            ValidIssuer = builder.Configuration.GetValue<string>("jwt:issuer"),
            // будет ли валидироваться потребитель токена
            ValidateAudience = true,
            // установка потребителя токена
            ValidAudience = builder.Configuration.GetValue<string>("jwt:audience"),
            // будет ли валидироваться время существования
            ValidateLifetime = true,

            // установка ключа безопасности
            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("jwt:key"))),
            // валидация ключа безопасности
            ValidateIssuerSigningKey = true,
        };
    });

#endregion


builder.Services.AddHostedService<DatabaseBootstrapService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors(cors => cors.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
}

app.UseRequestLocalization(options =>
{
    options.DefaultRequestCulture = new RequestCulture(cultures[0]);
    options.SupportedCultures = cultures;
});

app.UseStaticFiles();

app.MapFallbackToFile("/index.html");

app.UseProblemDetails();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();