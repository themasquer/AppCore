using System;
using AppCore.Business.Configs;

namespace AppCore.Business.Concretes.Models.Results
{
    public class ExceptionResult : Result
    {
        public ExceptionResult(Exception exception, bool showException = false)
            : base(ResultStatus.Exception,
                showException == true ?
                    (exception != null ? 
                        "Exception: " + exception.Message + (exception.InnerException != null ? 
                            " | Inner Exception: " + exception.InnerException.InnerException.Message
                        : "")
                    : "")
                : "Exception")
        {

        }

        public ExceptionResult() : base(ResultStatus.Exception,"Exception")
        {

        }
    }

    public class ExceptionResult<TResultType> : Result<TResultType>
    {
        public ExceptionResult(Exception exception, bool showException = false)
            : base(ResultStatus.Exception,
                showException == true ?
                    (exception != null ? 
                        "Exception: " + exception.Message + (exception.InnerException != null ? 
                            " | Inner Exception: " + exception.InnerException.InnerException.Message
                        : "")
                    : "")
                : "Exception",
            default)
        {

        }

        public ExceptionResult() : base(ResultStatus.Exception, "Exception", default)
        {

        }
    }
}
