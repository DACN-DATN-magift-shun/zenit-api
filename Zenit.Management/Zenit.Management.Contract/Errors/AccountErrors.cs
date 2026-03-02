namespace Zenit.Management.Contract.Errors
{
    public class AccountErrors
    {
        public const string ACCOUNT_NOT_FOUND = "ACCOUNT_NOT_FOUND";
        public const string ACCOUNT_ALREADY_EXISTS = "ACCOUNT_ALREADY_EXISTS";
        public const string WRONG_PASSWORD = "WRONG_PASSWORD";
        public const string INVALID_OTP = "INVALID_OTP";
        public const string OTP_EXPIRED = "OTP_EXPIRED";
        public const string INVALID_OR_EXPIRED_RESET_TOKEN = "INVALID_OR_EXPIRED_RESET_TOKEN";
    }
}
