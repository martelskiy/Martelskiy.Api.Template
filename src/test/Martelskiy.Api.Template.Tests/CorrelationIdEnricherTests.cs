using System;
using System.Collections.Generic;
using Martelskiy.Api.Template.Features.Shared.CorrelationId;
using Martelskiy.Api.Template.Features.Shared.Logging;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Serilog.Core;
using Serilog.Events;
using Shouldly;
using Xunit;

namespace Martelskiy.Api.Template.Tests
{
    public class CorrelationIdEnricherTests
    {
        private readonly ICorrelationIdAccessor _correlationIdAccessor;
        private readonly CorrelationIdEnricher _sut;

        public CorrelationIdEnricherTests()
        {
            _correlationIdAccessor = Substitute.For<ICorrelationIdAccessor>();
            _sut = new CorrelationIdEnricher(_correlationIdAccessor);
        }

        [Fact]
        public void GivenNullCorrelationId_WhenEnrich_ThenDoesNotAddCorrelationIdToLogEvent()
        {
            _correlationIdAccessor.GetCorrelationId().ReturnsNull();
            var logEvent = new LogEvent(
                DateTimeOffset.UtcNow,
                LogEventLevel.Information,
                null,
                MessageTemplate.Empty,
                new List<LogEventProperty>());

            _sut.Enrich(logEvent, Substitute.For<ILogEventPropertyFactory>());

            logEvent.Properties.ContainsKey("CorrelationId").ShouldBe(false);
        }

        [Fact]
        public void GivenCorrelationId_WhenEnrich_ThenAddsCorrelationIdToLogEvent()
        {
            var correlationId = "28CB86F1-59D0-4566-8A06-412E737BF8A3";
            _correlationIdAccessor.GetCorrelationId().Returns(correlationId);
            var logEvent = new LogEvent(
                DateTimeOffset.UtcNow,
                LogEventLevel.Information,
                null,
                MessageTemplate.Empty,
                new List<LogEventProperty>());

            _sut.Enrich(logEvent, Substitute.For<ILogEventPropertyFactory>());

            logEvent.Properties.TryGetValue("CorrelationId", out var value).ShouldBeTrue();
            value.ToString().ShouldBe($"\"{correlationId}\"");
        }
    }
}
