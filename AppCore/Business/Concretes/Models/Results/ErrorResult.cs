using System;

namespace AppCore.Business.Concretes.Models.Results
{
    public class ErrorResult : Result
    {
        public ErrorResult(string message, bool exception = false) : base(false, message, exception)
        {

        }

        public ErrorResult(bool exception = false) : base(false, "", exception)
        {

        }

        public ErrorResult(Exception exception, bool showException = false)
            : base(false,
                showException == true ?
                    (exception != null ? "Exception: " + exception.Message +
                        (exception.InnerException != null ? " | Inner Exception: " + exception.InnerException.InnerException.Message 
                        : "")
                    : "")
                : "",
                true)
        {
            
        }
    }

    public class ErrorResult<TResultType> : Result<TResultType>
    {
        public ErrorResult(string message, TResultType data, bool exception = false) : base(false, message, data, exception)
        {
            
        }

        public ErrorResult(string message, bool exception = false) : base(false, message, default, exception)
        {
            
        }

        public ErrorResult(TResultType data, bool exception = false) : base(false, "", data, exception)
        {
            
        }

        public ErrorResult(bool exception = false) : base(false, "", default, exception)
        {
            
        }

        public ErrorResult(Exception exception, bool showException = false)
            : base(false,
                showException == true ?
                    (exception != null ? "Exception: " + exception.Message +
                        (exception.InnerException != null ? " | Inner Exception: " + exception.InnerException.InnerException.Message
                            : "")
                        : "")
                    : "",
                default,
                true)
        {
            
        }
    }
}
