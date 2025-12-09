using MediatR;

using Zenit.Management.Business.Services;
using Zenit.Management.Contract.Requests.StatisticsRequests;

namespace Zenit.Statistics.Host.RequestHandlers
{
    public class StatisticsGetAllHandler(StatisticsService statisticsService) : IRequestHandler<StatisticsGetAllRequest, StatisticsGetAllResponse>
    {
        public async Task<StatisticsGetAllResponse> Handle(StatisticsGetAllRequest request, CancellationToken cancellationToken)
        {
            return await statisticsService.GetAll(request);
        }
    }
}