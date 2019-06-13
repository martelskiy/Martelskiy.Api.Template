namespace Martelskiy.Api.Template.Features.Shared.Api
{
    public class ApiResponse<T> : IApiResponse<T>
    {
        public ApiResponse(int statusCode, T data)
        {
            StatusCode = statusCode;
            Data = data;
        }

        public int StatusCode { get; }
        public T Data { get; }
    }

    public class ApiResponse : IApiResponse
    {
        public ApiResponse(int statusCode)
        {
            StatusCode = statusCode;
        }

        public int StatusCode { get; }
    }
}
