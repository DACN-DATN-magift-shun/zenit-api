using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Zenit.Management.Contract.Request.MoneyTransferRequests;

namespace Zenit.Management.Host.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class MoneyTransfersController : ManagementControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateMoneyTransferRequest request)
        {
            return await CreateRequest<CreateMoneyTransferRequest, CreateMoneyTransferResponse>(request);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllMoneyTransferRequest request)
        {
            return await GetRequest<GetAllMoneyTransferRequest, GetAllMoneyTransferResponse>(request);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetail([FromRoute] Guid id)
        {
            var request = new GetDetailMoneyTransferRequest { Id = id };
            return await GetRequest<GetDetailMoneyTransferRequest, GetDetailMoneyTransferResponse>(request);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateMoneyTransferRequest request)
        {
            if (id != request.Id)
            {
                return BadRequest("ID in route does not match ID in request body.");
            }
            return await UpdateRequest<UpdateMoneyTransferRequest, UpdateMoneyTransferResponse>(request);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var request = new DeleteMoneyTransferRequest { Id = id };
            return await DeleteRequest(request);
        }
    }
}