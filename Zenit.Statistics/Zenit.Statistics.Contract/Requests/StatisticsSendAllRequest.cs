using MediatR;

namespace Zenit.Statistics.Contract.Requests
{
    public class StatisticsSendAllRequest : IRequest<StatisticsSendAllResponse>
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }

    public class StatisticsSendAllResponse
    {
        IEnumerable<StatisticsResponseItem> Items { get; set; } = [];
    }
}