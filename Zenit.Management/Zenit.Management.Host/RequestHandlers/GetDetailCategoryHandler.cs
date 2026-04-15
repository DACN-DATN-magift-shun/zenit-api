using MediatR;

using Zenit.Management.Business.Services.CategoryServices;
using Zenit.Management.Contract.Requests.CategoryRequests;

namespace Zenit.Management.Host.RequestHandlers
{
    public class GetDetailCategoryHandler(CategoryService categoryService) : IRequestHandler<GetDetailCategoryRequest, GetDetailCategoryResponse>
    {
        public async Task<GetDetailCategoryResponse> Handle(GetDetailCategoryRequest request, CancellationToken cancellationToken)
        {
            return await categoryService.CategoryGetDetail(request);
        }
    }
}