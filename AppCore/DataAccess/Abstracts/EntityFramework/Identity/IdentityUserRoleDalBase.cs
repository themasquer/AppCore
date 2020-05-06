using AppCore.Entities.Concretes.Identity;
using Microsoft.EntityFrameworkCore;

namespace AppCore.DataAccess.Abstracts.EntityFramework.Identity
{
    public abstract class IdentityUserRoleDalBase : RepositoryBase<IdentityUserRole>
    {
        protected IdentityUserRoleDalBase(DbContext context) : base(context)
        {
            
        }
    }
}
