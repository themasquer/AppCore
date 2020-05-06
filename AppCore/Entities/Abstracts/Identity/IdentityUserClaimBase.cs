using AppCore.Records.Abstracts;

namespace AppCore.Entities.Abstracts.Identity
{
    public abstract class IdentityUserClaimBase : RecordBase
    {
        public int IdentityUserId { get; set; }
        public int IdentityClaimId { get; set; }
    }
}
