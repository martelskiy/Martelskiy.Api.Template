using System;
using System.Threading.Tasks;
using Martelskiy.Api.Template.Features.Shared.StartupTask;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Martelskiy.Api.Template
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var hostBuilder  = CreateHostBuilder(args);

            var host = hostBuilder.Build();
            
            LogApplicationStartup(host.Services);
            
            await host.RunWithTasksAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var configuration = new ConfigurationBuilder().AddCommandLine(args).Build();
            var environment = configuration.GetValue<string>("environment");

            if (string.IsNullOrWhiteSpace(environment))
            {
                environment = Environments.Development;
            }

            var hostBuilder = new HostBuilder()
                .UseEnvironment(environment)
                .ConfigureWebHost(x =>
                {
                    x.UseKestrel(options =>
                    {
                        options.AddServerHeader = false;
                        options.Limits.MaxRequestBodySize = 2000000;
                    });
                    x.UseStartup<Startup>();
                    x.SuppressStatusMessages(true);
                })
                .ConfigureAppConfiguration((context, configurationBuilder) =>
                {
                    configurationBuilder.AddConfiguration(configuration);
                    configurationBuilder.AddJsonFile("appsettings.json", optional: false);
                    configurationBuilder.AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", true);
                    configurationBuilder.AddJsonFile("appsettings.Local.json", optional: true);
                    configurationBuilder.AddEnvironmentVariables();
                    configurationBuilder.AddCommandLine(args);
                })
                .ConfigureLogging((context, loggingBuilder) =>
                {
                    loggingBuilder.AddSerilog(dispose: true);
                })
                .UseDefaultServiceProvider((context, options) =>
                {
                    options.ValidateOnBuild = true;
                    options.ValidateScopes = true;
                });

            return hostBuilder;
        }
            

        private static void LogApplicationStartup(IServiceProvider services)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();

            logger.LogInformation("Application has started");
        }
    }
}
