using System;

namespace AppCore.Records.Abstracts
{
    public interface IRecordUpdatedBy
    {
        string UpdatedBy { get; set; }
        DateTime? UpdateDate { get; set; }
    }
}
