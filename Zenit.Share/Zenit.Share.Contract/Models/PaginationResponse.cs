using Microsoft.EntityFrameworkCore;

namespace Zenit.Share.Contract.Models
{
    public class PaginationResponse<TItem>
    {
        public List<TItem> Items { get; set; }
        public PaginationMeta Meta { get; set; }

        public static PaginationResponse<TItem> Create(IQueryable<TItem> queryable, PaginationRequest request)
        {
            if (string.IsNullOrEmpty(request.OrderBy) == false)
            {
                var isDesc = request.OrderDirection?.ToLower() == "desc";

                if (isDesc)
                {
                    queryable = queryable.OrderByDescending(e => EF.Property<string>(e, request.OrderBy));
                }
                else
                {
                    queryable = queryable.OrderBy(e => EF.Property<string>(e, request.OrderBy));
                }
            }
            var items = queryable
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();
                
            var meta = new PaginationMeta
            {
                TotalItems = request.UseCountTotal ? queryable.Count() : null,
                PageCount = queryable != null ? (int)Math.Ceiling((double)queryable.Count() / request.PageSize) : null,
                Page = request.Page,
                PageSize = request.PageSize,
            };
            
            return new PaginationResponse<TItem>
            {
                Items = items,
                Meta = meta
            };
        }

        public static PaginationResponse<TItem> Create(IQueryable<TItem> queryable, ScrollPaginationRequest request)
        {
            var items = queryable
                .OrderByDescending(e => EF.Property<Guid>(e, "Id"))
                .Where(e => EF.Property<Guid>(e, "Id").CompareTo(request.BeforeId) < 0)
                .Take(request.PageSize)
                .ToList();
            
            var meta = new PaginationMeta
            {
                TotalItems = request.UseCountTotal ? queryable.Count() : null,
                PageCount = queryable != null ? (int)Math.Ceiling((double)queryable.Count() / request.PageSize) : null,
                PageSize = request.PageSize,
            };

            return new PaginationResponse<TItem>
            {
                Items = items,
                Meta = meta
            };
        }
    }
}