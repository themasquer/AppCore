using AppCore.Entities.Concretes.Identity;
using Microsoft.EntityFrameworkCore;

namespace AppCore.DataAccess.Abstracts.EntityFramework.Identity
{
    public abstract class IdentityUserDalBase : RepositoryBase<IdentityUser>
    {
        protected IdentityUserDalBase(DbContext context) : base(context)
        {

        }
    }
}
