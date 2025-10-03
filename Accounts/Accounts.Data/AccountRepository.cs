using MongoDB.Bson;
using System.Linq.Expressions;
using MongoDB.Driver;
using System.Reflection;

using Share.Data;
using Accounts.Common.Models;


namespace Accounts.Data
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
            foreach (var record in records)
            {
                SetCreateAuditProperties(record);
            }
            _context.GetCollection<TSchema>().InsertMany(records);
            return records;
        }

        public override TSchema Update(TSchema record)
        {
            SetUpdateAuditProperties(record);
            var idProperty = typeof(TSchema).GetProperty("Id");
            var idValue = idProperty?.GetValue(record);
            var filter = Builders<TSchema>.Filter.Eq("Id", idValue);

            // Tạo UpdateDefinition từ các properties không null
            var updateBuilder = Builders<TSchema>.Update;
            var updates = new List<UpdateDefinition<TSchema>>();

            var properties = typeof(TSchema).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead && p.CanWrite && p.Name != "Id");

            foreach (var property in properties)
            {
                var value = property.GetValue(record);
                if (value != null)
                {
                    updates.Add(updateBuilder.Set(property.Name, value));
                }
            }

            if (updates.Any())
            {
                var combinedUpdate = updateBuilder.Combine(updates);
                _context.GetCollection<TSchema>().UpdateOne(filter, combinedUpdate);
            }

            return record;
        }

        public override List<TSchema> UpdateRange(List<TSchema> records)
        {
            foreach (var record in records)
            {
                var updatedRecord = Update(record);
            }
            return records;
        }

        public override TSchema Delete(TSchema record)
        {
            SetDeleteAuditProperties(record);
            var idProperty = typeof(TSchema).GetProperty("Id");
            var idValue = idProperty?.GetValue(record);
            var filter = Builders<TSchema>.Filter.Eq("Id", idValue);

            // Tạo UpdateDefinition từ các properties không null
            var updateBuilder = Builders<TSchema>.Update;
            var updates = new List<UpdateDefinition<TSchema>>();

            var properties = typeof(TSchema).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead && p.CanWrite && p.Name != "Id");

            foreach (var property in properties)
            {
                var value = property.GetValue(record);
                if (value != null)
                {
                    updates.Add(updateBuilder.Set(property.Name, value));
                }
            }

            if (updates.Any())
            {
                var combinedUpdate = updateBuilder.Combine(updates);
                _context.GetCollection<TSchema>().UpdateOne(filter, combinedUpdate);
            }

            return record;
        }

        public override List<TSchema> DeleteRange(List<TSchema> records)
        {
            foreach (var record in records)
            {
                var deletedRecord = Delete(record);
            }
            return records;
        }
    }
}