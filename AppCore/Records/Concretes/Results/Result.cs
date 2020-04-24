using AppCore.Records.Abstracts.Results;

namespace AppCore.Records.Concretes.Results
{
    public class Result
    {
        public bool Success { get; }
        public string Message { get; }

        protected Result(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }

    public class Result<TResultType> : Result, IResultData<TResultType>
    {
        public TResultType Data { get; }

        protected Result(bool success, string message, TResultType data) : base(success, message)
        {
            Data = data;
        }
    }
}
