using Microsoft.AspNetCore.Http;

namespace Martelskiy.Api.Template.Features.Shared.ErrorHandling.ProblemDetails
{
    public class NotFoundProblemDetails : Microsoft.AspNetCore.Mvc.ProblemDetails
    {
        private const string TitleConst = "Not Found";
        private const string DetailConst = "The requested resource could not be found";

        public NotFoundProblemDetails()
        {
            Status = StatusCodes.Status404NotFound;
            Title = TitleConst;
            Detail = DetailConst;
        }
    }
}
