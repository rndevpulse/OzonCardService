using Hellang.Middleware.ProblemDetails;
using System.Globalization;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using OzonCard.Common.Core.Exceptions;
using OzonCard.Common.Infrastructure.Database;
using OzonCard.Common.Infrastructure.Extensions;
using OzonCard.Common.Logging;
using OzonCard.Excel;
using OzonCard.Files;
using OzonCard.Identity.Domain;
using OzonCard.Identity.Infrastructure.Jwt;
using OzonCard.Identity.Infrastructure.Security;

var assemblies = new[]
{
    Assembly.GetExecutingAssembly(),
    Assembly.Load("OzonCard.Common.Infrastructure"), 
    Assembly.Load("OzonCard.Common.Domain"), 
    Assembly.Load("OzonCard.Common.Application"), 
    Assembly.Load("OzonCard.Identity"), 
};
var cultures = new[]
{
    new CultureInfo("ru"),
    new CultureInfo("en-US")
};


var builder = WebApplication.CreateBuilder(args);
builder.UseDefaultLogging();

#region Identity

builder.Services.AddIdentityCore<User>()
    .AddRoles<UserRole>()
    .AddSignInManager()
    .AddEntityFrameworkStores<SecurityContext>()
    .AddDefaultTokenProviders()
    .AddRefreshTokenProvider<User>();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false; 
    options.Password.RequireUppercase = false;
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
builder.Services.AddScoped<IJwtGenerator, JwtGenerator>();

builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy(UserRole.Admin, policy => policy.RequireRole(UserRole.Admin));
    opt.AddPolicy(UserRole.Report, policy => policy.RequireRole(UserRole.Report));
    opt.AddPolicy(UserRole.Basic, policy => policy.RequireRole(UserRole.Basic));
});
#endregion

builder.Services.AddLocalization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAutoMapper(assemblies);
builder.Services.AddMemoryCache();

builder.Services.AddInfrastructure(opt =>
    opt.SetAssemblies(assemblies)
        .SetConnection(builder.Configuration.GetConnectionString("service"))
        .SetDevEnvironment(builder.Environment.IsDevelopment())
);



#region Problem details

builder.Services.AddProblemDetails(options =>
{
    options.Map<BusinessException>(exception => new ProblemDetails
    {
        Status = StatusCodes.Status400BadRequest,
        Title = "Bad Request",
        Detail = exception.Message,
    });
    options.Map<EntityNotFoundException>(exception => new ProblemDetails
    {
        Status = StatusCodes.Status404NotFound,
        Title = "Not found",
        Detail = exception.Message,
    });
    options.Map<Exception>(exception => new ProblemDetails
    {
        Status = StatusCodes.Status500InternalServerError,
        Title = "Server Error",
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

#region OtherStaff

builder.Services.AddScoped<IFileManager, FileManager>();
builder.Services.AddScoped<IExcelManager, ExcelManager>();

#endregion


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors(cors => cors.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
}

app.UseCors(policy =>
{
    policy.WithOrigins(
       builder.Configuration.GetValue<string>("origin", "http://localhost:3000")!
    ).AllowAnyMethod()
        .AllowCredentials()
        .AllowAnyHeader();
});
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
