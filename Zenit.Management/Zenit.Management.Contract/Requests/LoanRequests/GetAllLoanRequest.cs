using MediatR;

using Zenit.Management.Common.Enums;
using Zenit.Share.Contract.Models;

namespace Zenit.Management.Contract.Requests.LoanRequests
{
    public class GetAllLoanRequest : ScrollPaginationRequest, IRequest<GetAllLoanResponse>
    {
        public LoanType? Type { get; set; }
    }

    public class GetAllLoanResponse : PaginationResponse<GetAllLoanResponseItem>
    {
    }

    public class GetAllLoanResponseItem : GetDetailLoanResponse
    {
    }
}