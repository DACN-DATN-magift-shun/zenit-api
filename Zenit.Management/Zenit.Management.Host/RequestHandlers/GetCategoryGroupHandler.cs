using MediatR;

using Zenit.Management.Business.Services.CategoryServices;
using Zenit.Management.Contract.Requests.CategoryRequests;

namespace Zenit.Management.Host.RequestHandlers
{
    public class GetCategoryGroupHandler(CategoryService categoryService) : IRequestHandler<GetCategoryGroupRequest, GetCategoryGroupResponse>
    {
        public async Task<GetCategoryGroupResponse> Handle(GetCategoryGroupRequest request, CancellationToken cancellationToken)
        {
            return await categoryService.GetCategoryGroup(request);
        }
    }
}