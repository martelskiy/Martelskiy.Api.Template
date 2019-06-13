using System;
using System.Threading.Tasks;
using Martelskiy.Api.Template.Features.Shared;
using Martelskiy.Api.Template.Features.Shared.Serializers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Martelskiy.Api.Template.Features.HealthCheck
{
    public class HealthCheckResponseWriter
    {
        private readonly IJsonSerializer _jsonSerializer;

        public HealthCheckResponseWriter(IJsonSerializer jsonSerializer)
        {
            _jsonSerializer = jsonSerializer ?? throw new ArgumentNullException(nameof(jsonSerializer));
        }

        public Task WriteResponse(HttpContext httpContext, HealthReport result)
        {
            httpContext.Response.ContentType = ContentType.Json;
            var responseData = new HealthCheckResponse(result.Status == HealthStatus.Healthy ? 200 : 500);
            var responseJson = _jsonSerializer.Serialize(responseData);
            return httpContext.Response.WriteAsync(responseJson);
        }
    }
}
