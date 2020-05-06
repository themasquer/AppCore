using System.ComponentModel.DataAnnotations;
using AppCore.Records.Abstracts;

namespace AppCore.Entities.Abstracts.Identity
{
    public abstract class IdentityClaimBase : RecordBase
    {
        [Required]
        public string Type { get; set; }

        [Required]
        public string Value { get; set; }
    }
}
