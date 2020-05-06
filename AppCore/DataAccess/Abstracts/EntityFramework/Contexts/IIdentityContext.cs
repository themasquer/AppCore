using AppCore.Entities.Concretes.Identity;
using Microsoft.EntityFrameworkCore;

namespace AppCore.DataAccess.Abstracts.EntityFramework.Contexts
{
    public interface IIdentityContext
    {
        DbSet<IdentityUser> IdentityUsers { get; set; }
        DbSet<IdentityRole> IdentityRoles { get; set; }
        DbSet<IdentityClaim> IdentityClaims { get; set; }
        DbSet<IdentityUserRole> IdentityUserRoles { get; set; }
        DbSet<IdentityUserClaim> IdentityUserClaims { get; set; }
    }
}
