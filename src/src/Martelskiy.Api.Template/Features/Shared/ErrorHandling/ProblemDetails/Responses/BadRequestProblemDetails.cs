using System.Collections.Generic;

namespace Martelskiy.Api.Template.Features.Shared.ErrorHandling.ProblemDetails.Responses
{
    public class BadRequestProblemDetails : ProblemDetailsBase
    {
        public BadRequestProblemDetails(string correlationId, IEnumerable<ProblemDetailsError> errors) : base(correlationId)
        {
            Type = ProblemDetailsConstants.BadRequestType;
            Title = "Bad Request";
            Detail = "The request produced one or more errors";
            Status = 400;
            Errors = errors;
        }

        public IEnumerable<ProblemDetailsError> Errors { get; }
    }
}
