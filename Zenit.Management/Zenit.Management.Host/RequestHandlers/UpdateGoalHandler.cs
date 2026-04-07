using MediatR;

using Zenit.Management.Business.Services.GoalServices;
using Zenit.Management.Contract.Requests.GoalRequests;

namespace Zenit.Management.Host.RequestHandlers
{
    public class UpdateGoalHandler(GoalService goalService) : IRequestHandler<UpdateGoalRequest, UpdateGoalResponse>
    {
        public async Task<UpdateGoalResponse> Handle(UpdateGoalRequest request, CancellationToken cancellationToken)
        {
            return await goalService.Update(request);
        }
    }
}