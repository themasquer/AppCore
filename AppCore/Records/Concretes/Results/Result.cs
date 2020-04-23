using AppCore.Records.Abstracts.Results;

namespace AppCore.Records.Concretes.Results
{
    public class Result
    {
        protected bool Success { get; }
        protected string Message { get; }

        protected Result(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }

    public class Result<TResultType> : Result, IResultType<TResultType>
    {
        public TResultType ResultType { get; }

        protected Result(bool success, string message, TResultType resultType) : base(success, message)
        {
            ResultType = resultType;
        }
    }
}
