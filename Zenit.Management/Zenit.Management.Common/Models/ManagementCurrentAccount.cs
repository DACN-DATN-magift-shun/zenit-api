using Zenit.Share.Common.Interfaces;


namespace Zenit.Management.Common.Models
{
    public class ManagementCurrentAccount : ICurrentAccount
    {
        public required Guid Id { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
    }
}


