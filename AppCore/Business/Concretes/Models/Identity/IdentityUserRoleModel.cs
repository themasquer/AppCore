using AppCore.Entities.Abstracts.Identity;

namespace AppCore.Business.Concretes.Models.Identity
{
    public class IdentityUserRoleModel : IdentityUserRoleBase
    {
        public IdentityUserModel IdentityUserModel { get; set; }
        public IdentityRoleModel IdentityRoleModel { get; set; }
    }
}
