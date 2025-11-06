using MediatR;

using Zenit.Management.Business.Services.CategoryServices;
using Zenit.Management.Contract.Requests.CategoryRequests;

namespace Zenit.Management.Host.RequestHandlers
{
    public class GetCategoryHandler(CategoryService categoryService) : IRequestHandler<GetCategoryRequest, GetCategoryResponse>
    {
        public async Task<GetCategoryResponse> Handle(GetCategoryRequest request, CancellationToken cancellationToken)
        {
            return await categoryService.GetCategory(request);
        }
    }
}