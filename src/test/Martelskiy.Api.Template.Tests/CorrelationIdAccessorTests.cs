using System;
using Martelskiy.Api.Template.Features.Shared.CorrelationId;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Shouldly;
using Xunit;

namespace Martelskiy.Api.Template.Tests
{
    public class CorrelationIdAccessorTests
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CorrelationIdAccessor _sut;

        public CorrelationIdAccessorTests()
        {
            _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
            _sut = new CorrelationIdAccessor(_httpContextAccessor);
        }

        [Fact]
        public void GivenNullHttpContext_WhenGetCorrelationId_ThenReturnsNull()
        {
            _httpContextAccessor.HttpContext.ReturnsNull();

            var result = _sut.GetCorrelationId();

            result.ShouldBeNull();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void GivenInvalidTraceIdentifier_WhenGetCorrelationId_ThenReturnsNull(string traceIdentifier)
        {
            _httpContextAccessor.HttpContext.TraceIdentifier.Returns(traceIdentifier);

            var result = _sut.GetCorrelationId();

            result.ShouldBeNull();
        }

        [Fact]
        public void GivenOnlyTraceIdentifier_WhenGetCorrelationId_ThenReturnsTraceIdentifier()
        {
            var correlationId = Guid.NewGuid().ToString();
            _httpContextAccessor.HttpContext.TraceIdentifier.Returns(correlationId);

            var result = _sut.GetCorrelationId();

            result.ShouldBe(correlationId);
        }

        [Fact]
        public void GivenXCorrelationIdHeader_WhenGetCorrelationId_ThenReturnsXCorrelationIdHeaderValueAsCorrelationId()
        {
            var correlationId = "406B9CA7-A8DA-4EC1-8432-85EE3C794B02";
            _httpContextAccessor.HttpContext.Request.Headers.Returns(new HeaderDictionary
            {
                {"X-Correlation-ID", correlationId}
            });

            var result = _sut.GetCorrelationId();

            result.ShouldBe(correlationId);
        }
    }
}
