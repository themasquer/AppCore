namespace AppCore.Records.Abstracts.Results
{
    public interface IResultType<out TResultType>
    {
        TResultType ResultType { get; }
    }
}
