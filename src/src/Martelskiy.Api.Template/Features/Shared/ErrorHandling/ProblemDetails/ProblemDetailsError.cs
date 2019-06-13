namespace Martelskiy.Api.Template.Features.Shared.ErrorHandling.ProblemDetails
{
    public class ProblemDetailsError
    {
        public ProblemDetailsError(string title, string detail)
        {
            Title = title;
            Detail = detail;
        }
        public string Title { get; }
        public string Detail { get; }
    }
}
