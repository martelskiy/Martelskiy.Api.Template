using Microsoft.AspNetCore.Http;

namespace Martelskiy.Api.Template.Features.Shared.ErrorHandling.ProblemDetails
{
    public class InternalServerErrorProblemDetails : Microsoft.AspNetCore.Mvc.ProblemDetails
    {
        private const string TitleConst = "Internal Server Error";
        private const string DetailConst = "An unexpected error occured on the server and has been logged";

        public InternalServerErrorProblemDetails()
        {
            Status = StatusCodes.Status500InternalServerError;
            Title = TitleConst;
            Detail = DetailConst;
        }
    }
}
