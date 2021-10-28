
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


var result = fruits.OrderBy(f => f, StringComparer.CurrentCulture);
// or
CultureInfo culture = new CultureInfo("sv-SE");
var result = fruits.OrderBy(f => f, StringComparer.Create(culture, false));

Entity Framework Core evaluates a LINQ query on the server side as much as possible.
Entity Framework Core blocks any client evaluation.
.NET Core 3.1 and above doesn’t support client evaluation.

dotnet ef database drop -p ProjectHavingContext -s StartupProject
dotnet ef migrations remove -p ProjectHavingContext -s StartupProject
dotnet ef migrations add InitialCreate -p ProjectHavingContext -s StartupProject -o OutputModelFolder