using System.Linq.Expressions;


namespace Zenit.Share.Data.Interfaces
{
    public interface IRepository<TEntity> 
        where TEntity : class
    {
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> ApplyFilter(IQueryable<TEntity> source, string? filter);
        IQueryable<TEntity> Search(string? searchTerm);
        TEntity Add(TEntity entity);
        List<TEntity> AddRange(List<TEntity> entities);
        TEntity Update(TEntity entity);
        List<TEntity> UpdateRange(List<TEntity> entities);
        TEntity Delete(TEntity entity);
        List<TEntity> DeleteRange(List<TEntity> entities);
        TEntity Remove(TEntity entity);
        List<TEntity> RemoveRange(List<TEntity> entities);
    }
}
