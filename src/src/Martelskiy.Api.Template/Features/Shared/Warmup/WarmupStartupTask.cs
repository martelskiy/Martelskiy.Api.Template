using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Martelskiy.Api.Template.Features.Shared.StartupTask;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Martelskiy.Api.Template.Features.Shared.Warmup
{
    public class WarmupStartupTask : IStartupTask
    {
        private readonly IServiceCollection _services;
        private readonly IServiceProvider _provider;
        private readonly ILogger<WarmupStartupTask> _logger;

        public WarmupStartupTask(IServiceCollection services, IServiceProvider provider, ILogger<WarmupStartupTask> logger)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Warmup by building all singletons ....");

            foreach (var singleton in GetSingletons(_services))
            {
                _provider.GetServices(singleton);
            }

            return Task.CompletedTask;
        }

        static IEnumerable<Type> GetSingletons(IServiceCollection services)
        {
            return services
                .Where(descriptor => descriptor.Lifetime == ServiceLifetime.Singleton)
                .Where(descriptor => descriptor.ImplementationType != typeof(WarmupStartupTask))
                .Where(descriptor => descriptor.ServiceType.ContainsGenericParameters == false)
                .Select(descriptor => descriptor.ServiceType)
                .Distinct();
        }
    }
}