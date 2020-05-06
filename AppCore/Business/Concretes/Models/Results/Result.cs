using AppCore.Business.Abstracts.Models.Results;

namespace AppCore.Business.Concretes.Models.Results
{
    public class Result
    {
        public bool Success { get; }
        public string Message { get; set; }
        public bool Exception { get; set; }

        protected Result(bool success, string message, bool exception)
        {
            Success = success;
            Message = message;
            Exception = exception;
        }
    }

    public class Result<TResultType> : Result, IResultData<TResultType>
    {
        public TResultType Data { get; }

        protected Result(bool success, string message, TResultType data, bool exception) : base(success, message, exception)
        {
            Data = data;
        }
    }
}
