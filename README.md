OzonCardService
```shell
cd .\src\net\OzonCard.Customer.Api
```
**РАЗДЕЛЕНИЕ**
```shell
dotnet ef migrations add RestructuringMigration -p ../OzonCard.Database.Migrations.SqlServer -c InfrastructureContext -o Migrations/Operational -- --provider sqlserver
```
```shell
dotnet ef migrations add initTaskMigration -p ../OzonCard.Database.Migrations.Postgres -c TaskContext  -o Migrations/Task  -- --provider postgres
```

**ДО РАЗДЕЛЕНИЯ**


*InfrastructureContext*
```shell
dotnet ef migrations add JobPrograssDelTableMigration -c InfrastructureContext -p ..\OzonCard.Common.Infrastructure -o Database/Migrations/Operational 
```
*SecurityContext*
```shell
dotnet ef migrations add InitSecurityMigration -c SecurityContext -p ..\OzonCard.Common.Infrastructure -o Database/Migrations/Security
```
*TaskContext*
```shell
dotnet ef migrations add JobsTitleFieldMigration -c TaskContext -p ..\OzonCard.Common.Infrastructure -o Database/Migrations/Task
```

**REMOVE migrations**
```shell
dotnet ef migrations remove -c SecurityContext -p ..\OzonCard.Common.Infrastructure
```
```shell
dotnet ef migrations remove -c InfrastructureContext -p ..\OzonCard.Common.Infrastructure
```
