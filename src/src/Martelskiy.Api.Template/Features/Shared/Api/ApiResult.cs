using Microsoft.AspNetCore.Mvc;

namespace Martelskiy.Api.Template.Features.Shared.Api
{
    public class ApiResult : ObjectResult
    {
        public ApiResult(IApiResponse value) : base(value)
        {
        }
    }
}
