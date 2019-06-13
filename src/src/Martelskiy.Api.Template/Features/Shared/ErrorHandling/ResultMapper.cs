using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Martelskiy.Api.Template.Features.Shared.CorrelationId;
using Martelskiy.Api.Template.Features.Shared.ErrorHandling.ProblemDetails;
using Martelskiy.Api.Template.Features.Shared.ErrorHandling.ProblemDetails.Responses;
using Martelskiy.Api.Template.Features.Shared.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Martelskiy.Api.Template.Features.Shared.ErrorHandling
{
    public class ResultMapper
    {
        private readonly ICorrelationIdAccessor _correlationIdAccessor;

        public ResultMapper(ICorrelationIdAccessor correlationIdAccessor)
        {
            _correlationIdAccessor = correlationIdAccessor ?? throw new ArgumentNullException(nameof(correlationIdAccessor));
        }

        public IActionResult MapFailureResult(IResult result)
        {
            var mostPrioritizedError = result.Errors.OrderBy(x => ErrorTypePriority.IndexOf(x.Type)).FirstOrDefault();
            var statusCode = MapErrorToHttpStatusCode(mostPrioritizedError?.Type);
            var errors = result.Errors.Select(CreateProblemDetailsError);
            var correlationId = _correlationIdAccessor.GetCorrelationId();

            switch (statusCode)
            {
                case HttpStatusCode.NotFound:
                    return new NotFoundObjectResult(new NotFoundProblemDetails(correlationId));
                case HttpStatusCode.BadRequest:
                    return new BadRequestObjectResult(new BadRequestProblemDetails(correlationId, errors));
                default:
                    var objectResult = new ObjectResult(new ObjectResult(new InternalServerErrorProblemDetails(correlationId)));
                    objectResult.StatusCode = 500;
                    return objectResult;
            }
        }

        private static ProblemDetailsError CreateProblemDetailsError(Error error)
        {
            switch (error.Type)
            {
                case ErrorType.NotFound:
                    return new ProblemDetailsNotFoundError(error.Message);
                case ErrorType.ValidationError:
                    return new ProblemDetailsValidationError(error.Message);
                default:
                    throw new ArgumentOutOfRangeException($"ErrorType '{error.Type}' has not been mapped to a ProblemDetailsError.");
            }
        }

        private static HttpStatusCode MapErrorToHttpStatusCode(ErrorType? errorType)
        {
            switch (errorType)
            {
                case ErrorType.NotFound:
                    return HttpStatusCode.NotFound;
                case ErrorType.ValidationError:
                    return HttpStatusCode.BadRequest;
                default:
                    return HttpStatusCode.InternalServerError;
            }
        }

        /// <summary>
        /// This method prioritizes which errors we deem most important.
        /// We need to prioritize so that we can apply the appropriate status code to the http response.
        /// </summary>
        private static IEnumerable<ErrorType> ErrorTypePriority => new List<ErrorType>
        {
            ErrorType.Unspecified,
            ErrorType.ValidationError,
            ErrorType.NotFound
        };
    }
}
