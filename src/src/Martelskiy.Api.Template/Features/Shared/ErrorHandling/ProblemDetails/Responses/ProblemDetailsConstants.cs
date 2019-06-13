namespace Martelskiy.Api.Template.Features.Shared.ErrorHandling.ProblemDetails.Responses
{
    public static class ProblemDetailsConstants
    {
        private const string TypeUrlPattern = "https://httpstatuses/";

        public const string BadRequestType = TypeUrlPattern + "400";
        public const string InternalServerErrorType = TypeUrlPattern + "500";
        public const string NotFoundType = TypeUrlPattern + "404";
    }
}
