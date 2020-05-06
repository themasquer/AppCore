namespace AppCore.Business.Abstracts.Models.Results
{
    public interface IResultData<out TResultType>
    {
        TResultType Data { get; }
    }
}
