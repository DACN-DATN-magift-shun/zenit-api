using MediatR;

namespace Zenit.Management.Contract.Requests.CategoryRequests
{
    public class DeleteCategoryRequest : IRequest
    {
        public required Guid Id { get; set; }
    }
}