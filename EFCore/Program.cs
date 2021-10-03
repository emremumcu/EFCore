using EFCore.AppLib;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace EFCore
{
    /// <summary>
    /// EFCore V1 (01/10/2021)
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            // CreateHostBuilder(args).Build().Run();

            IHost host = CreateHostBuilder(args).Build();

            DataGenerator.Generate(host);

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
