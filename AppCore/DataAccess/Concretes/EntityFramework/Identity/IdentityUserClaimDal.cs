using AppCore.DataAccess.Abstracts.EntityFramework.Identity;
using Microsoft.EntityFrameworkCore;

namespace AppCore.DataAccess.Concretes.EntityFramework.Identity
{
    public class IdentityUserClaimDal : IdentityUserClaimDalBase
    {
        public IdentityUserClaimDal(DbContext context) : base(context)
        {

        }
    }
}
