using System.Collections.Generic;
using AppCore.Entities.Abstracts.Identity;

namespace AppCore.Entities.Concretes.Identity
{
    public sealed class IdentityClaim : IdentityClaimBase
    {
        public List<IdentityUserClaim> IdentityUserClaims { get; set; }
    }
}
