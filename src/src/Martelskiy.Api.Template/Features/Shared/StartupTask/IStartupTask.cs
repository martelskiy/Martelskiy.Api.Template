using System.Threading;
using System.Threading.Tasks;

namespace Martelskiy.Api.Template.Features.Shared.StartupTask
{
    public interface IStartupTask
    {
        Task ExecuteAsync(CancellationToken token = default);
    }
}
