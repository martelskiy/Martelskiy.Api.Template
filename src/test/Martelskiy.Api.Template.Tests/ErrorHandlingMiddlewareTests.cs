using System;
using System.Threading.Tasks;
using Martelskiy.Api.Template.Features.Shared.ErrorHandling;
using Martelskiy.Api.Template.Features.Shared.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Martelskiy.Api.Template.Tests
{
    public class ErrorHandlingMiddlewareTests
    {
        private readonly ErrorHandlingMiddleware _sut;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddlewareTests()
        {
            _logger = Substitute.For<ILogger<ErrorHandlingMiddleware>>();
            _sut = new ErrorHandlingMiddleware(_logger);
        }

        [Fact]
        public Task GivenSuccessfulExecution_ThenShouldNotThrowException()
        {
            Should.NotThrow(async () =>
            {
                await _sut.InvokeAsync(new DefaultHttpContext(), context => Task.CompletedTask);
                return Task.CompletedTask;
            });
            return Task.CompletedTask;
        }

        [Fact]
        public async Task GivenNonSuccessfulExecution_ThenReturnsStatusCode500()
        {
            var httpContext = new DefaultHttpContext();

            await _sut.InvokeAsync(httpContext, context => throw new Exception("Any error"));

            httpContext.Response.StatusCode.ShouldBe(500);
        }

        [Fact]
        public async Task GivenNonSuccessfulExecution_ThenLogsException()
        {
            var httpContext = new DefaultHttpContext();
            var exception = new Exception("Any error");

            await _sut.InvokeAsync(httpContext, context => throw exception);

            // Note, we cant use .Received here because the logging framework are doing some magic behind the scenes.
            // So our parameters to LogError here is useless. The test is still useful however since we are assuring that we are logging the error.
            _logger.ReceivedWithAnyArgs(1).LogError(ApplicationEventId.UnhandledError, exception, message: "Unhandled exception occured");
        }

        [Fact]
        public async Task GivenNonSuccessfulExecution_ThenReturnsProblemDetailsContentType()
        {
            var httpContext = new DefaultHttpContext();
            var exception = new Exception("Any error");

            await _sut.InvokeAsync(httpContext, context => throw exception);

            httpContext.Response.ContentType.ShouldBe("application/problem+json");
        }
    }
}
