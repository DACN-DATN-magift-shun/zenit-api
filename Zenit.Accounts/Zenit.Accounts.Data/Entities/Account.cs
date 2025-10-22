using Zenit.Accounts.Data.Models;

namespace Zenit.Accounts.Data.Entities
{
    public class Account : AccountAuditModel
    {
        public required string Username { get; set; }
        public required string Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public required string Password { get; set; }
    }
}