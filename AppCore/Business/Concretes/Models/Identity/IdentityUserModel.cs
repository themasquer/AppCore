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

        public List<IdentityRoleModel> IdentityRoles { get; set; }
        public List<IdentityClaimModel> IdentityClaims { get; set; }
    }
}
