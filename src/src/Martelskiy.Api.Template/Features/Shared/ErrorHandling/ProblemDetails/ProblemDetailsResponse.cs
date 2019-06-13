using System.Collections.Generic;

namespace Martelskiy.Api.Template.Features.Shared.ErrorHandling.ProblemDetails
{
    public class ProblemDetailsResponse : Microsoft.AspNetCore.Mvc.ProblemDetails
    {
        public ProblemDetailsResponse(IEnumerable<ProblemDetailsError> errors)
        {
            Errors = errors;
        }

        public IEnumerable<ProblemDetailsError> Errors { get; }
    }
}
