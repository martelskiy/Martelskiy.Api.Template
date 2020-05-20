using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Martelskiy.Api.Template.Features.Shared.StartupTask
{
    public static class StartupTaskConfigurator
    {
        public static IServiceCollection AddStartupTask<T>(this IServiceCollection serviceCollection)
            where T : class, IStartupTask
            => serviceCollection.AddTransient<IStartupTask, T>();

        public static async Task RunWithTasksAsync(this IHost host, CancellationToken cancellationToken = default)
        {
            var startupTasks = host.Services.GetServices<IStartupTask>();

            foreach (var startupTask in startupTasks)
            {
                await startupTask.ExecuteAsync(cancellationToken);
            }

            await host.RunAsync(cancellationToken);
        }
    }
}