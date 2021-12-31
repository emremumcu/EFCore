``` csharp
// Program.cs
using Microsoft.EntityFrameworkCore;
using Vocabook.AppData;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<AppDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Accessing services:
//IServiceScope scope = app.Services.CreateScope();
//IServiceProvider services = scope.ServiceProvider;
//AppDbContext context = services.GetRequiredService<AppDbContext>();
//if (context.Database.EnsureCreated()) await context.Database.MigrateAsync();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();

  
// Extension:  
    
namespace Vocabook.AppLib.Tools
{
    public class Services
    {
        /// <summary>
        /// Ex: AppDbContext context = Services.GetServiceInstance<AppDbContext>();
        /// </summary>
        public static T GetServiceInstance<T>() where T: class
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<T>();
            ServiceProvider provider = serviceCollection.BuildServiceProvider();
            T service = provider.GetRequiredService<T>();
            return service;
        }
    }
}

// IEntityTypeConfiguration

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vocabook.AppData.Entities;
using Vocabook.AppLib.Tools;

namespace Vocabook.AppData.Configuration
{
    public class WordConfig : IEntityTypeConfiguration<Word>
    {
        public void Configure(EntityTypeBuilder<Word> builder)
        {
            AppDbContext context = Services.GetServiceInstance<AppDbContext>();

            string defaultCurrentDateSql = context.Database.ProviderName switch
            {
                "Microsoft.EntityFrameworkCore.SqlServer" => "getutcdate()",
                "Microsoft.EntityFrameworkCore.Sqlite" => "datetime('now', 'utc')",
                // https://docs.microsoft.com/tr-tr/ef/core/providers/?tabs=dotnet-core-cli
                _ => ""
            };
            

            builder.ToTable("WordList");
            builder.HasKey(k => k.English);            
            builder.Property(p => p.Sample).IsRequired(false);
            builder.Property(p => p.Synonym).IsRequired(false);
            builder.Property(p => p.Created).IsRequired(true).HasDefaultValueSql(defaultCurrentDateSql);
        }
    }
}

// AppDbContext

using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Vocabook.AppData.Entities;

namespace Vocabook.AppData
{
    public class AppDbContext: DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=vocabook.db;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            {   // Default value setting for Created columns
                string defaultCurrentDateSql = this.Database.ProviderName switch
                {
                    "Microsoft.EntityFrameworkCore.SqlServer" => "getutcdate()",
                    "Microsoft.EntityFrameworkCore.Sqlite" => "datetime('now', 'utc')",
                    // https://docs.microsoft.com/tr-tr/ef/core/providers/?tabs=dotnet-core-cli
                    _ => ""
                };

                foreach (var entityType in modelBuilder.Model.GetEntityTypes())
                {
                    var properties = entityType.ClrType.GetProperties().Where(p => p.Name == "Created" && p.PropertyType == typeof(DateTime));

                    foreach (var property in properties)
                    {
                        modelBuilder.Entity(entityType.Name).Property(property.Name).HasDefaultValueSql(defaultCurrentDateSql);
                    }
                }
            }
        }

#pragma warning disable CS8618
        public DbSet<Word> Words { get; set; }

#pragma warning restore CS8618
    }
}


```
    
    
