namespace AppCore.Records.Concretes.Results
{
    public class SuccessResult : Result
    {
        public SuccessResult(string message) : base(true, message)
        {
            
        }

        public SuccessResult() : base(true, "Success")
        {
            
        }
    }

    public class SuccessResult<TResultType> : Result<TResultType>
    {
        public SuccessResult(string message, TResultType resultType) : base(true, message, resultType)
        {

        }

        public SuccessResult(TResultType resultType) : base(true, "Success", resultType)
        {

        }
    }
}
