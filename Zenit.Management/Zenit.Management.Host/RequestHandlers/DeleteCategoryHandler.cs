using MediatR;

using Zenit.Management.Business.Services.CategoryServices;
using Zenit.Management.Contract.Requests.CategoryRequests;

namespace Zenit.Management.Host.RequestHandlers
{
    public class DeleteCategoryHandler(CategoryService categoryService) : IRequestHandler<DeleteCategoryRequest>
    {
        public async Task Handle(DeleteCategoryRequest request, CancellationToken cancellationToken)
        {
            await categoryService.Delete(request);
        }
    }
}