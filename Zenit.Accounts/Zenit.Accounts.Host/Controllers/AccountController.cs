using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Zenit.Accounts.Contract.Requests;


namespace Zenit.Accounts.Host.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : AccountControllerBase
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

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetDetail(string id)
        {
            var request = new AccountGetDetailRequest { Id = id };
            return await GetRequest<AccountGetDetailRequest, AccountGetDetailResponse>(request);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] AccountUpdateRequest request)
        {

            return await UpdateRequest<AccountUpdateRequest, AccountUpdateResponse>(request);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var request = new AccountDeleteRequest { Id = id };
            return await DeleteRequest(request);
        }
    }
}
