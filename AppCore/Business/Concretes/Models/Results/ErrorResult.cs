using System;

namespace AppCore.Business.Concretes.Models.Results
{
    public class ErrorResult : Result
    {
        public ErrorResult(string message) : base(false, message)
        {

        }

        public ErrorResult() : base(false, "")
        {

        }

        public ErrorResult(Exception exception, bool showException = false)
            : base(false,
                showException == true ?
                    (exception != null ? "Exception: " + exception.Message +
                        (exception.InnerException != null ? " | Inner Exception: " + exception.InnerException.InnerException.Message 
                        : "")
                    : "")
                : "Exception")
        {
            
        }
    }

    public class ErrorResult<TResultType> : Result<TResultType>
    {
        public ErrorResult(string message, TResultType data) : base(false, message, data)
        {
            
        }

        public ErrorResult(string message) : base(false, message, default)
        {
            
        }

        public ErrorResult(TResultType data) : base(false, "", data)
        {
            
        }

        public ErrorResult() : base(false, "", default)
        {
            
        }

        public ErrorResult(Exception exception, bool showException = false)
            : base(false,
                showException == true ?
                    (exception != null ? "Exception: " + exception.Message +
                        (exception.InnerException != null ? " | Inner Exception: " + exception.InnerException.InnerException.Message
                            : "")
                        : "")
                    : "Exception",
                default)
        {
            
        }
    }
}
