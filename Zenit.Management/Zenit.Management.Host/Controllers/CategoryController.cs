using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Zenit.Management.Contract.Requests.CategoryRequests;

namespace Zenit.Management.Host.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CategoryController : ManagementControllerBase
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(GetCategoryRequest request)
        {
            return await GetRequest<GetCategoryRequest, GetCategoryResponse>(request);
        }

        [HttpGet("groups/{groupId}")]
        public async Task<IActionResult> GetCategoryGroup(GetCategoryGroupRequest request)
        {
            return await GetRequest<GetCategoryGroupRequest, GetCategoryGroupResponse>(request);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest request)
        {
            return await CreateRequest<CreateCategoryRequest, CreateCategoryResponse>(request);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateCategory(string id, UpdateCategoryRequest request)
        {
            return await UpdateRequest<UpdateCategoryRequest, UpdateCategoryResponse>(request);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(string id, DeleteCategoryRequest request)
        {
            return await DeleteRequest(request);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteMultipleCategories([FromBody] DeleteMultipleCategoriesRequest request)
        {
            return await DeleteRequest(request);
        }
    }
}