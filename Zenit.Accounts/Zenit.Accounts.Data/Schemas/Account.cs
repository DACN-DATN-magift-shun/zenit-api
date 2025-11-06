using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

using Zenit.Accounts.Data.Models;
using Zenit.Accounts.Data.Attributes;


namespace Zenit.Accounts.Data.Schemas
{
    [BsonCollection("account")]
    public class Account : AccountDataModel<ObjectId>
    {
        [BsonElement("username")]
        public required string Username { get; set; }

        [BsonElement("email")]
        public required string Email { get; set; }

        [BsonElement("phone")]
        public string? Phone { get; set; }

        [BsonElement("address")]
        public string? Address { get; set; }

        [BsonElement("hashed_password")]
        public required string Password { get; set; }
    }
}
