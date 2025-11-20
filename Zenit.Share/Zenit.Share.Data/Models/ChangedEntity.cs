using Microsoft.EntityFrameworkCore;

namespace Zenit.Share.Data.Models
{
    public class changedEntity
    {
        public object? EntityCurrentValues { get; set; }
        public EntityState EntityState { get; set; }
    }
}