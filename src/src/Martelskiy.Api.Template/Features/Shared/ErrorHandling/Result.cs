using System.Collections.Generic;
using System.Linq;

namespace Martelskiy.Api.Template.Features.Shared.ErrorHandling
{
    public class Result : IResult
    {
        private static readonly Result OkResult = new Result(true);
        private static readonly Result FailResult = new Result(false);
        protected Result(bool success, string message = null) : this(success, message, Enumerable.Empty<Error>()) { }

        protected Result(bool success, string message, IEnumerable<Error> errors)
        {
            Success = success;
            Message = message;
            Errors = errors ?? Enumerable.Empty<Error>();
        }

        public bool Success { get; }
        public string Message { get; }
        public IEnumerable<Error> Errors { get; }
        public Error FirstError => Errors?.FirstOrDefault();

        public static IResult Ok() => OkResult;
        public static IResult Ok(string message) => new Result(true, message);
        public static IResult Fail() => FailResult;
        public static IResult Fail(string message) => new Result(false, message);
        public static IResult Fail(string message, IEnumerable<Error> errors) => new Result(false, message, errors);
    }

    public class Result<T> : Result, IResult<T>
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

    public interface IResult
    {
        bool Success { get; }
        string Message { get; }
        IEnumerable<Error> Errors { get; }
        Error FirstError { get; }
    }

    public interface IResult<out T> : IResult
    {
        T Data { get; }
    }

    public class Error
    {
        public Error(string message, ErrorType type) : this(message, type, null)
        {
            
        }

        public Error(string message, ErrorType type, string errorCode)
        {
            Message = message;
            Type = type;
            ErrorCode = errorCode;
        }

        public string Message { get; }
        public ErrorType Type { get; }
        public string ErrorCode { get; }

        public override string ToString()
        {
            return $"{Type}";
        }
    }

    public enum ErrorType
    {
        Unspecified = 0,
        ValidationError = 1,
        NotFound = 2
    }
}
