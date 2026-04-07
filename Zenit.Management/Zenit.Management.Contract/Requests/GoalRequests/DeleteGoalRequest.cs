using MediatR;

using Zenit.Management.Common.Enums;

namespace Zenit.Management.Contract.Requests.GoalRequests
{
    public class DeleteGoalRequest : IRequest
    {
        public required Guid Id { get; set; }
    }

    public class DeleteGoalResponse
    {
    }
}