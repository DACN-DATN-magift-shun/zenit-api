using System.ComponentModel.DataAnnotations;

namespace Zenit.Share.Contract.Models
{
    public class PaginationRequest
    {
        public string? Search { get; set; }

        public string? OrderBy { get; set; }
        public string? OrderDirection { get; set; }

        public bool UseCountTotal { get; set; } = true;
        
        [Range(1, int.MaxValue)]
        public required int Page { get; set; }
        [Range(1, 1000)]
        public required int PageSize { get; set; }
    }
}