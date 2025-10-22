namespace Zenit.Share.Common.Interfaces
{

    public interface ICurrentAccount
    {
        Guid Id { get; set; }
        string Username { get; set; }
        string Email { get; set; }
        string? Phone { get; set; }
        string? Address { get; set; }
    }
}
