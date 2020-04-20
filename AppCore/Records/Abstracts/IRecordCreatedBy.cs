using System;

namespace AppCore.Records.Abstracts
{
    public interface IRecordCreatedBy
    {
        string CreatedBy { get; set; }
        DateTime? CreateDate { get; set; }
    }
}
