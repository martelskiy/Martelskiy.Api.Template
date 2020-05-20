using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Martelskiy.Api.Template.Features.Shared.CorrelationId
{
    public class CorrelationIdAccessor : ICorrelationIdAccessor
    {
        private readonly IReadOnlyCollection<string> _correlationHeaderNames;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CorrelationIdAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _correlationHeaderNames = new List<string> { "X-Request-ID", "X-Correlation-ID" };
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public string GetCorrelationId()
        {
            var request = _httpContextAccessor?.HttpContext?.Request;
            var correlationId = _httpContextAccessor?.HttpContext?.TraceIdentifier;
            if (request?.Headers != null)
            {
                foreach (var correlationHeaderName in _correlationHeaderNames)
                {
                    if (request.Headers.TryGetValue(correlationHeaderName, out var headerValue))
                    {
                        correlationId = headerValue;
                        break;
                    }
                }
            }

            return !string.IsNullOrWhiteSpace(correlationId) ? correlationId : null;
        }
    }
}