using AppCore.Entities.Concretes.Identity;
using Microsoft.EntityFrameworkCore;

namespace AppCore.DataAccess.Abstracts.EntityFramework.Identity
{
    public abstract class IdentityUserClaimDalBase : RepositoryBase<IdentityUserClaim>
    {
        protected IdentityUserClaimDalBase(DbContext context) : base(context)
        {
            
        }
    }
}
