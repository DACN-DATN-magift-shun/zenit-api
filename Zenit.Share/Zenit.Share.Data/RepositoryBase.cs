using System;
using System.Linq.Expressions;

using Zenit.Share.Data.Interfaces;


namespace Zenit.Share.Data
{
    public abstract class RepositoryBase<TSchema, TID>(TID? accountId) : IRepository<TSchema>
        where TSchema : class
    {
        TID? AccountId { get; } = accountId;

        public void SetCreateAuditProperties(TSchema entity)
        {
            if (entity is ICreationAuditModel creationAuditModel)
            {
                creationAuditModel.CreatedAt = DateTime.Now;
            }

            if (entity is IDeletionAuditModel deletionAuditModel)
            {
                deletionAuditModel.IsDeleted = false;
                deletionAuditModel.DeletedAt = null;
            }
        }

        public void SetUpdateAuditProperties(TSchema entity)
        {
            if (entity is IModificationAuditModel modificationAuditModel)
            {
                modificationAuditModel.LastModifiedAt = DateTime.Now;
            }
        }

        public void SetDeleteAuditProperties(TSchema entity)
        {
            if (entity is IDeletionAuditModel deletionAuditModel)
            {
                deletionAuditModel.IsDeleted = true;
                deletionAuditModel.DeletedAt = DateTime.Now;
            }
        }

        public abstract IQueryable<TSchema> GetAll();
        public abstract IQueryable<TSchema> FindBy(Expression<Func<TSchema, bool>> predicate);
        public abstract IQueryable<TSchema> ApplyFilter(IQueryable<TSchema> source, string? filter);
        public abstract TSchema Add(TSchema entity);
        public abstract List<TSchema> AddRange(List<TSchema> entities);
        public abstract TSchema Update(TSchema entity);
        public abstract List<TSchema> UpdateRange(List<TSchema> entities);
        public abstract TSchema Delete(TSchema entity);
        public abstract List<TSchema> DeleteRange(List<TSchema> entities);
    }
}

