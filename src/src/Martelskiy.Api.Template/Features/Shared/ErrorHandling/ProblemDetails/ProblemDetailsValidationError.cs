namespace Martelskiy.Api.Template.Features.Shared.ErrorHandling.ProblemDetails
{
    public class ProblemDetailsValidationError : ProblemDetailsError
    {
        public ProblemDetailsValidationError(string detail) : base(ProblemDetailsErrorType.ValidationError, detail)
        {
        }
    }
}
