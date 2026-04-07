using MediatR;

using Zenit.Management.Business.Services.LoanServices;
using Zenit.Management.Contract.Requests.LoanRequests;

namespace Zenit.Management.Host.RequestHandlers
{
    public class UpdateLoanHandler(LoanService loanService) : IRequestHandler<UpdateLoanRequest, UpdateLoanResponse>
    {
        public async Task<UpdateLoanResponse> Handle(UpdateLoanRequest request, CancellationToken cancellationToken)
        {
            return await loanService.Update(request);
        }
    }
}