using Martelskiy.Api.Template.Features.Shared.CorrelationId;
using Martelskiy.Api.Template.Features.Shared.ErrorHandling;
using Martelskiy.Api.Template.Features.Shared.StartupTask;
using Martelskiy.Api.Template.Features.Shared.Warmup;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Serilog;

namespace Martelskiy.Api.Template.Features.Shared
{
    public static class SharedConfigurator
    {
        public static void ConfigureSharedFeatures(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddSingleton(Log.Logger);
            services.AddSingleton<ICorrelationIdAccessor, CorrelationIdAccessor>();
            services.AddSingleton<ErrorHandlingMiddleware>();

            services
                .AddStartupTask<WarmupStartupTask>()
                .TryAddSingleton(services);
        }
    }
}
