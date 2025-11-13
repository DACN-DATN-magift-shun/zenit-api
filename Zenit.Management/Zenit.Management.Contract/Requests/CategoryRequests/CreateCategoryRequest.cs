using MediatR;

namespace Zenit.Management.Contract.Requests.CategoryRequests
{
    public class CreateCategoryRequest : IRequest<CreateCategoryResponse>
    {
        public required string Name { get; set; }
        public required string Icon { get; set; }
        public int? ExpenseLimit { get; set; }
        public float? ExpenseAlertThreshold { get; set; }
        public required Guid GroupId { get; set; }
    }

    public class CreateCategoryResponse
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Icon { get; set; }
        public int? ExpenseLimit { get; set; }
        public float? ExpenseAlertThreshold { get; set; }
        public required Guid GroupId { get; set; }
    }
}