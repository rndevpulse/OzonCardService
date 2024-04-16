OzonCardService
```shell
cd .\src\net\OzonCard.Customer.Api\
```

dotnet ef migrations add InfrastructureContext
```shell
dotnet ef migrations add InitMigration -c InfrastructureContext -p ..\OzonCard.Common.Infrastructure -o Database/Migrations/Operational
```
dotnet ef migrations add SecurityContext
```shell
dotnet ef migrations add InitMigration -c SecurityContext -p ..\OzonCard.Common.Infrastructure -o Database/Migrations/Security
```

dotnet ef migrations remove
```shell
dotnet ef migrations remove -c SecurityContext -p ..\OzonCard.Common.Infrastructure
```
