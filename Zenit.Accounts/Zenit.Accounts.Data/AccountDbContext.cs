using MongoDB.Driver;
using System.Reflection;

using Zenit.Accounts.Data.Attributes;


namespace Zenit.Accounts.Data
{
    public class AccountDbContext
    {
        private readonly IMongoDatabase _database;
        public AccountDbContext(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public IMongoDatabase GetDatabase()
        {
            return _database;
        }

        public IMongoCollection<TSchema> GetCollection<TSchema>()
        {
            var type = typeof(TSchema);
            var attribute = type.GetCustomAttributes<BsonCollectionAttribute>();

            try
            {
                return _database.GetCollection<TSchema>(attribute.First().CollectionName);
            }
            catch
            {
                throw new Exception($"Collection name for {type.Name} not found.");
            }
        }
    }
}
