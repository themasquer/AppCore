using AppCore.Records.Abstracts;

namespace AppCore.Entities.Abstracts.Identity
{
    public abstract class IdentityUserRoleBase : RecordBase
    {
        public int IdentityUserId { get; set; }
        public int IdentityRoleId { get; set; }
    }
}
