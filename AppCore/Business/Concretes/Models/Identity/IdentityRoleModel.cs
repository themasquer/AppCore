using System.Collections.Generic;
using AppCore.Entities.Abstracts.Identity;
using AppCore.Entities.Concretes.Identity;

namespace AppCore.Business.Concretes.Models.Identity
{
    public class IdentityRoleModel : IdentityRoleBase
    {
        public List<IdentityUserModel> IdentityUsers { get; set; }
    }
}
