using Zenit.Share.Data.Interfaces;

namespace Zenit.Management.Data.Entities
{
    public class ManagementSeederHistory : ISeederHistory<Guid>
    {
        public Guid Id { get; set; }
        public string SeederName { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}