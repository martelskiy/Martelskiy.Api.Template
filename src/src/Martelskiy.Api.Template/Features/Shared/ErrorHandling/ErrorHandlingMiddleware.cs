using System;
using System.Net;
using System.Threading.Tasks;
using Martelskiy.Api.Template.Features.Shared.ErrorHandling.ProblemDetails.Responses;
using Martelskiy.Api.Template.Features.Shared.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Martelskiy.Api.Template.Features.Shared.ErrorHandling
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception exception)
            {
                const int statusCode = (int) HttpStatusCode.InternalServerError;
                const string errorMessage = "Unhandled exception occured";

                context.Response.StatusCode = statusCode;
                context.Response.ContentType = ContentType.ProblemDetails;
                _logger.LogError(eventId: ApplicationEventId.UnhandledError, exception: exception, message: errorMessage);

                var problemDetails = new InternalServerErrorProblemDetails(context.TraceIdentifier);

                await context.Response.WriteAsync(JsonConvert.SerializeObject(problemDetails));
            }
        }
    }
}
