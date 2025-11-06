using MediatR;

using Zenit.Management.Business.Services.CategoryServices;
using Zenit.Management.Contract.Requests.CategoryRequests;

namespace Zenit.Management.Host.RequestHandlers
{
    public class CreateCategoryHandler(CategoryService categoryService) : IRequestHandler<CreateCategoryRequest, CreateCategoryResponse>
    {
        public async Task<CreateCategoryResponse> Handle(CreateCategoryRequest request, CancellationToken cancellationToken)
        {
            return await categoryService.Create(request);
        }
    }
}