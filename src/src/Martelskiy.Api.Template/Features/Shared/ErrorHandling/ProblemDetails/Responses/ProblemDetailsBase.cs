namespace Martelskiy.Api.Template.Features.Shared.ErrorHandling.ProblemDetails.Responses
{
    public class ProblemDetailsBase : Microsoft.AspNetCore.Mvc.ProblemDetails
    {
        public ProblemDetailsBase(string correlationId)
        {
            CorrelationId = correlationId;
        }
        public string CorrelationId { get; }
    }
}
