using System;
using Microsoft.Extensions.Hosting;

namespace Martelskiy.Api.Template.Features.Shared.Environment
{
    public static class HostingEnvironmentExtensions
    {
        public static string ShortName(this IHostingEnvironment hostingEnvironment)
        {
            if (string.IsNullOrWhiteSpace(hostingEnvironment?.EnvironmentName))
            {
                throw new ArgumentException(nameof(hostingEnvironment));
            }

            switch (hostingEnvironment.EnvironmentName)
            {
                case EnvironmentName.Development:
                    return Environment.Development.ShortName;
                case EnvironmentName.Production:
                    return Environment.Production.ShortName;
                default:
                    throw new ArgumentOutOfRangeException(nameof(hostingEnvironment.EnvironmentName), $"No ShortName could be mapped for environment '{hostingEnvironment.EnvironmentName}'");
            }
        }
    }
}
