namespace Zenit.Management.Data.Models
{
    public class AccountTokenModel
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
    }
}
