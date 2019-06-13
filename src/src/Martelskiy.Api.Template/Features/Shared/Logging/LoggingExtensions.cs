using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace Martelskiy.Api.Template.Features.Shared.Logging
{
    public static class LoggingExtensions
    {
        public static IWebHostBuilder UseLogging(this IWebHostBuilder webHost)
        {
            webHost.UseSerilog();
            return webHost;
        }
    }
}
