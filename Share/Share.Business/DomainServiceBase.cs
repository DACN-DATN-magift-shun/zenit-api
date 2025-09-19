using System.Linq.Expressions;

using Share.Business.Interfaces;
using Share.Data.Interfaces;


namespace Share.Business
{
    public abstract class DomainServiceBase<TEntity>(IRepository<TEntity> repository)
        : IDomainService<TEntity> where TEntity : class
    {
        public IRepository<TEntity> Repository { get; } = repository;

        public IQueryable<TEntity> GetAll()
        {
            return Repository.GetAll();
        }

        public IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            return Repository.FindBy(predicate);
        }

        public IQueryable<TEntity> ApplyFilter(IQueryable<TEntity> source, string? filter)
        {
            return Repository.ApplyFilter(source, filter);
        }

        public TEntity Add(TEntity entity)
        {
            return Repository.Add(entity);
        }

        public List<TEntity> AddRange(List<TEntity> entities)
        {
            return Repository.AddRange(entities);
        }

        public TEntity Update(TEntity entity)
        {
            return Repository.Update(entity);
        }

        public List<TEntity> UpdateRange(List<TEntity> entities)
        {
            return Repository.UpdateRange(entities);
        }

        public TEntity Delete(TEntity entity)
        {
            return Repository.Delete(entity);
        }

        public List<TEntity> DeleteRange(List<TEntity> entities)
        {
            return Repository.DeleteRange(entities);
        }
    }
}