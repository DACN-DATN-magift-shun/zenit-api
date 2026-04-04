using MediatR;

using Zenit.Share.Contract.Models;

namespace Zenit.Management.Contract.Request.MoneyTransferRequests
{
    public class GetAllMoneyTransferRequest : ScrollPaginationRequest, IRequest<GetAllMoneyTransferResponse>
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

    public class GetAllMoneyTransferResponse : PaginationResponse<GetAllMoneyTransferResponseItem>
    {
    }

    public class GetAllMoneyTransferResponseItem : GetDetailMoneyTransferResponse
    {
    }
}