namespace AppCore.Records.Abstracts
{
    public interface IRecordSoftDelete
    {
        bool? IsDeleted { get; set; }
    }
}
