using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Zenit.Accounts.Contract.Requests;


namespace Zenit.Accounts.Host.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountsController : AccountControllerBase
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
        [Authorize]
        public async Task<IActionResult> GetDetail()
        {
            var request = new AccountGetDetailRequest();
            return await GetRequest<AccountGetDetailRequest, AccountGetDetailResponse>(request);
        }

        [HttpPatch("me")]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] AccountUpdateRequest request)
        {

            return await UpdateRequest<AccountUpdateRequest, AccountUpdateResponse>(request);
        }

        [HttpDelete("me")]
        [Authorize]
        public async Task<IActionResult> Delete()
        {
            var request = new AccountDeleteRequest();
            return await DeleteRequest(request);
        }
    }
}
