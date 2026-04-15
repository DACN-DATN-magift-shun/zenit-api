using MediatR;

using Zenit.Management.Business.Services.CategoryServices;
using Zenit.Management.Contract.Requests.CategoryRequests;

namespace Zenit.Management.Host.RequestHandlers
{
    public class GetAllCategoriesHandler(CategoryService categoryService) : IRequestHandler<GetAllCategoriesRequest, GetAllCategoriesResponse>
    {
        public async Task<GetAllCategoriesResponse> Handle(GetAllCategoriesRequest request, CancellationToken cancellationToken)
        {
            return await categoryService.GetAllCategories(request);
        }
    }
}