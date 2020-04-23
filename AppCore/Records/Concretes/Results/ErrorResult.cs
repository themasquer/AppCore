using System;

namespace AppCore.Records.Concretes.Results
{
    public class ErrorResult : Result
    {
        public ErrorResult(string message) : base(false, message)
        {

        }

        public ErrorResult() : base(false, "Error")
        {

        }

        public ErrorResult(Exception exception)
            : base(false,
                exception != null ? "Exception: " + exception.Message + (exception.InnerException != null ? " | Inner Exception: " + exception.InnerException.InnerException.Message : "") : "Error")
        {

        }
    }

    public class ErrorResult<TResultType> : Result<TResultType>
    {
        public ErrorResult(string message, TResultType resultType) : base(false, message, resultType)
        {

        }

        public ErrorResult(TResultType resultType) : base(false, "Error", resultType)
        {
            
        }

        public ErrorResult(Exception exception)
            : base(false,
                exception != null ? "Exception: " + exception.Message + (exception.InnerException != null ? " | Inner Exception: " + exception.InnerException.InnerException.Message : "") : "Error",
                default)
        {

        }
    }
}
