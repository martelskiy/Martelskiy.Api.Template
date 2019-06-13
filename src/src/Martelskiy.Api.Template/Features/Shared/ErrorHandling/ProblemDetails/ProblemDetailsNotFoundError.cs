namespace Martelskiy.Api.Template.Features.Shared.ErrorHandling.ProblemDetails
{
    public class ProblemDetailsNotFoundError : ProblemDetailsError
    {
        public ProblemDetailsNotFoundError(string detail) : base(ProblemDetailsErrorType.NotFound, detail)
        {
        }
    }
}
