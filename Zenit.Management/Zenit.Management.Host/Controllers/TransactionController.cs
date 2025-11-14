using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Zenit.Management.Contract.TransactionRequests;

namespace Zenit.Management.Host.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class TransactionsController : ManagementControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll(GetAllTransactionRequest request)
        {
            return await GetRequest<GetAllTransactionRequest, GetAllTransactionResponse>(request);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetail([FromRoute] Guid id, [FromBody] GetDetailTransactionRequest request)
        {
            if (id != request.Id)
            {
                return BadRequest("Route id does not match body id.");
            }
            return await GetRequest<GetDetailTransactionRequest, GetDetailTransactionResponse>(request);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTransactionRequest request)
        {
            return await CreateRequest<CreateTransactionRequest, CreateTransactionResponse>(request);
        }

        // not work
        [HttpPost("many")]
        public async Task<IActionResult> CreateMany([FromBody] CreateManyTransactionsRequest request)
        {
            return await CreateRequest<CreateManyTransactionsRequest, CreateManyTransactionsResponse>(request);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateTransactionRequest request)
        {
            if (id != request.Id)
            {
                return BadRequest("Route id does not match body id.");
            }
            return await UpdateRequest<UpdateTransactionRequest, UpdateTransactionResponse>(request);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id, [FromBody] DeleteTransactionRequest request)
        {
            if (id != request.Id)
            {
                return BadRequest("Route id does not match body id.");
            }
            return await DeleteRequest(request);
        }

        // not work
        [HttpPatch]
        public async Task<IActionResult> UpdateMany([FromBody] UpdateManyTransactionsRequest request)
        {
            return await UpdateRequest<UpdateManyTransactionsRequest, UpdateManyTransactionsResponse>(request);
        }
    }
}