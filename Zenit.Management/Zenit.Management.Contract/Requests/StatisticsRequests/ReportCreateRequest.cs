using MediatR;

namespace Zenit.Management.Contract.Requests.StatisticsRequests
{
    public class ReportCreateRequest : IRequest<ReportCreateResponse>
    {
        public required DateTime FromDate { get; set; }
        public required DateTime ToDate { get; set; }
    }

    public class ReportCreateResponse
    {
        public required string ReportUrl { get; set; }
    }
}