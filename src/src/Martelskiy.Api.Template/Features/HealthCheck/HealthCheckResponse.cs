namespace Martelskiy.Api.Template.Features.HealthCheck
{
    public class HealthCheckResponse
    {
        public HealthCheckResponse(int statusCode)
        {
            StatusCode = statusCode;
        }

        public int StatusCode { get; }
    }
}
