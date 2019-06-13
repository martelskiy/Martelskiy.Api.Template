using Microsoft.Extensions.Logging;

namespace Martelskiy.Api.Template.Features.Shared.Logging
{
    public static class ApplicationEventId
    {
        public static EventId UnhandledError = new EventId(50000);
    }
}
