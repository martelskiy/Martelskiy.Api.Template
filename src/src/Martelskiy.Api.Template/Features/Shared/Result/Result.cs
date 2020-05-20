using System.Collections.Generic;
using System.Linq;

namespace Martelskiy.Api.Template.Features.Shared.Result
{
    public class Result : IResult
    {
        protected Result(bool success, string message = null)
            : this(success, message, Enumerable.Empty<Error>())
        {
        }

        protected Result(bool success, string message, IEnumerable<Error> errors)
        {
            this.Success = success;
            this.Message = message;
            this.Errors = errors ?? Enumerable.Empty<Error>();
        }

        public bool Success { get; }

        public string Message { get; }

        public IEnumerable<Error> Errors { get; }

        public Error FirstError
        {
            get
            {
                IEnumerable<Error> errors = this.Errors;
                if (errors == null)
                    return (Error)null;
                return errors.FirstOrDefault<Error>();
            }
        }

        public static Result Ok()
        {
            return new Result(true);
        }

        public static Result Ok(string message)
        {
            return new Result(true, message);
        }

        public static Result Fail()
        {
            return new Result(false);
        }

        public static Result Fail(string message)
        {
            return new Result(false, message);
        }

        public static Result Fail(string message, IEnumerable<Error> errors)
        {
            return new Result(false, message, errors);
        }
    }

    public class Result<T> : Result
    {
        protected Result(bool success, T data) : this(success, data, null) { }

        protected Result(bool success, T data, string message) : this(success, data, message, Enumerable.Empty<Error>()) { }

        protected Result(bool success, T data, string message, IEnumerable<Error> errors) : base(success, message, errors)
        {
            Data = data;
        }

        public T Data { get; }

        public static Result<T> Ok(T data)
        {
            return new Result<T>(true, data);
        }

        public static Result<T> Ok(T data, string message)
        {
            return new Result<T>(true, data, message);
        }

        public new static Result<T> Fail()
        {
            return new Result<T>(false, default(T));
        }

        public new static Result<T> Fail(string message)
        {
            return new Result<T>(false, default(T), message);
        }

        public new static Result<T> Fail(string message, IEnumerable<Error> errors)
        {
            return new Result<T>(false, default(T), message, errors);
        }
    }
}
