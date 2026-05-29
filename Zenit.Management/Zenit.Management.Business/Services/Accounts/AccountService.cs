using Task = System.Threading.Tasks.Task;

using Mapster;

using Zenit.Management.Business.Constants;
using Zenit.Management.Business.Helpers;
using Zenit.Management.Business.Managers;
using Zenit.Management.Contract.Errors;
using Zenit.Management.Contract.Requests.AccountRequests;
using Zenit.Management.Data;
using Zenit.Management.Data.Entities;
using Zenit.Share.Business.Requests;

using SendGrid.Helpers.Mail;



namespace Zenit.Management.Business.Services.Accounts
{
    public class AccountService(IServiceProvider serviceProvider) : ManagementApplicationService(serviceProvider)
    {
        private AccountManager _AccountManager => GetService<AccountManager>();
        private ManagementRedisCache _RedisCache => GetService<ManagementRedisCache>();
        private ManagementSendGridEmailService _EmailService => GetService<ManagementSendGridEmailService>();
        // private ManagementResendEmailService _EmailService => GetService<ManagementResendEmailService>();

        public async Task<AccountCreateResponse> Create(AccountCreateRequest request)
        {
            var salt = Pbkdf2Helpers.GenerateSalt();
            var hashedPassword = Pbkdf2Helpers.HashPassword(request.Password, salt);
            request.Password = hashedPassword;

            var account = Mapper.Map<Account>(request);
            _AccountManager.Add(account);

            await UnitOfWork.SaveChangesAsync();
            return Mapper.Map<AccountCreateResponse>(account);
        }

        public async Task<AccountLoginResponse> Login(AccountLoginRequest request)
        {
            var account = _AccountManager.GetAll()
                .FirstOrDefault(current => current.Email == request.Email && current.IsDeleted == false)
                ?? throw new Exception(AccountErrors.ACCOUNT_NOT_FOUND);

            var password = Convert.FromBase64String(account.Password);

            var saltBytes = new byte[16];
            var hashedPasswordBytes = new byte[32];
            Array.Copy(password, 0, saltBytes, 0, 16);
            Array.Copy(password, 16, hashedPasswordBytes, 0, 32);

            // Hash input password với salt đã extract
            var hashedInputPassword = Pbkdf2Helpers.HashPassword(request.Password, saltBytes);
            var inputPasswordBytes = Convert.FromBase64String(hashedInputPassword);

            // Extract chỉ phần hash (bỏ salt) để so sánh
            var inputHashBytes = new byte[32];
            Array.Copy(inputPasswordBytes, 16, inputHashBytes, 0, 32);

            // So sánh 2 hash (không bao gồm salt)
            bool isPasswordValid = true;
            for (int i = 0; i < hashedPasswordBytes.Length; i++)
            {
                if (inputHashBytes[i] != hashedPasswordBytes[i])
                {
                    isPasswordValid = false;
                }
            }

            if (!isPasswordValid)
            {
                throw new Exception(AccountErrors.WRONG_PASSWORD);
            }

            var tokens = JwtHelpers.GenerateJwtTokens(account);
            await UnitOfWork.SaveChangesAsync();
            return Mapper.Map<AccountLoginResponse>(tokens);
        }

        public async Task<AccountIsEmailExistResponse> VerifyEmailExistance(AccountIsEmailExistRequest request)
        {
            var isExist = _AccountManager.GetAll().FirstOrDefault(current => current.Email == request.Email && !current.IsDeleted) != null;
            return new AccountIsEmailExistResponse { Result = isExist };
        }

        public async Task<AccountSendOTPResponse> SendOTP(AccountSendOTPRequest request)
        {
            var From = EmailServiceConstants.FROM_EMAIL;
            var FromName = EmailServiceConstants.FROM_NAME;
            var To = request.Email;
            var Subject = "Zenit send OTP for reset password";

            var OTP = new Random().Next(100000, 999999).ToString();
            var TextContent = "Your OTP for resetting password is: " + OTP;

            var cacheKey = $"OTP:{request.Email}";
            await _RedisCache.AddAsync(cacheKey, OTP, DateTimeOffset.UtcNow.AddMinutes(2));

            var sendEmailResponse = await _EmailService.SendEmailAsync(
                new SendGridEmailRequest
                {
                    From = new EmailAddress(From, FromName),
                    To = new EmailAddress(To),
                    Subject = Subject,
                    PlainTextContent = TextContent,
                    HtmlContent = $"<p>{TextContent}</p>"
                }
            );

            return Mapper.Map<AccountSendOTPResponse>(sendEmailResponse);
        }

