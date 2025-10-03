using MongoDB.Bson;

using Share.Common.Interfaces;


namespace Accounts.Common.Models
{
    public class CurrentAccount : ICurrentAccount<ObjectId>
    {
        public required ObjectId Id { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
    }
}

