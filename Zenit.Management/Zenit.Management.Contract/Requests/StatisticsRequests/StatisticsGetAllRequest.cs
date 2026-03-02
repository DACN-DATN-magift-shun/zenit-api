using MediatR;

using Zenit.Share.Common.Enums;

namespace Zenit.Management.Contract.Requests.StatisticsRequests
{
    public class StatisticsGetAllRequest : IRequest<StatisticsGetAllResponse>
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }

    public class StatisticsGetAllResponse
    {
       public IEnumerable<StatisticsResponseItem> GroupStatistics { get; set; } = [];
       public IncomeExpenseStatistics IncomeExpenseStatistics { get; set; }
    }

    public class StatisticsResponseItem
    {
        public required long TotalAmount { get; set; }
        public float? Percentage { get; set; }
        public float? PercentageChange { get; set; }
        public CategoryGroupType? GroupType { get; set; }
        public IEnumerable<CategoryStatistics> Categories { get; set; } = [];
    }

    public class CategoryStatistics
    {
        public required long TotalAmount { get; set; }
        public float? Percentage { get; set; }
        public float? PercentageChange { get; set; }
        public string? CategoryName { get; set; }
    }

    public class IncomeExpenseStatistics
    {
        public long TotalIncome { get; set; }
        public long TotalExpense { get; set; }
        public float? IncomePercentageChange { get; set; }
        public float? ExpensePercentageChange { get; set; }
    }
}