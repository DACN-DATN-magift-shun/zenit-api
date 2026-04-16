using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Zenit.Management.Contract.Requests.ConversationRequests;


namespace Zenit.Management.Host.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ConversationsController : ManagementControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateConversationRequest request)
        {
            return await CreateRequest<CreateConversationRequest, CreateConversationResponse>(request);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllConversationRequest request)
        {
            return await GetRequest<GetAllConversationRequest, GetAllConversationResponse>(request);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetail([FromRoute] Guid id)
        {
            var request = new GetDetailConversationRequest { Id = id };
            return await GetRequest<GetDetailConversationRequest, GetDetailConversationResponse>(request);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var request = new DeleteConversationRequest { Id = id };
            return await DeleteRequest(request);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateConversationRequest request)
        {
            if (id != request.Id)
            {
                return BadRequest("ID in route does not match ID in request body.");
            }
            return await UpdateRequest<UpdateConversationRequest, UpdateConversationResponse>(request);
        }
    }
}