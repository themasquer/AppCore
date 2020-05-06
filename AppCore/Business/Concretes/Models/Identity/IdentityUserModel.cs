using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AppCore.Entities.Abstracts.Identity;
using AppCore.Entities.Concretes.Identity;

namespace AppCore.Business.Concretes.Models.Identity
{
    public class IdentityUserModel : IdentityUserBase
    {
        [Required]
        public string Password { get; set; }

        public List<IdentityUserRole> IdentityUserRoles { get; set; }
        public List<IdentityUserClaim> IdentityUserClaims { get; set; }
    }
}
