using MediatR;

using Zenit.Management.Data.Entities;
using Zenit.Share.Common.Enums;

namespace Zenit.Management.Contract.Requests.CategoryRequests
{
    public class GetCategoryGroupRequest : IRequest<GetCategoryGroupResponse>
    {
        public required Guid GroupId { get; set; }
    }

    public class GetCategoryGroupResponse
    {
        public required string Name { get; set; }
        public required CategoryGroupType Type { get; set; }
        public List<Category>? Categories { get; set; }
    }
}