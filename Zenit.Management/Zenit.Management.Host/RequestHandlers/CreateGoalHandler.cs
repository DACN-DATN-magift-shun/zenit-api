using MediatR;

using Zenit.Management.Business.Services.GoalServices;
using Zenit.Management.Contract.Requests.GoalRequests;

namespace Zenit.Management.Host.RequestHandlers
{
    public class CreateGoalHandler(GoalService goalService) : IRequestHandler<CreateGoalRequest, CreateGoalResponse>
    {
        public async Task<CreateGoalResponse> Handle(CreateGoalRequest request, CancellationToken cancellationToken)
        {
            return await goalService.Create(request);
        }
    }
}