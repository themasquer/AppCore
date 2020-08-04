using System;
using Microsoft.EntityFrameworkCore;

namespace AppCore.DataAccess.Abstracts.EntityFramework
{
    public abstract class SqlBase
    {
        private readonly DbContext _context;

        protected SqlBase(DbContext context)
        {
            _context = context;
        }

        public virtual void ExecuteSql(string sql)
        {
            try
            {
                _context.Database.ExecuteSqlRaw(sql);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
