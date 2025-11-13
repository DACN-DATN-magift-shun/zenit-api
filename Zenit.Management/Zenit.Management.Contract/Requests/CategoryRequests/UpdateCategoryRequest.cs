using MediatR;

namespace Zenit.Management.Contract.Requests.CategoryRequests
{
    public class UpdateCategoryRequest : IRequest<UpdateCategoryResponse>
    {
        public required Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Icon { get; set; }
        public int? ExpenseLimit { get; set; }
        public float? ExpenseAlertThreshold { get; set; }
        public Guid? GroupId { get; set; }
    }

    public class UpdateCategoryResponse
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Icon { get; set; }
        public int? ExpenseLimit { get; set; }
        public float? ExpenseAlertThreshold { get; set; }
        public required Guid GroupId { get; set; }
    }
}