/*     
    PM> Install-Package Microsoft.EntityFrameworkCore.SqlServer
    PM> Install-Package Microsoft.EntityFrameworkCore.Design -ProjectName <startupproject>
    # Rebuild soluiton after Install-Package Microsoft.EntityFrameworkCore.Design
    > dotnet tool install --global dotnet-ef
    > dotnet tool update --global dotnet-ef
    > dotnet ef migrations add InitialCreate -p <ProjectHavingDbContext> -s <StartupProject> -o EFCore/Migrations
    > dotnet ef migrations add InitialCreate -o AppData/EFCore/Migrations
    > dotnet ef database update -p SGKWeb.Lib -s SGKWeb.CMS.UI
    > dotnet ef database drop -p SGKWeb.Lib -s SGKWeb.CMS.UI
    > dotnet ef migrations remove -p SGKWeb.Lib -s SGKWeb.CMS.UI 

    dotnet ef migrations remove
    dotnet ef database drop

 */

// https://docs.microsoft.com/en-us/ef/core/modeling/relationships?tabs=fluent-api%2Cfluent-api-simple-key%2Csimple-key

namespace EFCore.AppLib.CodeFirst
{
    using EFCore.AppLib.CodeFirst.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    public class CodeFirstDbContext : DbContext
    {
        // public AppDbContext(DbContextOptions contextOptions) : base(contextOptions) { }
        // public AppDbContext(DbContextOptions<CMSDbContext> contextOptions) : base(contextOptions) { }

        /// <summary>
        /// This constructor is mandatory !!!
        /// A database provider can be configured by overriding the 'DbContext.OnConfiguring' method 
        /// or by using 'AddDbContext' on the application service provider. 
        /// If 'AddDbContext' is used, then also ensure that your DbContext type accepts a DbContextOptions<TContext> object 
        /// in its constructor and passes it to the base constructor for DbContext.
        /// </summary>
        public CodeFirstDbContext()
        {
            /// Database.EnsureCreated(); 
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            if (!optionsBuilder.IsConfigured)
            {
                // string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                // IConfigurationBuilder builder = new ConfigurationBuilder();
                // IConfigurationRoot configuration = new ConfigurationBuilder()
                //    .SetBasePath(Directory.GetCurrentDirectory())
                //    .AddJsonFile("data.json")
                //    .Build();
                // var connectionString = configuration.GetConnectionString("CMSConnectionString");

                // optionsBuilder.UseSqlServer("Server=.;Database=DynamicCMS;Trusted_Connection=True;");

                // string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                // IConfigurationBuilder builder = new ConfigurationBuilder();

                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("data.json")
                    .Build();

                // TODO @task Decoding of encrypted connection string
                var connectionString = configuration.GetConnectionString("AppConnectionString");

                optionsBuilder.UseSqlServer(connectionString);
            }

            // For development only:
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /// Maps entities to tables using specified names
            // modelBuilder.Entity<Il>().ToTable("Iller");
            // modelBuilder.Entity<Ilce>().ToTable("Ilceler");
            // 
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            if (Database.ProviderName == "Microsoft.EntityFrameworkCore.SqlServer")
            {
                foreach (var entityType in modelBuilder.Model.GetEntityTypes())
                {
                    var properties = entityType.ClrType.GetProperties().Where(p => p.Name == "RowTimeStamp" && p.PropertyType == typeof(DateTime));

                    foreach (var property in properties)
                    {
                        modelBuilder.Entity(entityType.Name).Property(property.Name).HasDefaultValueSql("getdate()");
                    }
                }
            }
        }

        /// Entities
        public DbSet<IL> Iller { get; set; }
        public DbSet<ILCE> Ilceler { get; set; }
        public DbSet<SBB> SemtBucakBeldeler { get; set; }
        public DbSet<MAHALLE> Mahalleler { get; set; }
    }
}

/// *****
/// NOTES
/// *****
/// 
/// EF-Core Code First Create Database:
/// -----------------------------------
/// After creating entity classes and dbcontext run the following commands:
/// PM> add-migration Mig0 -OutputDir AppData/Migrations        (add migration with name Mig0 to output directory specified)
/// PM> update-database [name] –verbose                         (apply migration to database, if name is omitted, all migrations are updated)
/// PM> remove-migration                                        (remove last migration if it is not applied to the database)
/// PM> remove-migration [-Migration] name                      (remove the named migration if it is not applied to the database)
/// PM> update-database CreateDB                                (target migration is the point to which you want to restore the database)
/// PM> Update-Database [-Migration] 0                          (unapply all migrations and revert database back the blank state)
/// PM> Update-Database [-Migration] name                       (unapply all migrations till the given name and revert database back the previous state)
/// 
/// EF-Core DB First Create Entities:
/// ---------------------------------
/// Scaffold-DbContext "Server=.;Database=App102;User Id=sa; Password=aA123456;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir AppData/Scaffold
/// Scaffold-DbContext "Host=127.0.0.1;Database=postgres;Username=postgres;Password=aA123456" Npgsql.EntityFrameworkCore.PostgreSQL -OutputDir AppData/Scaffold
