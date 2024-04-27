using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OzonCard.Identity.Domain;

namespace OzonCard.Common.Infrastructure.Database;

public class SecurityContext(DbContextOptions<SecurityContext> options) : IdentityDbContext<User, UserRole, string>(options)
{
    private const string Schema = "security";

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>().ToTable("user", Schema);
        builder.Entity<UserRole>().ToTable("role", Schema);
        builder.Entity<IdentityUserToken<string>>().ToTable("user_tokens", Schema);
        builder.Entity<IdentityUserRole<string>>().ToTable("user_roles", Schema);
        builder.Entity<IdentityUserClaim<string>>().ToTable("user_claims", Schema);
        builder.Entity<IdentityRoleClaim<string>>().ToTable("role_claims", Schema);
        builder.Entity<IdentityUserLogin<string>>().ToTable("user_logins", Schema);
        builder.Entity<UserRole>().HasData(GetInitialRoles());

        // User Administrator
        builder.Entity<User>().HasData(GetInitialUsers());
        builder.Entity<IdentityUserRole<string>>().HasData(GetInitialUsersRole());
        
    }

    private static IEnumerable<UserRole> GetInitialRoles()
    {
        yield return new UserRole
        {
            Id = new Guid("21D938A6-66E3-4AAD-ABBC-9A176E1AE506").ToString(),
            ConcurrencyStamp = Guid.NewGuid().ToString("N"),
            Name = UserRole.Basic,
            NormalizedName = UserRole.Basic.ToUpper(),
        };

        yield return new UserRole
        {
            Id = new Guid("E88FBC30-1985-4379-AE4C-E24657835212").ToString(),
            ConcurrencyStamp = Guid.NewGuid().ToString("N"),
            Name = UserRole.Report,
            NormalizedName = UserRole.Report.ToUpper()
        };
        yield return new UserRole
        {
            Id = new Guid("44064C53-4CD3-472C-9895-EABF9464DC2D").ToString(),
            ConcurrencyStamp = Guid.NewGuid().ToString("N"),
            Name = UserRole.Admin,
            NormalizedName = UserRole.Admin.ToUpper()
        };
    }
    
    private static IEnumerable<User> GetInitialUsers()
    {
        yield return new User
        {
            Id = new Guid("4cc34a7e-3c7f-4b92-9279-b7b2def75fff").ToString(),
            UserName = "kolur_20@bk.ru",
            NormalizedUserName = "KOLUR_20@BK.RU",
            Email = "kolur_20@bk.ru",
            NormalizedEmail = "KOLUR_20@BK.RU",
            EmailConfirmed = true,
            PasswordHash = "AQAAAAIAAYagAAAAEJJrBy0pq8RJrN8I6i77dfBhHuSE9ulo98YLCchlOwDExKR2BNk6swyNIIOrFtOBPA==",//ozoncard
            SecurityStamp = "XJ5DBWJXEXC4HELI3LEARREZ6VDY4YBS",
            ConcurrencyStamp = new Guid("cf3b4531-7b1a-4382-bba4-c39e548b5ad4").ToString(),
        };
    }
    private static IEnumerable<IdentityUserRole<string>> GetInitialUsersRole()
    {
        yield return new IdentityUserRole<string>
        {
            UserId = new Guid("4cc34a7e-3c7f-4b92-9279-b7b2def75fff").ToString(),
            RoleId = new Guid("21D938A6-66E3-4AAD-ABBC-9A176E1AE506").ToString(),
        };
        yield return new IdentityUserRole<string>
        {
            UserId = new Guid("4cc34a7e-3c7f-4b92-9279-b7b2def75fff").ToString(),
            RoleId = new Guid("E88FBC30-1985-4379-AE4C-E24657835212").ToString(),
        };
        yield return new IdentityUserRole<string>
        {
            UserId = new Guid("4cc34a7e-3c7f-4b92-9279-b7b2def75fff").ToString(),
            RoleId = new Guid("44064C53-4CD3-472C-9895-EABF9464DC2D").ToString(),
        };
    }
    
}