        public async Task<AccountVerifyOTPResponse> VerifyOTP(AccountVerifyOTPRequest request)
        {
            var cacheKey = $"OTP:{request.Email}";
            var cachedOTP = await _RedisCache.GetAsync<string>(cacheKey);

            if (cachedOTP == null)
            {
                throw new Exception(AccountErrors.OTP_EXPIRED);
            }

            if (cachedOTP != request.OTP)
            {
                throw new Exception(AccountErrors.INVALID_OTP);
            }

            await _RedisCache.RemoveAsync(cacheKey);

            var resetToken = Guid.NewGuid().ToString();
            var resetTokenCacheKey = $"ResetToken:{resetToken}";
            var resetTokenValue = _AccountManager.FindBy(current => current.Email == request.Email && !current.IsDeleted)
                .Select(current => current.Id)
                .FirstOrDefault();

            await _RedisCache.AddAsync(resetTokenCacheKey, resetTokenValue, DateTimeOffset.UtcNow.AddMinutes(10));

            return Mapper.Map<AccountVerifyOTPResponse>(
                new AccountVerifyOTPResponse
                {
                    ResetToken = resetToken
                }
            );
        }

        public async Task<AccountResetPasswordResponse> ResetPassword(AccountResetPasswordRequest request)
        {
            var resetTokenCacheKey = $"ResetToken:{request.ResetToken}";
            var accountId = await _RedisCache.GetAsync<string>(resetTokenCacheKey);

            if (accountId == null)
            {
                throw new Exception(AccountErrors.INVALID_OR_EXPIRED_RESET_TOKEN);
            }

            await _RedisCache.RemoveAsync(resetTokenCacheKey);

            var salt = Pbkdf2Helpers.GenerateSalt();
            var hashedPassword = Pbkdf2Helpers.HashPassword(request.NewPassword, salt);

            var account = _AccountManager.FindBy(current => current.Id.ToString() == accountId && !current.IsDeleted).FirstOrDefault();
            account.Password = hashedPassword;
            _AccountManager.Update(account);

            await UnitOfWork.SaveChangesAsync();

            return Mapper.Map<AccountResetPasswordResponse>(
                new AccountResetPasswordResponse
                {
                    StatusCode = 200,
                    Message = "Password reset successful"
                }
            );
        }

        public Task<AccountGetDetailResponse> GetDetail(AccountGetDetailRequest request)
        {
            var account = _AccountManager.FindBy(current => current.Id == CurrentAccount.Id && !current.IsDeleted).FirstOrDefault();

            if (account == null)
            {
                throw new Exception(AccountErrors.ACCOUNT_NOT_FOUND);
            }

            return Task.FromResult(Mapper.Map<AccountGetDetailResponse>(account!));
        }

        public async Task<AccountUpdateResponse> Update(AccountUpdateRequest request)
        {
            var account = _AccountManager
                .FindBy(current => current.Id == CurrentAccount.Id && !current.IsDeleted)
                .FirstOrDefault();

            if (account == null)
            {
                throw new Exception(AccountErrors.ACCOUNT_NOT_FOUND);
            }

            request.Adapt(account);
            _AccountManager.Update(account!);

            await UnitOfWork.SaveChangesAsync();
            return Mapper.Map<AccountUpdateResponse>(account!);
        }

        public async Task Delete(AccountDeleteRequest request)
        {
            var account = _AccountManager
                .FindBy(current => current.Id == CurrentAccount.Id)
                .FirstOrDefault();

            _AccountManager.Delete(account!);

            await UnitOfWork.SaveChangesAsync();
            return;
        }


    }
}
