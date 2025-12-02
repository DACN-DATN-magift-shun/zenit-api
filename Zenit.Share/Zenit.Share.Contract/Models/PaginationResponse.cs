using Microsoft.EntityFrameworkCore;

namespace Zenit.Share.Contract.Models
{
    public class PaginationResponse<TItem>
    {
        public List<TItem> Items { get; set; }
        public PaginationMeta Meta { get; set; }

        public static PaginationResponse<TItem> Create(IQueryable<TItem> queryable, PaginationRequest request)
        {
            var TotalItems = queryable.Count();

            if (string.IsNullOrEmpty(request.OrderBy) == false)
            {
                var isDesc = request.OrderDirection?.ToLower() == "desc";

                if (isDesc)
                {
                    queryable = queryable.OrderByDescending(e => EF.Property<object>(e, request.OrderBy));
                }
                else
                {
                    queryable = queryable.OrderBy(e => EF.Property<object>(e, request.OrderBy));
                }
            }
            var items = queryable
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();
                
            var meta = new PaginationMeta
            {
                TotalItems = request.UseCountTotal ? TotalItems : null,
                PageCount = request.UseCountTotal && TotalItems > 0 ? (int)Math.Ceiling((double)TotalItems / request.PageSize) : null,
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
            var query = queryable.OrderByDescending(e => EF.Property<Guid>(e, "Id")).AsQueryable();
    
            if (request.BeforeId.HasValue)
            {
                query = query.Where(e => EF.Property<Guid>(e, "Id") < request.BeforeId.Value);
            }
            
            var items = query.Take(request.PageSize).ToList();
            
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