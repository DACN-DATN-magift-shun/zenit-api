using MediatR;

using Zenit.Management.Business.Services.StatisticsServices;
using Zenit.Management.Contract.Requests.StatisticsRequests;

namespace Zenit.Management.Host.RequestHandlers
{
    public class CreateReportHandler(StatisticsService statisticsService) : IRequestHandler<ReportCreateRequest, ReportCreateResponse>
    {
        public async Task<ReportCreateResponse> Handle(ReportCreateRequest request, CancellationToken cancellationToken)
        {
            return await statisticsService.GenerateReport(request);
        }
    }
}