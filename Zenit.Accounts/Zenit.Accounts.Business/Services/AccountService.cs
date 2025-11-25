using Mapster;

using MediatR;

using Zenit.Accounts.Business.Helpers;
using Zenit.Accounts.Business.Managers;
using Zenit.Accounts.Contract.Errors;
using Zenit.Accounts.Contract.Requests;
using Zenit.Accounts.Data.Entities;


namespace Zenit.Accounts.Business.Services
{
    public class AccountService(IServiceProvider serviceProvider) : AccountApplicationService(serviceProvider)
    {
        private AccountManager _AccountManager => GetService<AccountManager>();

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

        // public Task<AccountForgotPasswordRequest> ForgotPassword(AccountForgotPasswordRequest request)
        // {
        //     return Task.FromResult(request);
        // }

        // public Task<AccountResetPasswordResponse> ResetPassword(AccountResetPasswordRequest request)
        // {
        //     return Task.FromResult(new AccountResetPasswordResponse());
        // }

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
