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
        public SuccessResult(string message, TResultType data) : base(true, message, data)
        {

        }

        public SuccessResult(TResultType data) : base(true, "Success", data)
        {

        }
    }
}
