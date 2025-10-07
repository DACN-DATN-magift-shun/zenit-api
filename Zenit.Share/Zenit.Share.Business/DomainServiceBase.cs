using System.Linq.Expressions;

using Zenit.Share.Business.Interfaces;
using Zenit.Share.Data.Interfaces;


namespace Zenit.Share.Business
{
    public abstract class DomainServiceBase<TSchema>(IRepository<TSchema> repository)
        : IDomainService<TSchema> where TSchema : class
    {
        public IRepository<TSchema> Repository { get; } = repository;

        public IQueryable<TSchema> GetAll()
        {
            return Repository.GetAll();
        }

        public IQueryable<TSchema> FindBy(Expression<Func<TSchema, bool>> predicate)
        {
            return Repository.FindBy(predicate);
        }

        public IQueryable<TSchema> ApplyFilter(IQueryable<TSchema> source, string? filter)
        {
            return Repository.ApplyFilter(source, filter);
        }

        public TSchema Add(TSchema entity)
        {
            return Repository.Add(entity);
        }

        public List<TSchema> AddRange(List<TSchema> entities)
        {
            return Repository.AddRange(entities);
        }

        public TSchema Update(TSchema entity)
        {
            return Repository.Update(entity);
        }

        public List<TSchema> UpdateRange(List<TSchema> entities)
        {
            return Repository.UpdateRange(entities);
        }

        public TSchema Delete(TSchema entity)
        {
            return Repository.Delete(entity);
        }

        public List<TSchema> DeleteRange(List<TSchema> entities)
        {
            return Repository.DeleteRange(entities);
        }
    }
}
