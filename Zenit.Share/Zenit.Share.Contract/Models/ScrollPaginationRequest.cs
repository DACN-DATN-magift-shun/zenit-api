namespace Zenit.Share.Contract.Models
{
    public class ScrollPaginationRequest
    {
        public string? Search { get; set; } 
        public Guid? BeforeId { get; set; }
        public required int PageSize { get; set; }
        public bool UseCountTotal { get; set; } = true;
    }
}