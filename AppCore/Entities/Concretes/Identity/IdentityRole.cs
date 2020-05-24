using System.Collections.Generic;
using AppCore.Entities.Abstracts.Identity;

namespace AppCore.Entities.Concretes.Identity
{
    public class IdentityRole : IdentityRoleBase
    {
        public List<IdentityUserRole> IdentityUserRoles { get; set; }
    }
}
