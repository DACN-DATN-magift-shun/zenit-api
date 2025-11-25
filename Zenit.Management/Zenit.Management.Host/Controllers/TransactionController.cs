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
        public async Task<IActionResult> GetAll()
        {
            var request = new GetAllTransactionRequest();
            return await GetRequest<GetAllTransactionRequest, GetAllTransactionResponse>(request);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetail([FromRoute] Guid id)
        {
            var request = new GetDetailTransactionRequest { Id = id };
            return await GetRequest<GetDetailTransactionRequest, GetDetailTransactionResponse>(request);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTransactionRequest request)
        {
            return await CreateRequest<CreateTransactionRequest, UpdateTransactionResponse>(request);
        }

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
                return BadRequest("ID in route does not match ID in request body.");
            }
            return await UpdateRequest<UpdateTransactionRequest, UpdateTransactionResponse>(request);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var request = new DeleteTransactionRequest { Id = id };
            return await DeleteRequest(request);
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateMany([FromBody] UpdateManyTransactionsRequest request)
        {
            return await UpdateRequest<UpdateManyTransactionsRequest, UpdateManyTransactionsResponse>(request);
        }
    }
}