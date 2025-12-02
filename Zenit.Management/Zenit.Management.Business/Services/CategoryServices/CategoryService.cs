using Mapster;

using Zenit.Management.Business.Managers.CategoryManager;
using Zenit.Management.Contract.Requests.CategoryRequests;
using Zenit.Management.Data.Entities;

namespace Zenit.Management.Business.Services.CategoryServices
{
    public class CategoryService(IServiceProvider serviceProvider) : ManagementApplicationService(serviceProvider)
    {
        private CategoryManager _CategoryManager => GetService<CategoryManager>();
        private CategorySettingsManager _CategorySettingsManager => GetService<CategorySettingsManager>();

        public Task<GetCategoryResponse> CategoryGetDetail(GetCategoryRequest request)
        {
            var category = _CategoryManager.FindBy(c => c.Id == request.Id && c.IsDeleted == false).FirstOrDefault();

            if (category == null)
            {
                throw new Exception("Category not found");
            }

            var categorySettings = _CategorySettingsManager.FindBy(cs => cs.CategoryId == category.Id && cs.IsDeleted == false).FirstOrDefault();

            return Task.FromResult(new GetCategoryResponse
            {
                Name = category.Name,
                Icon = category.Icon,
                Color = category.Color,
                BackgroundColor =  category.BackgroundColor,
                ExpenseLimit = categorySettings?.ExpenseLimit,
                ExpenseAlertThreshold = categorySettings?.ExpenseAlertThreshold,
                GroupType = category.GroupType,
            });
        }

        public Task<GetCategoryGroupResponse> GetCategoryGroup(GetCategoryGroupRequest request)
        {
            var categories = _CategoryManager.FindBy(
                c => c.GroupType == request.GroupType &&
                c.IsDeleted == false &&
                (c.AccountId == CurrentAccount.Id || c.AccountId == null)).ToList();

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
            category.AccountId = CurrentAccount.Id;

            _CategoryManager.Add(category);

            var categorySettings = new CategorySettings
            {
                Id = Guid.NewGuid(),
                CategoryId = category.Id,
                AccountId = CurrentAccount.Id,
                ExpenseLimit = request.ExpenseLimit ?? null,
                ExpenseAlertThreshold = request.ExpenseAlertThreshold ?? null,
            };

            if (request.ExpenseLimit.HasValue || request.ExpenseAlertThreshold.HasValue)
            {
                _CategorySettingsManager.Add(categorySettings);
            }

            await UnitOfWork.SaveChangesAsync();

            var response = new CreateCategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
                Icon = category.Icon,
                Color = category.Color,
                BackgroundColor = category.BackgroundColor,
                ExpenseLimit = categorySettings.ExpenseLimit ?? null,
                ExpenseAlertThreshold = categorySettings.ExpenseAlertThreshold ?? null,
                GroupType = category.GroupType,
            };

            return response;
        }

        public async Task<UpdateCategoryResponse> Update(UpdateCategoryRequest request)
        {
            var category = _CategoryManager.FindBy(c => c.Id == request.Id).FirstOrDefault();

            if (category.AccountId == null)
            {
                if (!string.IsNullOrEmpty(request.Name) || !string.IsNullOrWhiteSpace(request.Icon))
                    throw new Exception("Cannot update default category name or icon");
            }

            request.Adapt(category);
            _CategoryManager.Update(category);

            if (request.ExpenseLimit.HasValue || request.ExpenseAlertThreshold.HasValue)
            {
                var categorySettings = _CategorySettingsManager.FindBy(cs => cs.CategoryId == category.Id).FirstOrDefault();
                if (categorySettings == null)
                {
                    _CategorySettingsManager.Add(new CategorySettings
                    {
                        Id = Guid.NewGuid(),
                        CategoryId = category.Id,
                        AccountId = CurrentAccount.Id,
                        ExpenseLimit = request.ExpenseLimit ?? null,
                        ExpenseAlertThreshold = request.ExpenseAlertThreshold ?? null,
                    });
                }
                else
                {
                    categorySettings.ExpenseLimit = request.ExpenseLimit ?? categorySettings.ExpenseLimit;
                    categorySettings.ExpenseAlertThreshold = request.ExpenseAlertThreshold ?? categorySettings.ExpenseAlertThreshold;
                }

                _CategorySettingsManager.Update(categorySettings);
            }

            await UnitOfWork.SaveChangesAsync();
            return new UpdateCategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
                Icon = category.Icon,
                ExpenseLimit = request.ExpenseLimit,
                ExpenseAlertThreshold = request.ExpenseAlertThreshold,
                GroupType = category.GroupType,
            };
        }

        public async Task Delete(DeleteCategoryRequest request)
        {
            var category = _CategoryManager.FindBy(c => c.Id == request.Id).FirstOrDefault();

            _CategoryManager.Delete(category);

            var categorySettings = _CategorySettingsManager.FindBy(cs => cs.CategoryId == category.Id).FirstOrDefault();
            if (categorySettings != null)
            {
                _CategorySettingsManager.Delete(categorySettings);
            }

            await UnitOfWork.SaveChangesAsync();
        }

        public async Task DeleteMany(DeleteMultipleCategoriesRequest request)
        {
            var categories = _CategoryManager.FindBy(c => request.Ids.Contains(c.Id)).ToList();

            _CategoryManager.DeleteRange(categories);

            var categorySettings = _CategorySettingsManager.FindBy(cs => request.Ids.Contains(cs.CategoryId)).ToList();
            foreach (var categorySetting in categorySettings)
            {
                if (categorySetting != null)
                    _CategorySettingsManager.Delete(categorySetting);
            }

            await UnitOfWork.SaveChangesAsync();
        }
    }
}