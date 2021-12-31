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

    /// <summary>
    /// Ex: string? name = Services.GetProviderNamespace<AppDbContext>();
    /// </summary>
    public static string? GetProviderNamespace<T>() where T : DbContext
    {
        IServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection.AddTransient<T>();
        ServiceProvider provider = serviceCollection.BuildServiceProvider();
        T context = provider.GetRequiredService<T>();
        return context.Database.GetType().Namespace;            
    }
}

// IEntityTypeConfiguration

namespace Vocabook.AppData.Configuration
{
    public class WordConfig : IEntityTypeConfiguration<Word>
    {
        public void Configure(EntityTypeBuilder<Word> builder)
        {
            AppDbContext context = Services.GetServiceInstance<AppDbContext>();
            string? name = context.Database.GetType().Namespace;



            builder.ToTable(builder.Metadata.DisplayName());
            builder.HasKey(k => k.English);            
            builder.Property(p => p.Sample).IsRequired(false);
            builder.Property(p => p.Synonym).IsRequired(false);
            builder.Property(p => p.Created).IsRequired(true).HasDefaultValueSql("datetime('now', 'utc')");
        }
    }
}

```
    
    
