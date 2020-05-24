using AppCore.Entities.Abstracts.Identity;

namespace AppCore.Entities.Concretes.Identity
{
    public class IdentityUserRole : IdentityUserRoleBase
    {
        public IdentityUser IdentityUser { get; set; }
        public IdentityRole IdentityRole { get; set; }
    }
}
