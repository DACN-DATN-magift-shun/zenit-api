using Mapster;

using Zenit.Management.Business.Managers.LoanManager;
using Zenit.Management.Common.Enums;
using Zenit.Management.Contract.Requests.LoanRequests;
using Zenit.Management.Data.Entities;
using Zenit.Share.Contract.Models;

namespace Zenit.Management.Business.Services.LoanServices
{
    public class LoanService(IServiceProvider serviceProvider) : ManagementApplicationService(serviceProvider)
    {
        private LoanManager _LoanManager => GetService<LoanManager>();

        public async Task<CreateLoanResponse> Create(CreateLoanRequest request)
        {
            var loan = Mapper.Map<Loan>(request);
            loan.Id = Guid.NewGuid();
            loan.AccountId = CurrentAccount.Id;
            loan.Status = LoanStatus.Ongoing;

            _LoanManager.Add(loan);
            await UnitOfWork.SaveChangesAsync();

            return Mapper.Map<CreateLoanResponse>(loan);
        }

        public async Task<CreateManyLoanResponse> CreateMany(CreateManyLoanRequest request)
        {
            var loans = Mapper.Map<List<Loan>>(request.Loans);
            loans.ForEach(loan =>
            {
                loan.Id = Guid.NewGuid();
                loan.AccountId = CurrentAccount.Id;
                loan.Status = LoanStatus.Ongoing;
            });

            _LoanManager.AddRange(loans);
            await UnitOfWork.SaveChangesAsync();

            return new CreateManyLoanResponse
            {
                Loans = Mapper.Map<List<CreateLoanResponse>>(loans)
            };
        }

        public async Task Delete(DeleteLoanRequest request)
        {
            var loan = _LoanManager.FindBy(l => l.Id == request.Id && l.IsDeleted == false).FirstOrDefault();
            if (loan == null)
            {
                throw new Exception("Loan not found");
            }

            _LoanManager.Delete(loan);
            await UnitOfWork.SaveChangesAsync();
        }

        public async Task<UpdateLoanResponse> Update(UpdateLoanRequest request)
        {
            var loan = _LoanManager.FindBy(l => l.Id == request.Id && l.IsDeleted == false).FirstOrDefault();
            if (loan == null)
            {
                throw new Exception("Loan not found");
            }

            request.Adapt(loan);
            await UnitOfWork.SaveChangesAsync();

            return Mapper.Map<UpdateLoanResponse>(loan);
        }

        public Task<GetAllLoanResponse> GetAll(GetAllLoanRequest request)
        {
            var loanQuery = _LoanManager.GetAll()
                .Where(l => l.AccountId == CurrentAccount.Id && l.IsDeleted == false);
            
            if (request.Type.HasValue)
            {
                loanQuery = loanQuery.Where(l => l.Type == request.Type);
            }

            return Task.FromResult(Mapper.Map<GetAllLoanResponse>(
                PaginationResponse<Loan>.Create(loanQuery, request)
            ));
        }

        public Task<GetDetailLoanResponse> GetDetail(GetDetailLoanRequest request)
        {
            var loan = _LoanManager.FindBy(l => l.Id == request.Id && l.IsDeleted == false).FirstOrDefault();
            if (loan == null)
            {
                throw new Exception("Loan not found");
            }
            return Task.FromResult(Mapper.Map<GetDetailLoanResponse>(loan));
        }
    }
}