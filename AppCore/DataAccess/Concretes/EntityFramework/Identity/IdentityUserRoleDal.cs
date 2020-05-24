using AppCore.DataAccess.Abstracts.EntityFramework.Identity;
using Microsoft.EntityFrameworkCore;

namespace AppCore.DataAccess.Concretes.EntityFramework.Identity
{
    public class IdentityUserRoleDal : IdentityUserRoleDalBase
    {
        public IdentityUserRoleDal(DbContext context) : base(context)
        {

        }
    }
}
