using AppCore.Records.Abstracts;

namespace AppCore.Entities.Abstracts.Identity
{
    public abstract class IdentityClaimBase : RecordBase
    {
        public string Type { get; set; }
        public string Value { get; set; }
        public int? RelatedClaimId { get; set; }
    }
}
