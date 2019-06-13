using Martelskiy.Api.Template.Features.Shared.CorrelationId;
using Serilog.Core;
using Serilog.Events;

namespace Martelskiy.Api.Template.Features.Shared.Logging
{
    public class CorrelationIdEnricher : ILogEventEnricher
    {
        private readonly ICorrelationIdAccessor _correlationIdAccessor;
        private const string CorrelationId = "CorrelationId";

        public CorrelationIdEnricher(ICorrelationIdAccessor correlationIdAccessor)
        {
            _correlationIdAccessor = correlationIdAccessor;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var correlationId = _correlationIdAccessor.GetCorrelationId();
            if (!string.IsNullOrWhiteSpace(correlationId))
            {
                logEvent.AddOrUpdateProperty(new LogEventProperty(CorrelationId, new ScalarValue(correlationId)));
            }
        }
    }
}
