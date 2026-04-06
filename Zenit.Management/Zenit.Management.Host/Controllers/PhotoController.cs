using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Zenit.Management.Contract.Requests.PhotoRequests;

namespace Zenit.Management.Host.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PhotosController : ManagementControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreatePhotoRequest request)
        {
            return await CreateRequest<CreatePhotoRequest, CreatePhotoResponse>(request);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllPhotoRequest request)
        {
            return await GetRequest<GetAllPhotoRequest, GetAllPhotoResponse>(request);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetail([FromRoute] Guid id)
        {
            var request = new GetDetailPhotoRequest { Id = id };
            return await GetRequest<GetDetailPhotoRequest, GetDetailPhotoResponse>(request);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var request = new DeletePhotoRequest { Id = id };
            return await DeleteRequest(request);
        }
    }
}