using AppCore.Entities.Concretes.Identity;
using Microsoft.EntityFrameworkCore;

namespace AppCore.DataAccess.Abstracts.EntityFramework.Identity
{
    public abstract class IdentityRoleDalBase : RepositoryBase<IdentityRole>
    {
        protected IdentityRoleDalBase(DbContext context) : base(context)
        {
            
        }
    }
}
