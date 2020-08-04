using AppCore.DataAccess.Abstracts.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace AppCore.DataAccess.Concretes.EntityFramework
{
    public class Sql : SqlBase
    {
        public Sql(DbContext context) : base(context)
        {

        }
    }
}
