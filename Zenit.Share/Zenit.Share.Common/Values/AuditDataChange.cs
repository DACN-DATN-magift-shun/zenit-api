namespace Zenit.Share.Common.Values
{
    public class AuditDataChange
    {
        public required string Field { get; set; }
        public object? OriginalValue { get; set; }
        public object? NewValue { get; set; }
    }
}