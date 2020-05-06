using AppCore.DataAccess.Abstracts.EntityFramework.Identity;
using Microsoft.EntityFrameworkCore;

namespace AppCore.DataAccess.Concretes.EntityFramework.Identity
{
    public sealed class IdentityRoleDal : IdentityRoleDalBase
    {
        public IdentityRoleDal(DbContext context) : base(context)
        {

        }
    }
}
