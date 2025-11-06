using MediatR;

namespace Zenit.Management.Contract.Requests.CategoryRequests
{
    public class DeleteMultipleCategoriesRequest : IRequest
    {
        public required List<Guid> Ids { get; set; }
    }

    public class DeleteMultipleCategoriesResponse
    {
    }
}