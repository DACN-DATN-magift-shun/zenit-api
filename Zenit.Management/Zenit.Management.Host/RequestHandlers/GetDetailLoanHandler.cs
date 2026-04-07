using MediatR;

using Zenit.Management.Business.Services.LoanServices;
using Zenit.Management.Contract.Requests.LoanRequests;

namespace Zenit.Management.Host.RequestHandlers
{
    public class GetDetailLoanHandler(LoanService loanService) : IRequestHandler<GetDetailLoanRequest, GetDetailLoanResponse>
    {
        public async Task<GetDetailLoanResponse> Handle(GetDetailLoanRequest request, CancellationToken cancellationToken)
        {
            return await loanService.GetDetail(request);
        }
    }
}