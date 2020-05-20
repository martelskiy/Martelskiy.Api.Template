namespace Martelskiy.Api.Template.Features.Shared.Result
{
    public class Error
    {
        public Error(string message, ErrorType type)
            : this(message, type, (string)null)
        {
        }

        public Error(string message, ErrorType type, string errorCode)
        {
            this.Message = message;
            this.Type = type;
            this.ErrorCode = errorCode;
        }

        public string Message { get; }

        public ErrorType Type { get; }

        public string ErrorCode { get; }

        public override string ToString()
        {
            return string.Format("{0}", (object)this.Type);
        }
    }
}
