using System.Collections.Generic;
using System.IO;
using Martelskiy.Api.Template.Features.Shared;
using Martelskiy.Api.Template.Features.Shared.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Martelskiy.Api.Template
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var webHostBuilder = CreateWebHostBuilder(args);
            var webHost = webHostBuilder.Build();

            var configurators = webHost.Services.GetRequiredService<IEnumerable<IConfigurator>>();

            foreach (var configurator in configurators)
            {
                configurator.Configure();
            }

            var hostingEnvironment = webHost.Services.GetRequiredService<IHostingEnvironment>();

            Log.Logger.Information($"Starting application. Environment: {hostingEnvironment.EnvironmentName}");

            webHost.Run();
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;
                    config
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                        .AddEnvironmentVariables();
                })
                .UseLogging()
                .UseStartup<Startup>();
        }
    }
}
