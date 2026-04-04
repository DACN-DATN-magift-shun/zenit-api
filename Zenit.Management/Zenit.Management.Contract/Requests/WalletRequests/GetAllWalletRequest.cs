using MediatR;

using Zenit.Management.Data.Entities;
using Zenit.Share.Contract.Models;

namespace Zenit.Management.Contract.Request.WalletRequests
{
    public class GetAllWalletRequest : ScrollPaginationRequest, IRequest<GetAllWalletResponse>
    {
    }

    public class GetAllWalletResponse : PaginationResponse<PaginationGetAllResponseItem>
    {    
    }

    public class PaginationGetAllResponseItem : GetDetailWalletResponse
    {
    }
}