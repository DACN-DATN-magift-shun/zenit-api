using MediatR;

using Zenit.Management.Common.Enums;

namespace Zenit.Management.Contract.Requests.GoalRequests
{
    public class GetDetailGoalRequest : IRequest<GetDetailGoalResponse>
    {
        public required Guid Id { get; set; }
    }

    public class GetDetailGoalResponse
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required int TargetAmount { get; set; }
        public int CurrentAmount { get; set; } = 0;
        public required string BackgroundColor { get; set; }
        public required string Icon { get; set; }
        public DateTime? DueDate { get; set; }
        public string? Note { get; set; }
        public required GoalStatus Status { get; set; }
    }
}