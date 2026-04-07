using MediatR;

using Zenit.Management.Business.Services.GoalServices;
using Zenit.Management.Contract.Requests.GoalRequests;

namespace Zenit.Management.Host.RequestHandlers
{
    public class GetDetailGoalHandler(GoalService goalService) : IRequestHandler<GetDetailGoalRequest, GetDetailGoalResponse>
    {
        public async Task<GetDetailGoalResponse> Handle(GetDetailGoalRequest request, CancellationToken cancellationToken)
        {
            return await goalService.GetDetail(request);
        }
    }
}