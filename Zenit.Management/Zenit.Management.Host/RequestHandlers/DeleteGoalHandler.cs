using MediatR;

using Zenit.Management.Business.Services.GoalServices;
using Zenit.Management.Contract.Requests.GoalRequests;

namespace Zenit.Management.Host.RequestHandlers
{
    public class DeleteGoalHandler(GoalService goalService) : IRequestHandler<DeleteGoalRequest>
    {
        public async Task Handle(DeleteGoalRequest request, CancellationToken cancellationToken)
        {
            await goalService.Delete(request);
        }
    }
}