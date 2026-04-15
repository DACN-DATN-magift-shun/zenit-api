using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Zenit.Management.Contract.Request.WalletRequests;

namespace Zenit.Management.Host.Controllers
{
    [Authorize(Policy = "JwtOrInternal")]
    [ApiController]
    [Route("[controller]")]
    public class WalletsController : ManagementControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateWalletRequest request)
        {
            return await CreateRequest<CreateWalletRequest, CreateWalletResponse>(request);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllWalletRequest request)
        {
            return await GetRequest<GetAllWalletRequest, GetAllWalletResponse>(request);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetail([FromRoute] Guid id)
        {
            var request = new GetDetailWalletRequest { Id = id };   
            return await GetRequest<GetDetailWalletRequest, GetDetailWalletResponse>(request);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWalletRequest request)
        {
            if (id != request.Id)
            {
                return BadRequest("ID in route does not match ID in request body.");
            }
            return await UpdateRequest<UpdateWalletRequest, UpdateWalletResponse>(request);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var request = new DeleteWalletRequest { Id = id };
            return await DeleteRequest(request);
        }
    }
}