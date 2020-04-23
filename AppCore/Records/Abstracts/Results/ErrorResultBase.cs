using System;

namespace AppCore.Records.Abstracts.Results
{
    public abstract class ErrorResultBase : ResultBase
    {
        protected ErrorResultBase(string message) : base(false, message)
        {

        }

        protected ErrorResultBase() : base(false, "Error")
        {

        }

        protected ErrorResultBase(Exception exception)
            : base(false,
                exception != null ? "Exception: " + exception.Message + (exception.InnerException != null ? " | Inner Exception: " + exception.InnerException.InnerException.Message : "") : "Error")
        {

        }
    }

    public abstract class ErrorResultBase<TResultType> : ResultBase<TResultType>
    {
        protected ErrorResultBase(string message, TResultType resultType) : base(false, message, resultType)
        {

        }

        protected ErrorResultBase(TResultType resultType) : base(false, "Error", resultType)
        {

        }

        protected ErrorResultBase(Exception exception)
            : base(false,
                exception != null ? "Exception: " + exception.Message + (exception.InnerException != null ? " | Inner Exception: " + exception.InnerException.InnerException.Message : "") : "Error",
                default)
        {

        }
    }
}
