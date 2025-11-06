using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

using Zenit.Share.Data.Interfaces;


namespace Zenit.Share.Data
{
    public abstract class RepositoryBase<TEntity, TID>(
        DbContext context,
        TID? accountId
    ) : IRepository<TEntity>
        where TEntity : class
        where TID : struct
    {
        DbContext _context { get; } = context;
        TID? AccountId { get; } = accountId;

        public void SetCreateAuditProperties(TEntity entity)
        {
            if (entity is ICreationAuditModel<TID> creationAuditModel)
            {
                creationAuditModel.CreatedAt = DateTime.UtcNow;
            }

            if (entity is IDeletionAuditModel<TID> deletionAuditModel)
            {
                deletionAuditModel.IsDeleted = false;
                deletionAuditModel.DeletedAt = null;
            }
        }

        public void SetUpdateAuditProperties(TEntity entity)
        {
            if (entity is IModificationAuditModel<TID> modificationAuditModel)
            {
                modificationAuditModel.LastModifiedAt = DateTime.UtcNow;
            }
        }

        public bool SetDeleteAuditProperties(TEntity entity)
        {
            if (entity is IDeletionAuditModel<TID> deletionAuditModel)
            {
                deletionAuditModel.IsDeleted = true;
                deletionAuditModel.DeletedAt = DateTime.UtcNow;
                return true;
            }
            return false;
        }

        public IQueryable<TEntity> GetAll()
        {
            var data = _context.Set<TEntity>().AsQueryable();
            return data;
        }

        public IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            var data = _context.Set<TEntity>().Where(predicate);
            return data;
        }

        public IQueryable<TEntity> ApplyFilter(IQueryable<TEntity> source, string? filter)
        {
            return source;
        }

        public IQueryable<TEntity> Search(string? searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return _context.Set<TEntity>().AsQueryable();
            }

            var data = _context.Set<TEntity>().Where(e => e.ToString().Contains(searchTerm));
            return data;
        }

        public TEntity Add(TEntity entity)
        {
            SetCreateAuditProperties(entity);
            _context.Set<TEntity>().Add(entity);
            return entity;
        }

        public List<TEntity> AddRange(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                SetCreateAuditProperties(entity);
            }
            _context.Set<TEntity>().AddRange(entities);
            return entities;
        }

        public TEntity Update(TEntity entity)
        {
            SetUpdateAuditProperties(entity);
            _context.Set<TEntity>().Update(entity);
            return entity;
        }

        public List<TEntity> UpdateRange(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                SetUpdateAuditProperties(entity);
            }
            _context.Set<TEntity>().UpdateRange(entities);
            return entities;
        }
        
        public TEntity Delete(TEntity entity)
        {
            SetDeleteAuditProperties(entity);
            _context.Set<TEntity>().Update(entity);
            return entity;
        }

        public List<TEntity> DeleteRange(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                SetDeleteAuditProperties(entity);
            }
            _context.Set<TEntity>().UpdateRange(entities);
            return entities;
        }

        public TEntity Remove(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
            return entity;
        }

        public List<TEntity> RemoveRange(List<TEntity> entities)
        {
            _context.Set<TEntity>().RemoveRange(entities);
            return entities;
        }
    }
}

