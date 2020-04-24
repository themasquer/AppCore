﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AppCore.Records.Abstracts;

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

        public virtual IQueryable<TEntity> GetEntityQuery()
        {
            try
            {
                if (_isDeletedEntityProperty != null)
                {
                    return _context.Set<TEntity>().Where(e => EF.Property<bool?>(e, _isDeletedEntityProperty) == null || EF.Property<bool?>(e, _isDeletedEntityProperty) == false).AsQueryable();
                }
                return _context.Set<TEntity>().AsQueryable();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public virtual IQueryable<TEntity> GetSoftDeletedEntityQuery()
        {
            try
            {
                if (_isDeletedEntityProperty != null)
                {
                    return _context.Set<TEntity>().Where(e => EF.Property<bool?>(e, _isDeletedEntityProperty) == true).AsQueryable();
                }
                return null;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public virtual IQueryable<TEntity> GetEntityQuery(params string[] entitiesToInclude)
        {
            try
            {
                var queryEntity = GetEntityQuery();
                foreach (string entityToInclude in entitiesToInclude)
                {
                    queryEntity = queryEntity.Include(entityToInclude);
                }
                return queryEntity;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public virtual IQueryable<TEntity> GetEntityQuery(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return GetEntityQuery().Where(predicate);
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

        public virtual List<TEntity> GetEntities()
        {
            try
            {
                return GetEntityQuery().ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public virtual async Task<List<TEntity>> GetEntitiesAsync()
        {
            try
            {
                return await GetEntityQuery().ToListAsync();
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

        public virtual List<TEntity> GetEntities(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return GetEntityQuery(predicate).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public virtual async Task<List<TEntity>> GetEntitiesAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return await GetEntityQuery(predicate).ToListAsync();
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

        public virtual TEntity GetEntity(int id)
        {
            try
            {
                return GetEntityQuery().SingleOrDefault(e => e.Id == id);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public virtual async Task<TEntity> GetEntityAsync(int id)
        {
            try
            {
                return await GetEntityQuery().SingleOrDefaultAsync(e => e.Id == id);
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

        public virtual TEntity GetEntity(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return GetEntityQuery().SingleOrDefault(predicate);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public virtual async Task<TEntity> GetEntityAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return await GetEntityQuery().SingleOrDefaultAsync(predicate);
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

        public virtual int? AddEntity(TEntity entity)
        {
            try
            {
                var contextEntity = _context.Entry(entity);
                contextEntity.State = EntityState.Added;
                if (Commit)
                    SaveChanges();
                return entity.Id;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public virtual async Task<int?> AddEntityAsync(TEntity entity)
        {
            try
            {
                var contextEntity = _context.Entry(entity);
                contextEntity.State = EntityState.Added;
                if (Commit)
                    await SaveChangesAsync();
                return entity.Id;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public virtual int? UpdateEntity(TEntity entity)
        {
            try
            {
                var contextEntity = _context.Entry(entity);
                contextEntity.State = EntityState.Modified;
                if (Commit)
                    SaveChanges();
                return entity.Id;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public virtual async Task<int?> UpdateEntityAsync(TEntity entity)
        {
            try
            {
                var contextEntity = _context.Entry(entity);
                contextEntity.State = EntityState.Modified;
                if (Commit)
                    await SaveChangesAsync();
                return entity.Id;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public virtual int? DeleteEntity(int id)
        {
            try
            {
                var entity = GetEntity(id);
                return DeleteEntity(entity);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public virtual int? DeleteEntity(TEntity entity)
        {
            try
            {
                var id = entity.Id;
                var contextEntity = _context.Entry(entity);
                contextEntity.State = EntityState.Deleted;
                if (Commit)
                    SaveChanges();
                return id;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public virtual async Task<int?> DeleteEntityAsync(TEntity entity)
        {
            try
            {
                var id = entity.Id;
                var contextEntity = _context.Entry(entity);
                contextEntity.State = EntityState.Deleted;
                if (Commit)
                    await SaveChangesAsync();
                return id;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public virtual int? SaveChanges()
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

        public virtual async Task<int?> SaveChangesAsync()
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

        public virtual int? HardDeleteSoftDeletedEntities()
        {
            try
            {
                int deletedCount = 0;
                var query = GetSoftDeletedEntityQuery();
                if (query != null)
                {
                    var entities = query.ToList();
                    Commit = false;
                    foreach (var entity in entities)
                    {
                        DeleteEntity(entity);
                    }
                    _context.SaveChanges();
                }
                return deletedCount;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #region Dispose
        protected bool disposed;

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
