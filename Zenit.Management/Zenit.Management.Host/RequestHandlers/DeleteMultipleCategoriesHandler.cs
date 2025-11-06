using MediatR;

using Zenit.Management.Business.Services.CategoryServices;
using Zenit.Management.Contract.Requests.CategoryRequests;

namespace Zenit.Management.Host.RequestHandlers
{
    public class DeleteMultipleCategoriesHandler(CategoryService categoryService) : IRequestHandler<DeleteMultipleCategoriesRequest>
    {
        public async Task Handle(DeleteMultipleCategoriesRequest request, CancellationToken cancellationToken)
        {
            await categoryService.DeleteMany(request);
        }
    }
}