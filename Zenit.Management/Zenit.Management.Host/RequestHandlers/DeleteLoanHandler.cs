using MediatR;

using Zenit.Management.Business.Services.LoanServices;
using Zenit.Management.Contract.Requests.LoanRequests;

namespace Zenit.Management.Host.RequestHandlers
{
    public class DeleteLoanHandler(LoanService loanService) : IRequestHandler<DeleteLoanRequest>
    {
        public async Task Handle(DeleteLoanRequest request, CancellationToken cancellationToken)
        {
            await loanService.Delete(request);
        }
    }
}