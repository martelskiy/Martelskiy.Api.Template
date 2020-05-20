using System.Collections.Generic;

namespace Martelskiy.Api.Template.Features.Shared.Result
{
    public interface IResult
    {
        bool Success { get; }

        string Message { get; }

        IEnumerable<Error> Errors { get; }

        Error FirstError { get; }
    }
}
