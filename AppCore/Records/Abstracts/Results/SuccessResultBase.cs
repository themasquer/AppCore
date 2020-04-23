namespace AppCore.Records.Abstracts.Results
{
    public abstract class SuccessResultBase : ResultBase
    {
        protected SuccessResultBase(string message) : base(true, message)
        {
            
        }

        protected SuccessResultBase() : base(true, "Success")
        {
            
        }
    }

    public abstract class SuccessResultBase<TResultType> : ResultBase<TResultType>
    {
        protected SuccessResultBase(string message, TResultType resultType) : base(true, message, resultType)
        {

        }

        protected SuccessResultBase(TResultType resultType) : base(true, "Success", resultType)
        {

        }
    }
}
