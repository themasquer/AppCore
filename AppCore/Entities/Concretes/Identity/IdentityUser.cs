using System.Collections.Generic;
using AppCore.Entities.Abstracts.Identity;

namespace AppCore.Entities.Concretes.Identity
{
    public sealed class IdentityUser : IdentityUserBase
    {
        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }
        public List<IdentityUserRole> IdentityUserRoles { get; set; }
        public List<IdentityUserClaim> IdentityUserClaims { get; set; }
    }
}
