using MediatR;

using Zenit.Management.Data.Entities;
using Zenit.Share.Contract.Models;

namespace Zenit.Management.Contract.TransactionRequests
{
    public class GetAllTransactionRequest : ScrollPaginationRequest, IRequest<GetAllTransactionResponse>
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public Guid? CategoryId { get; set; }
    }

    public class GetAllTransactionResponse : PaginationResponse<Transaction>
    {
    }
}