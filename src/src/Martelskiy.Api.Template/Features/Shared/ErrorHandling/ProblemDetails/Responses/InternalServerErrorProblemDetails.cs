namespace Martelskiy.Api.Template.Features.Shared.ErrorHandling.ProblemDetails.Responses
{
    public class InternalServerErrorProblemDetails : ProblemDetailsBase
    {
        private const string TitleConst = "Internal Server Error";
        private const string DetailConst = "An unexpected error occured on the server and has been logged";

        public InternalServerErrorProblemDetails(string correlationId) : base(correlationId)
        {
            Status = 500;
            Type = ProblemDetailsConstants.InternalServerErrorType;
            Title = TitleConst;
            Detail = DetailConst;
        }
    }
}
