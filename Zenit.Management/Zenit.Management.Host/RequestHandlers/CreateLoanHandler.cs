using MediatR;

using Zenit.Management.Business.Services.LoanServices;
using Zenit.Management.Contract.Requests.LoanRequests;

namespace Zenit.Management.Host.RequestHandlers
{
    public class CreateLoanHandler(LoanService loanService) : IRequestHandler<CreateLoanRequest, CreateLoanResponse>
    {
        public async Task<CreateLoanResponse> Handle(CreateLoanRequest request, CancellationToken cancellationToken)
        {
            return await loanService.Create(request);
        }
    }
}