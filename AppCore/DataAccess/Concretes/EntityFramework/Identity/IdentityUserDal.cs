using AppCore.DataAccess.Abstracts.EntityFramework.Identity;
using Microsoft.EntityFrameworkCore;

namespace AppCore.DataAccess.Concretes.EntityFramework.Identity
{
    public sealed class IdentityUserDal : IdentityUserDalBase
    {
        public IdentityUserDal(DbContext context) : base(context)
        {
            
        }
    }
}
