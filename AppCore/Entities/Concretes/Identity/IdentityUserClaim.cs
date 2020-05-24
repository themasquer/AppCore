using AppCore.Entities.Abstracts.Identity;

namespace AppCore.Entities.Concretes.Identity
{
    public class IdentityUserClaim : IdentityUserClaimBase
    {
        public IdentityUser IdentityUser { get; set; }
        public IdentityClaim IdentityClaim { get; set; }
    }
}
