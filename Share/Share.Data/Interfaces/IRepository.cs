using System.Linq.Expressions;


namespace Share.Data.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> ApplyFilter(IQueryable<TEntity> source, string? filter);
        TEntity Add(TEntity entity);
        List<TEntity> AddRange(List<TEntity> entities);
        TEntity Update(TEntity entity);
        List<TEntity> UpdateRange(List<TEntity> entities);
        TEntity Delete(TEntity entity);
        List<TEntity> DeleteRange(List<TEntity> entities);
    }
}