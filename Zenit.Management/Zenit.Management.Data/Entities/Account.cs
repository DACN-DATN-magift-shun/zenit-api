using Zenit.Management.Data.Models;

namespace Zenit.Management.Data.Entities
{
    public class Account : ManagementAuditModel
    {
        public required string Username { get; set; }
        public required string Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public required string Password { get; set; }
    }
}