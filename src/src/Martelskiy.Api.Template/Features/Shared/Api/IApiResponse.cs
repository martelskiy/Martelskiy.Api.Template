namespace Martelskiy.Api.Template.Features.Shared.Api
{
    public interface IApiResponse
    {
        int StatusCode { get; }
    }

    public interface IApiResponse<out T> : IApiResponse
    {
        T Data { get; }
    }
}
