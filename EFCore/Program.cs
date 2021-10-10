namespace EFCore
{
    using EFCore.AppLib;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using System.Threading.Tasks;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();

            await DataGenerator.Generate(host);

            /// IServiceScope scope = host.Services.CreateScope();
            /// IServiceProvider services = scope.ServiceProvider;
            /// IWebHostEnvironment environment = services.GetRequiredService<IWebHostEnvironment>();

            IConfigurationRoot configRoot = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddEnvironmentVariables()
                .AddJsonFile("appsettings.json", optional: false)
                .AddCommandLine(args)
                .Build();

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
