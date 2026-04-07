using MediatR;

using Zenit.Management.Business.Services.LoanServices;
using Zenit.Management.Contract.Requests.LoanRequests;

namespace Zenit.Management.Host.RequestHandlers
{
    public class GetAllLoanHandler(LoanService loanService) : IRequestHandler<GetAllLoanRequest, GetAllLoanResponse>
    {
        public async Task<GetAllLoanResponse> Handle(GetAllLoanRequest request, CancellationToken cancellationToken)
        {
            return await loanService.GetAll(request);
        }
    }
}