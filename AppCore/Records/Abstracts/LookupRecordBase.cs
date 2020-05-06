using System.ComponentModel.DataAnnotations;

namespace AppCore.Records.Abstracts
{
    public abstract class LookupRecordBase : RecordBase
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
