``` batch

# This package should be installed to the project containing the DbContext
PM> Install-Package Microsoft.EntityFrameworkCore.SqlServer

# This package should be installed to the startup project
PM> Install-Package Microsoft.EntityFrameworkCore.Design

# Rebuild the in Visual Studio after installing the Microsoft.EntityFrameworkCore.Design package

# Install dotnet ef CLI tools
> dotnet tool install --global dotnet-ef
> dotnet tool update --global dotnet-ef

# Create DbContext and DbSet

# Create migrations and update database
> dotnet ef migrations add InitialCreate -p <ProjectHavingDbContext> -s <StartupProject> -o EFCore/Migrations
> dotnet ef migrations add InitialCreate -p Lib -s UI -o EFCore/Migrations
> dotnet ef database update -p Lib -s UI
> dotnet ef database drop -p Lib -s UI
> dotnet ef migrations remove -p Lib -s UI

> Open dotnetcli (right click project file and select open in terminal)
> dotnet ef migrations add InitialCreate -o AppData/Migrations
> dotnet ef database update

```