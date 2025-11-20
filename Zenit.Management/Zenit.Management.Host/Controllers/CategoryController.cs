using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Zenit.Management.Contract.Requests.CategoryRequests;
using Zenit.Share.Common.Enums;

namespace Zenit.Management.Host.Controller
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CategoriesController : ManagementControllerBase
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(Guid id)
        {
            var request = new GetCategoryRequest { Id = id };
            return await GetRequest<GetCategoryRequest, GetCategoryResponse>(request);
        }

        [HttpGet]
        public async Task<IActionResult> GetCategoryGroup([FromQuery] CategoryGroupType groupType)
        {
            var request = new GetCategoryGroupRequest { GroupType = groupType };
            return await GetRequest<GetCategoryGroupRequest, GetCategoryGroupResponse>(request);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest request)
        {
            return await CreateRequest<CreateCategoryRequest, CreateCategoryResponse>(request);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryRequest request)
        {
            return await UpdateRequest<UpdateCategoryRequest, UpdateCategoryResponse>(request);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            var request = new DeleteCategoryRequest { Id = id };
            return await DeleteRequest(request);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteMultipleCategories([FromBody] DeleteMultipleCategoriesRequest request)
        {
            return await DeleteRequest(request);
        }
    }
}