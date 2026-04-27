using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Zenit.Management.Contract.Requests.MessageRequests;



namespace Zenit.Management.Host.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class MessagesController : ManagementControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateMessageRequest request)
        {
            return await CreateRequest<CreateMessageRequest, CreateMessageResponse>(request);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllMessageRequest request)
        {
            return await GetRequest<GetAllMessageRequest, GetAllMessageResponse>(request);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetail([FromRoute] Guid id)
        {
            var request = new GetDetailMessageRequest { Id = id };
            return await GetRequest<GetDetailMessageRequest, GetDetailMessageResponse>(request);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var request = new DeleteMessageRequest { Id = id };
            return await DeleteRequest(request);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateMessageRequest request)
        {
            if (id != request.Id)
            {
                return BadRequest("ID in route does not match ID in request body.");
            }
            return await UpdateRequest<UpdateMessageRequest, UpdateMessageResponse>(request);
        }

        [HttpGet("stream")]
        public async Task<IActionResult> StreamMessages([FromQuery] StreamMessageRequest request)
        {
            return await StreamRequest<StreamMessageRequest, StreamMessageResponse>(request);
        }
    }
}