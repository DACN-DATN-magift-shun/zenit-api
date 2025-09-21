using System.Linq.Expressions;


namespace Share.Business.Interfaces
{
    public interface IDomainService<TSchema> where TSchema : class
    {
        IQueryable<TSchema> GetAll();
        IQueryable<TSchema> FindBy(Expression<Func<TSchema, bool>> predicate);
        IQueryable<TSchema> ApplyFilter(IQueryable<TSchema> source, string? filter);
        TSchema Add(TSchema entity);
        List<TSchema> AddRange(List<TSchema> entities);
        TSchema Update(TSchema entity);
        List<TSchema> UpdateRange(List<TSchema> entities);
        TSchema Delete(TSchema entity);
        List<TSchema> DeleteRange(List<TSchema> entities);
    }
}