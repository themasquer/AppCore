using AppCore.Entities.Concretes.Identity;
using Microsoft.EntityFrameworkCore;

namespace AppCore.DataAccess.Abstracts.EntityFramework.Identity
{
    public abstract class IdentityClaimDalBase : RepositoryBase<IdentityClaim>
    {
        protected IdentityClaimDalBase(DbContext context) : base(context)
        {
            
        }
    }
}
