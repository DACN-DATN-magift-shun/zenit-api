using MediatR;

using Zenit.Management.Business.Services.LoanServices;
using Zenit.Management.Contract.Requests.LoanRequests;

namespace Zenit.Management.Host.RequestHandlers
{
    public class CreateManyLoanHandler(LoanService loanService) : IRequestHandler<CreateManyLoanRequest, CreateManyLoanResponse>
    {
        public async Task<CreateManyLoanResponse> Handle(CreateManyLoanRequest request, CancellationToken cancellationToken)
        {
            return await loanService.CreateMany(request);
        }
    }
}