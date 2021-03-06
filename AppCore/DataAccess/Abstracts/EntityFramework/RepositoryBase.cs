using AppCore.Records.Abstracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AppCore.DataAccess.Abstracts.EntityFramework
{
    public abstract class RepositoryBase<TEntity> : IDisposable where TEntity : RecordBase, new()
    {
        private readonly DbContext _context;
        private readonly string _isDeletedEntityProperty;

        public bool Commit { get; set; }

        protected RepositoryBase(DbContext context)
        {
            _context = context;
            _isDeletedEntityProperty = typeof(IRecordSoftDelete).GetProperties().FirstOrDefault().Name;
            if (typeof(TEntity).GetProperty(_isDeletedEntityProperty) == null)
            {
                _isDeletedEntityProperty = null;
            }
            Commit = true;
        }

        public virtual IQueryable<TEntity> GetEntityQuery(params string[] entitiesToInclude)
        {
            try
            {
                IQueryable<TEntity> queryEntity = _context.Set<TEntity>().AsQueryable();
                foreach (string entityToInclude in entitiesToInclude)
                {
                    queryEntity = queryEntity.Include(entityToInclude);
                }
                if (_isDeletedEntityProperty != null)
                {
                    queryEntity = queryEntity.Where(e => EF.Property<bool?>(e, _isDeletedEntityProperty) == null || EF.Property<bool?>(e, _isDeletedEntityProperty) == false).AsQueryable();
                }
                return queryEntity;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public virtual IQueryable<TEntity> GetEntityQueryIncludingSoftDeleted(params string[] entitiesToInclude)
        {
            try
            {
                IQueryable<TEntity> queryEntity = _context.Set<TEntity>().AsQueryable();
                foreach (string entityToInclude in entitiesToInclude)
                {
                    queryEntity = queryEntity.Include(entityToInclude);
                }
                if (_isDeletedEntityProperty != null)
                {
                    return queryEntity;
                }
                return null;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public virtual IQueryable<TEntity> GetSoftDeletedEntityQuery(params string[] entitiesToInclude)
        {
            try
            {
                IQueryable<TEntity> queryEntity = _context.Set<TEntity>().AsQueryable();
                foreach (string entityToInclude in entitiesToInclude)
                {
                    queryEntity = queryEntity.Include(entityToInclude);
                }
                if (_isDeletedEntityProperty != null)
                {
                    return queryEntity.Where(e => EF.Property<bool?>(e, _isDeletedEntityProperty) == true).AsQueryable();
                }
                return null;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public virtual IQueryable<TEntity> GetEntityQuery(Expression<Func<TEntity, bool>> predicate, params string[] entitiesToInclude)
        {
            try
            {
                var queryEntity = GetEntityQuery(entitiesToInclude);
                return queryEntity.Where(predicate);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public virtual List<TEntity> GetEntities(params string[] entitiesToInclude)
        {
            try
            {
                var queryEntity = GetEntityQuery(entitiesToInclude);
                return queryEntity.ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public virtual async Task<List<TEntity>> GetEntitiesAsync(params string[] entitiesToInclude)
        {
            try
            {
                var queryEntity = GetEntityQuery(entitiesToInclude);
                return await queryEntity.ToListAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public virtual List<TEntity> GetEntities(Expression<Func<TEntity, bool>> predicate, params string[] entitiesToInclude)
        {
            try
            {
                var queryEntity = GetEntityQuery(entitiesToInclude);
                return queryEntity.Where(predicate).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public virtual async Task<List<TEntity>> GetEntitiesAsync(Expression<Func<TEntity, bool>> predicate, params string[] entitiesToInclude)
        {
            try
            {
                var queryEntity = GetEntityQuery(entitiesToInclude);
                return await queryEntity.Where(predicate).ToListAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public virtual TEntity GetEntity(int id, params string[] entitiesToInclude)
        {
            try
            {
                return GetEntityQuery(entitiesToInclude).SingleOrDefault(e => e.Id == id);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public virtual async Task<TEntity> GetEntityAsync(int id, params string[] entitiesToInclude)
        {
            try
            {
                return await GetEntityQuery(entitiesToInclude).SingleOrDefaultAsync(e => e.Id == id);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public virtual TEntity GetEntity(string guid, params string[] entitiesToInclude)
        {
            try
            {
                return GetEntityQuery(entitiesToInclude).SingleOrDefault(e => e.Guid == guid);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public virtual async Task<TEntity> GetEntityAsync(string guid, params string[] entitiesToInclude)
        {
            try
            {
                return await GetEntityQuery(entitiesToInclude).SingleOrDefaultAsync(e => e.Guid == guid);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public virtual TEntity GetEntity(Expression<Func<TEntity, bool>> predicate, params string[] entitiesToInclude)
        {
            try
            {
                return GetEntityQuery(entitiesToInclude).SingleOrDefault(predicate);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public virtual async Task<TEntity> GetEntityAsync(Expression<Func<TEntity, bool>> predicate, params string[] entitiesToInclude)
        {
            try
            {
                return await GetEntityQuery(entitiesToInclude).SingleOrDefaultAsync(predicate);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public virtual bool EntityExists(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                if (GetEntityQuery().Any(predicate))
                    return true;
                return false;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public virtual async Task<bool> EntityExistsAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                if (await GetEntityQuery().AnyAsync(predicate))
                    return true;
                return false;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public virtual int GetEntityCount()
        {
            try
            {
                return GetEntityQuery().Count();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public virtual async Task<int> GetEntityCountAsync()
        {
            try
            {
                return await GetEntityQuery().CountAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public virtual long GetEntityLongCount()
        {
            try
            {
                return GetEntityQuery().LongCount();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public virtual async Task<long> GetEntityLongCountAsync()
        {
            try
            {
                return await GetEntityQuery().LongCountAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public virtual int GetEntityCount(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return GetEntityQuery().Count(predicate);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public virtual async Task<int> GetEntityCountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return await GetEntityQuery().CountAsync(predicate);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public virtual long GetEntityLongCount(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return GetEntityQuery().LongCount(predicate);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public virtual async Task<long> GetEntityLongCountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return await GetEntityQuery().LongCountAsync(predicate);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public virtual void AddEntity(TEntity entity)
        {
            try
            {
                entity.Guid = Guid.NewGuid().ToString();
                _context.Set<TEntity>().Add(entity);
                if (Commit)
                    SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public virtual async Task AddEntityAsync(TEntity entity)
        {
            try
            {
                entity.Guid = Guid.NewGuid().ToString();
                _context.Set<TEntity>().Add(entity);
                if (Commit)
                    await SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public virtual void UpdateEntity(TEntity entity)
        {
            try
            {
                _context.Set<TEntity>().Update(entity);
                if (Commit)
                    SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public virtual async Task UpdateEntityAsync(TEntity entity)
        {
            try
            {
                _context.Set<TEntity>().Update(entity);
                if (Commit)
                    await SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public virtual void DeleteEntity(int id)
        {
            try
            {
                var entity = GetEntity(id);
                DeleteEntity(entity);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public virtual async Task DeleteEntityAsync(int id)
        {
            try
            {
                var entity = await GetEntityAsync(id);
                await DeleteEntityAsync(entity);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public virtual void DeleteEntity(string guid)
        {
            try
            {
                var entity = GetEntity(guid);
                DeleteEntity(entity);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public virtual async Task DeleteEntityAsync(string guid)
        {
            try
            {
                var entity = await GetEntityAsync(guid);
                await DeleteEntityAsync(entity);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public virtual void DeleteEntity(TEntity entity)
        {
            try
            {
                _context.Set<TEntity>().Remove(entity);
                if (Commit)
                    SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public virtual async Task DeleteEntityAsync(TEntity entity)
        {
            try
            {
                _context.Set<TEntity>().Remove(entity);
                if (Commit)
                    await SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public virtual int SaveChanges()
        {
            try
            {
                if (_isDeletedEntityProperty != null)
                {
                    foreach (var entry in _context.ChangeTracker.Entries<TEntity>())
                    {
                        switch (entry.State)
                        {
                            case EntityState.Added:
                                entry.CurrentValues[_isDeletedEntityProperty] = false;
                                break;
                            case EntityState.Deleted:
                                entry.CurrentValues[_isDeletedEntityProperty] = true;
                                entry.State = EntityState.Modified;
                                break;
                        }
                    }
                }
                Commit = true;
                return _context.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public virtual async Task<int> SaveChangesAsync()
        {
            try
            {
                if (_isDeletedEntityProperty != null)
                {
                    foreach (var entry in _context.ChangeTracker.Entries<TEntity>())
                    {
                        switch (entry.State)
                        {
                            case EntityState.Added:
                                entry.CurrentValues[_isDeletedEntityProperty] = false;
                                break;
                            case EntityState.Deleted:
                                entry.CurrentValues[_isDeletedEntityProperty] = true;
                                entry.State = EntityState.Modified;
                                break;
                        }
                    }
                }
                Commit = true;
                return await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public virtual int HardDeleteSoftDeletedEntities()
        {
            try
            {
                int deletedCount = 0;
                var queryEntity = GetSoftDeletedEntityQuery();
                if (queryEntity != null)
                {
                    var entities = queryEntity.ToList();
                    Commit = false;
                    foreach (var entity in entities)
                    {
                        DeleteEntity(entity);
                        deletedCount++;
                    }
                    _context.SaveChanges();
                }
                Commit = true;
                return deletedCount;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #region Dispose
        private bool disposed = false;

        protected void Dispose(bool disposing)
        {
            if (!disposed && disposing)
            {
                _context?.Dispose();
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
