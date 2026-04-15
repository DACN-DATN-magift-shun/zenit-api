using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Zenit.Management.Contract.Requests.LoanRequests;

namespace Zenit.Management.Host.Controllers
{
    [Authorize(Policy = "JwtOrInternal")]
    [ApiController]
    [Route("[controller]")]
    public class LoansController : ManagementControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllLoanRequest request)
        {
            return await GetRequest<GetAllLoanRequest, GetAllLoanResponse>(request);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetail([FromRoute] Guid id)
        {
            var request = new GetDetailLoanRequest { Id = id };
            return await GetRequest<GetDetailLoanRequest, GetDetailLoanResponse>(request);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateLoanRequest request)
        {
            return await CreateRequest<CreateLoanRequest, CreateLoanResponse>(request);
        }

        [HttpPost("many")]
        public async Task<IActionResult> CreateMany([FromBody] CreateManyLoanRequest request)
        {
            return await CreateRequest<CreateManyLoanRequest, CreateManyLoanResponse>(request);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateLoanRequest request)
        {
            if (id != request.Id)
            {
                return BadRequest("ID in route does not match ID in request body.");
            }
            return await UpdateRequest<UpdateLoanRequest, UpdateLoanResponse>(request);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var request = new DeleteLoanRequest { Id = id };
            return await DeleteRequest(request);
        }
    }
}