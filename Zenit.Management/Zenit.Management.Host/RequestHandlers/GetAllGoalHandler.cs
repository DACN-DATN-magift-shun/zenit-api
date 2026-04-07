using MediatR;

using Zenit.Management.Business.Services.GoalServices;
using Zenit.Management.Contract.Requests.GoalRequests;

namespace Zenit.Management.Host.RequestHandlers
{
    public class GetAllGoalHandler(GoalService goalService) : IRequestHandler<GetAllGoalRequest, GetAllGoalResponse>
    {
        public async Task<GetAllGoalResponse> Handle(GetAllGoalRequest request, CancellationToken cancellationToken)
        {
            return await goalService.GetAll(request);
        }
    }
}