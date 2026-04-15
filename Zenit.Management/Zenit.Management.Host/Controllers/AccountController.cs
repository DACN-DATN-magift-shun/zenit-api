using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Zenit.Management.Contract.Requests.AccountRequests;


namespace Zenit.Management.Host.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountsController : ManagementControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AccountCreateRequest request)
        {
            return await CreateRequest<AccountCreateRequest, AccountCreateResponse>(request);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AccountLoginRequest request)
        {
            return await CreateRequest<AccountLoginRequest, AccountLoginResponse>(request);
        }

        [HttpGet("me")]
        [Authorize(Policy = "JwtOrInternal")]
        public async Task<IActionResult> GetDetail()
        {
            var request = new AccountGetDetailRequest();
            return await GetRequest<AccountGetDetailRequest, AccountGetDetailResponse>(request);
        }

        [HttpPatch("me")]
        [Authorize(Policy = "JwtOrInternal")]
        public async Task<IActionResult> Update([FromBody] AccountUpdateRequest request)
        {

            return await UpdateRequest<AccountUpdateRequest, AccountUpdateResponse>(request);
        }

        [HttpDelete("me")]
        [Authorize(Policy = "JwtOrInternal")]
        public async Task<IActionResult> Delete()
        {
            var request = new AccountDeleteRequest();
            return await DeleteRequest(request);
        }

        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOTP([FromBody] AccountSendOTPRequest request)
        {
            return await CreateRequest<AccountSendOTPRequest, AccountSendOTPResponse>(request);
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOTP([FromBody] AccountVerifyOTPRequest request)
        {
            return await CreateRequest<AccountVerifyOTPRequest, AccountVerifyOTPResponse>(request);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] AccountResetPasswordRequest request)
        {
            return await CreateRequest<AccountResetPasswordRequest, AccountResetPasswordResponse>(request);
        }
    }
}
