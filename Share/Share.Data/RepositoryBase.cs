using System;
using System.Linq.Expressions;

using Share.Data.Interfaces;


namespace Share.Data
{
    public abstract class RepositoryBase<TEntity, TID>(TID? accountId) : IRepository<TEntity>
        where TEntity : class
        where TID : struct
    {
        TID? AccountId { get; } = accountId;

        public void SetCreateAuditProperties(TEntity entity)
        {
            if (entity is ICreationAuditModel<TID> creationAuditModel)
            {
                creationAuditModel.CreatedId = AccountId;
                creationAuditModel.CreatedAt = DateTime.Now;
            }

            if(entity is IDeletionAuditModel<TID> deletionAuditModel)
            {
                deletionAuditModel.IsDeleted = false;
            }
        }

        public void SetUpdateAuditProperties(TEntity entity)
        {
            if (entity is IModificationAuditModel<TID> modificationAuditModel)
            {
                modificationAuditModel.LastModifiedId = AccountId;
                modificationAuditModel.LastModifiedAt = DateTime.Now;
            }
        }

        public bool SetDeleteAuditProperties(TEntity entity)
        {
            if (entity is IDeletionAuditModel<TID> deletionAuditModel)
            {
                deletionAuditModel.IsDeleted = true;
                deletionAuditModel.DeletedId = AccountId;
                deletionAuditModel.DeletedAt = DateTime.Now;
                return true;
            }
            
            return false;
        }


        public abstract IQueryable<TEntity> GetAll();
        public abstract IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);
        public abstract IQueryable<TEntity> ApplyFilter(IQueryable<TEntity> source, string? filter);
        public abstract TEntity Add(TEntity entity);
        public abstract List<TEntity> AddRange(List<TEntity> entities);
        public abstract TEntity Update(TEntity entity);
        public abstract List<TEntity> UpdateRange(List<TEntity> entities);
        public abstract TEntity Delete(TEntity entity);
        public abstract List<TEntity> DeleteRange(List<TEntity> entities);
    }
}
