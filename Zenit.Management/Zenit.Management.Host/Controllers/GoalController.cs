using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Zenit.Management.Contract.Requests.GoalRequests;

namespace Zenit.Management.Host.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class GoalsController : ManagementControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateGoalRequest request)
        {
            return await CreateRequest<CreateGoalRequest, CreateGoalResponse>(request);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllGoalRequest request)
        {
            return await GetRequest<GetAllGoalRequest, GetAllGoalResponse>(request);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetail([FromRoute] Guid id)
        {
            var request = new GetDetailGoalRequest { Id = id };
            return await GetRequest<GetDetailGoalRequest, GetDetailGoalResponse>(request);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var request = new DeleteGoalRequest { Id = id };
            return await DeleteRequest(request);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateGoalRequest request)
        {
            if (id != request.Id)
            {
                return BadRequest("ID in route does not match ID in request body.");
            }
            return await UpdateRequest<UpdateGoalRequest, UpdateGoalResponse>(request);
        }
    }
}