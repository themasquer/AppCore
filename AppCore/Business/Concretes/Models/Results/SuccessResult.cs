namespace AppCore.Business.Concretes.Models.Results
{
    public class SuccessResult : Result
    {
        public SuccessResult(string message) : base(true, message, false)
        {
            
        }

        public SuccessResult() : base(true, "", false)
        {
            
        }
    }

    public class SuccessResult<TResultType> : Result<TResultType>
    {
        public SuccessResult(string message, TResultType data) : base(true, message, data, false)
        {

        }

        public SuccessResult(string message) : base(true, message, default, false)
        {

        }

        public SuccessResult(TResultType data) : base(true, "", data, false)
        {

        }

        public SuccessResult() : base(true, "", default, false)
        {

        }
    }
}
