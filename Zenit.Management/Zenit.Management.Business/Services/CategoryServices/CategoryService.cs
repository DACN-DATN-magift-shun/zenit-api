using Mapster;

using Zenit.Management.Business.Managers.CategoryManager;
using Zenit.Management.Contract.Requests.CategoryRequests;
using Zenit.Management.Data.Entities;

namespace Zenit.Management.Business.Services.CategoryServices
{
    public class CategoryService(IServiceProvider serviceProvider) : ManagementApplicationService(serviceProvider)
    {
        private CategoryManager _CategoryManager => GetService<CategoryManager>();

        public Task<GetCategoryResponse> GetCategory(GetCategoryRequest request)
        {
            var category = _CategoryManager.FindBy(c => c.Id == request.Id).FirstOrDefault();

            return Task.FromResult(Mapper.Map<GetCategoryResponse>(category));
        }

        public Task<GetCategoryGroupResponse> GetCategoryGroup(GetCategoryGroupRequest request)
        {
            var categories = _CategoryManager.FindBy(c => c.GroupType == request.GroupType).ToList();

            var response = new
            {
                Name = request.GroupType,
                Type = (int)request.GroupType,
                Categories = categories,
            };

            return Task.FromResult(Mapper.Map<GetCategoryGroupResponse>(response));
        }

        public async Task<CreateCategoryResponse> Create(CreateCategoryRequest request)
        {
            var category = Mapper.Map<Category>(request);
            category.Id = Guid.NewGuid();

            _CategoryManager.Add(category);
            await UnitOfWork.SaveChangesAsync();
            return Mapper.Map<CreateCategoryResponse>(category);
        }

        public async Task<UpdateCategoryResponse> Update(UpdateCategoryRequest request)
        {
            var category = _CategoryManager.FindBy(c => c.Id == request.Id).FirstOrDefault();

            request.Adapt(category);

            _CategoryManager.Update(category);
            await UnitOfWork.SaveChangesAsync();
            return Mapper.Map<UpdateCategoryResponse>(category);
        }

        public async Task Delete(DeleteCategoryRequest request)
        {
            var category = _CategoryManager.FindBy(c => c.Id == request.Id).FirstOrDefault();

            _CategoryManager.Delete(category);
            await UnitOfWork.SaveChangesAsync();
        }

        public async Task DeleteMany(DeleteMultipleCategoriesRequest request)
        {
            var categories = _CategoryManager.FindBy(c => request.Ids.Contains(c.Id)).ToList();

            _CategoryManager.DeleteRange(categories);
            await UnitOfWork.SaveChangesAsync();
        }
    }
}