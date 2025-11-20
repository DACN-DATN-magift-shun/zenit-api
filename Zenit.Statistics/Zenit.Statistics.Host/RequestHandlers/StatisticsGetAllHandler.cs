using MediatR;

using Zenit.Statistics.Business.Services;
using Zenit.Statistics.Contract.Requests;

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