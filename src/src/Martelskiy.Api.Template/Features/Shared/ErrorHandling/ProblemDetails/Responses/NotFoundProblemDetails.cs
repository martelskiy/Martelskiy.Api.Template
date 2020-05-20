namespace Martelskiy.Api.Template.Features.Shared.ErrorHandling.ProblemDetails.Responses
{
    public class NotFoundProblemDetails : ProblemDetailsBase
    {
        private const string TitleConst = "Not Found";
        private const string DetailConst = "The requested resource could not be found";

        public NotFoundProblemDetails(string correlationId) : base(correlationId)
        {
            Status = 404;
            Type = ProblemDetailsConstants.NotFoundType;
            Title = TitleConst;
            Detail = DetailConst;
        }
    }
}
