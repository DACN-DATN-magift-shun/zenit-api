using MongoDB.Bson;
using System.Linq.Expressions;
using MongoDB.Driver;
using System.Reflection;

using Zenit.Share.Data;
using Zenit.Accounts.Common.Models;


namespace Zenit.Accounts.Data
{
    public class AccountRepository<TSchema>(
        CurrentAccount currentAccount,
        AccountDbContext context
    ) : RepositoryBase<TSchema, ObjectId>(currentAccount.Id)
        where TSchema : class
    {

        private readonly AccountDbContext _context = context;

        public override IQueryable<TSchema> GetAll()
        {
            return _context.GetCollection<TSchema>().AsQueryable();
        }

        public override IQueryable<TSchema> FindBy(Expression<Func<TSchema, bool>> predicate)
        {
            return _context.GetCollection<TSchema>().AsQueryable().Where(predicate);
        }

        public override IQueryable<TSchema> ApplyFilter(IQueryable<TSchema> source, string? filter)
        {
            if (string.IsNullOrEmpty(filter))
            {
                return source;
            }

            try
            {
                var filterDefinition = new JsonFilterDefinition<TSchema>(filter);
                return _context.GetCollection<TSchema>().Find(filterDefinition).ToEnumerable().AsQueryable();
            }
            catch
            {
                return source;
            }
        }

        public override TSchema Add(TSchema record)
        {
            SetCreateAuditProperties(record);
            _context.GetCollection<TSchema>().InsertOne(record);

            return record;
        }

        public override List<TSchema> AddRange(List<TSchema> records)
        {
            records.ForEach(SetCreateAuditProperties);
            _context.GetCollection<TSchema>().InsertMany(records);
            return records;
        }

        public override TSchema Update(TSchema record)
        {
            SetUpdateAuditProperties(record);
            var recordId = typeof(TSchema).GetProperty("Id")?.GetValue(record);
            var filter = Builders<TSchema>.Filter
                .Eq("Id", recordId);

            _context.GetCollection<TSchema>()
                .FindOneAndUpdate(
                    filter,
                    record.ToBsonDocument()
                );

            return record;
        }

        public override List<TSchema> UpdateRange(List<TSchema> records)
        {
            records.ForEach(SetUpdateAuditProperties);

            var bulkOps = records.Select(record =>
                new UpdateOneModel<TSchema>(
                    Builders<TSchema>.Filter.Eq(
                        "Id",
                        typeof(TSchema).GetProperty("Id")?.GetValue(record)
                    ),
                    record.ToBsonDocument()
                )).ToList();

            _context.GetCollection<TSchema>().BulkWrite(bulkOps);
            return records;
        }

        public override TSchema Delete(TSchema record)
        {
            SetDeleteAuditProperties(record);
            Update(record);
            return record;
        }

        public override List<TSchema> DeleteRange(List<TSchema> records)
        {
            records.ForEach(SetDeleteAuditProperties);
            UpdateRange(records);
            return records;
        }
    }
}
