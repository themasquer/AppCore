using AppCore.DataAccess.Abstracts.EntityFramework.Identity;
using Microsoft.EntityFrameworkCore;

namespace AppCore.DataAccess.Concretes.EntityFramework.Identity
{
    public sealed class IdentityClaimDal : IdentityClaimDalBase
    {
        public IdentityClaimDal(DbContext context) : base(context)
        {

        }
    }
}
