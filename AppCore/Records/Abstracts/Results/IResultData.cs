namespace AppCore.Records.Abstracts.Results
{
    public interface IResultData<out TResultType>
    {
        TResultType Data { get; }
    }
}
