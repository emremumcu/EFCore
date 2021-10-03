
Providers and tools
``` batch
PM> Install-Package Microsoft.EntityFrameworkCore.SQLite
PM> Install-Package Microsoft.EntityFrameworkCore.Tools
> dotnet tool install --global dotnet-ef
```


Code First
``` batch
PM> add-migration Mig0 -OutputDir AppLib/Migrations
> dotnet ef migrations add Mig0 -o AppLib/Migrations
PM> update-database [-migration Mig0] [–verbose]
> dotnet ef database update [Mig0] 
```

DB First
``` batch
> dotnet ef dbcontext scaffold "Server=.\SQLEXPRESS;Database=SchoolDB;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -o Models
```