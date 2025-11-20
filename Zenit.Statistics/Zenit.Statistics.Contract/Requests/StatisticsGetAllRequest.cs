using MediatR;

using Zenit.Share.Common.Enums;

namespace Zenit.Statistics.Contract.Requests
{
    public class StatisticsGetAllRequest : IRequest<StatisticsGetAllResponse>
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }

    public class StatisticsGetAllResponse
    {
       IEnumerable<StatisticsResponseItem> Items { get; set; } = [];
    }

    public class StatisticsResponseItem
    {
        public required long TotalAmount { get; set; }
        public float? Percentage { get; set; }
        public float? PercentageChange { get; set; }
        public CategoryGroupType? GroupType { get; set; }
        public IEnumerable<CategoryStatistics> Details { get; set; } = [];
    }

    public class CategoryStatistics
    {
        public required long TotalAmount { get; set; }
        public float? Percentage { get; set; }
        public float? PercentageChange { get; set; }
        public Guid? CategoryId { get; set; }
    }
}