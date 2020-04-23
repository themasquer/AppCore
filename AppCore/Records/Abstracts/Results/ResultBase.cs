namespace AppCore.Records.Abstracts.Results
{
    public abstract class ResultBase
    {
        public bool Success { get; }
        public string Message { get; }

        protected ResultBase(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }

    public abstract class ResultBase<TResultType> : ResultBase, IResultType<TResultType>
    {
        public TResultType ResultType { get; }

        protected ResultBase(bool success, string message, TResultType resultType) : base(success, message)
        {
            ResultType = resultType;
        }
    }
}
