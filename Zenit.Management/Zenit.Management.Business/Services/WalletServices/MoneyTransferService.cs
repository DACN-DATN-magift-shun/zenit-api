using Mapster;

using Microsoft.Extensions.DependencyInjection;

using Zenit.Management.Business.Managers.WalletManager;
using Zenit.Management.Contract.Request.MoneyTransferRequests;
using Zenit.Management.Data.Entities;
using Zenit.Share.Common.Values;
using Zenit.Share.Contract.Models;

namespace Zenit.Management.Business.Services.WalletServices
{
    public class MoneyTransferService(IServiceProvider serviceProvider) : ManagementApplicationService(serviceProvider)
    {
        private MoneyTransferManager _MoneyTransferManager => GetService<MoneyTransferManager>();

        public async Task<CreateMoneyTransferResponse> Create(CreateMoneyTransferRequest request)
        {
            var moneyTransfer = Mapper.Map<MoneyTransferHistory>(request);
            moneyTransfer.Id = Guid.NewGuid();
            moneyTransfer.AccountId = CurrentAccount.Id;
            _MoneyTransferManager.Add(moneyTransfer);
            await UnitOfWork.SaveChangesAsync();
            return Mapper.Map<CreateMoneyTransferResponse>(moneyTransfer);
        }

        public Task<GetAllMoneyTransferResponse> GetAll(GetAllMoneyTransferRequest request)
        {
            var moneyTransfersQuery = _MoneyTransferManager.GetAll()
                .Where(mt => mt.IsDeleted == false && mt.AccountId == CurrentAccount.Id);
            
            var fromDate = request.FromDate;
            var toDate = request.ToDate;

            if (fromDate.HasValue)
            {
                moneyTransfersQuery = moneyTransfersQuery.Where(mt => mt.TransferDate >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                moneyTransfersQuery = moneyTransfersQuery.Where(mt => mt.TransferDate <= toDate.Value);
            }

            return Task.FromResult(Mapper.Map<GetAllMoneyTransferResponse>(
                PaginationResponse<MoneyTransferHistory>.Create(moneyTransfersQuery, request)
            ));
        }

        public Task<GetDetailMoneyTransferResponse> GetDetail(GetDetailMoneyTransferRequest request)
        {
            var moneyTransfer = _MoneyTransferManager.FindBy(mt => mt.Id == request.Id && mt.IsDeleted == false).FirstOrDefault();
            return Task.FromResult(Mapper.Map<GetDetailMoneyTransferResponse>(moneyTransfer));
        }
        public async Task<UpdateMoneyTransferResponse> Update(UpdateMoneyTransferRequest request)
        {
            var moneyTransfer = _MoneyTransferManager.FindBy(mt => mt.Id == request.Id && mt.IsDeleted == false).FirstOrDefault();

            if (moneyTransfer == null)
            {
                throw new Exception("Money transfer not found");
            }

            request.Adapt(moneyTransfer);
            await UnitOfWork.SaveChangesAsync();
            return Mapper.Map<UpdateMoneyTransferResponse>(moneyTransfer);
        }

        public async Task Delete(DeleteMoneyTransferRequest request)
        {
            var moneyTransfer = _MoneyTransferManager.FindBy(mt => mt.Id == request.Id).FirstOrDefault();
            _MoneyTransferManager.Delete(moneyTransfer!);
            await UnitOfWork.SaveChangesAsync();
        }

        public async Task HandleCreateWalletAsync(Wallet wallet, List<AuditDataChange> dataChanges)
        {
            var transferHistory = new MoneyTransferHistory
            {
                Id = Guid.NewGuid(),
                AccountId = CurrentAccount.Id,
                Amount = wallet.Amount,
                FromWalletId = null,
                ToWalletId = wallet.Id,
                TransferDate = DateTime.UtcNow,
                Note = "Initial balance",
            };

            _MoneyTransferManager.Add(transferHistory);
            await UnitOfWork.SaveChangesAsync();
        }

        public async Task HandleUpdateWalletAsync(Wallet wallet, List<AuditDataChange> dataChanges)
        {
            var oldAmount = dataChanges.FirstOrDefault(dc => dc.Field == nameof(Wallet.Amount))?.OriginalValue as int? ?? wallet.Amount;
            var newAmount = dataChanges.FirstOrDefault(dc => dc.Field == nameof(Wallet.Amount))?.NewValue as int? ?? wallet.Amount;
            var amountChange = newAmount - oldAmount;

            if (amountChange == 0)
            {
                return; // No change in amount, no need to create transfer history
            }

            var transferHistory = new MoneyTransferHistory
            {
                Id = Guid.NewGuid(),
                AccountId = CurrentAccount.Id,
                Amount = amountChange,
                FromWalletId = null,
                ToWalletId = wallet.Id,
                TransferDate = DateTime.UtcNow,
                Note = "Balance adjustment",
            };

            _MoneyTransferManager.Add(transferHistory);
            await UnitOfWork.SaveChangesAsync();
        }
    }
}