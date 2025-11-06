using MediatR;

using Zenit.Management.Business.Services.CategoryServices;
using Zenit.Management.Contract.Requests.CategoryRequests;

namespace Zenit.Management.Host.RequestHandlers
{
    public class UpdateCategoryHandler(CategoryService categoryService) : IRequestHandler<UpdateCategoryRequest, UpdateCategoryResponse>
    {
        public async Task<UpdateCategoryResponse> Handle(UpdateCategoryRequest request, CancellationToken cancellationToken)
        {
            return await categoryService.Update(request);
        }
    }
}