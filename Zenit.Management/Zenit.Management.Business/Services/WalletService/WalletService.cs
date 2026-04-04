using Mapster;

using Microsoft.Extensions.DependencyInjection;

using Zenit.Management.Business.Managers;
using Zenit.Management.Contract.Request.WalletRequests;
using Zenit.Management.Data;
using Zenit.Management.Data.Entities;
using Zenit.Share.Common.Values;
using Zenit.Share.Contract.Models;

namespace Zenit.Management.Business.Services
{
    public class WalletService(IServiceProvider serviceProvider) : ManagementApplicationService(serviceProvider)
    {
        private WalletManager _WalletManager => GetService<WalletManager>();
        private ManagementDbContext _DbContext => GetService<ManagementDbContext>();

        public async Task<CreateWalletResponse> Create(CreateWalletRequest request)
        {
            var wallet = Mapper.Map<Wallet>(request);
            wallet.Id = Guid.NewGuid();
            wallet.AccountId = CurrentAccount.Id;
            _WalletManager.Add(wallet);
            await UnitOfWork.SaveChangesAsync();
            return Mapper.Map<CreateWalletResponse>(wallet);
        }

        public Task<GetAllWalletResponse> GetAll(GetAllWalletRequest request)
        {
            var walletsQuery = _WalletManager.GetAll()
                .Where(w => w.IsDeleted == false && w.AccountId == CurrentAccount.Id);
            
            if (!string.IsNullOrEmpty(request.Search))
            {
                walletsQuery = walletsQuery.Where(w => w.Name.Contains(request.Search));
            }

            return Task.FromResult(Mapper.Map<GetAllWalletResponse>(
                PaginationResponse<Wallet>.Create(walletsQuery, request)
            ));
        }

        public Task<GetDetailWalletResponse> GetDetail(GetDetailWalletRequest request)
        {
            var wallet = _WalletManager.FindBy(w => w.Id == request.Id && w.IsDeleted == false).FirstOrDefault();

            if (wallet == null)
            {
                throw new Exception("Wallet not found");
            }
            return Task.FromResult(Mapper.Map<GetDetailWalletResponse>(wallet));

        }

        public async Task<UpdateWalletResponse> Update(UpdateWalletRequest request)
        {
            var wallet = _WalletManager.FindBy(w => w.Id == request.Id && w.IsDeleted == false).FirstOrDefault();
            request.Adapt(wallet);
            await UnitOfWork.SaveChangesAsync();
            return Mapper.Map<UpdateWalletResponse>(wallet);
        }

        public async Task Delete(DeleteWalletRequest request)
        {
            var wallet = _WalletManager.FindBy(w => w.Id == request.Id).FirstOrDefault();
            _WalletManager.Delete(wallet!);
            await UnitOfWork.SaveChangesAsync();
        }

        public async Task HandleCreateMoneyTransferHistoryAsync(MoneyTransferHistory moneyTransferHistory, List<AuditDataChange> dataChanges)
        {
            var fromWallet = _WalletManager.FindBy(w => w.Id == moneyTransferHistory.FromWalletId).FirstOrDefault();
            var toWallet = _WalletManager.FindBy(w => w.Id == moneyTransferHistory.ToWalletId).FirstOrDefault();

            fromWallet.Amount -= moneyTransferHistory.Amount;
            toWallet.Amount += moneyTransferHistory.Amount;

            await _DbContext.SaveChangesAsync();
        }

        public async Task HandleUpdateMoneyTransferHistoryAsync(MoneyTransferHistory moneyTransferHistory, List<AuditDataChange> dataChanges)
        {
            var oldFromWalletId = dataChanges?.FirstOrDefault(dc => dc.Field == nameof(MoneyTransferHistory.FromWalletId))?.OriginalValue as Guid?;
            var oldFromWallet = oldFromWalletId.HasValue ? _WalletManager.FindBy(w => w.Id == oldFromWalletId.Value).FirstOrDefault() : moneyTransferHistory.FromWallet;
            var oldToWalletId = dataChanges?.FirstOrDefault(dc => dc.Field == nameof(MoneyTransferHistory.ToWalletId))?.OriginalValue as Guid?;
            var oldToWallet = oldToWalletId.HasValue ? _WalletManager.FindBy(w => w.Id == oldToWalletId.Value).FirstOrDefault() : moneyTransferHistory.ToWallet;

            var newFromWalletId = moneyTransferHistory.FromWalletId;
            var newFromWallet = _WalletManager.FindBy(w => w.Id == newFromWalletId).FirstOrDefault();
            var newToWalletId = moneyTransferHistory.ToWalletId;
            var newToWallet = _WalletManager.FindBy(w => w.Id == newToWalletId).FirstOrDefault();

            var oldAmount = dataChanges?.FirstOrDefault(dc => dc.Field == nameof(MoneyTransferHistory.Amount))?.OriginalValue as int? ?? moneyTransferHistory.Amount;

            oldFromWallet.Amount += oldAmount;
            oldToWallet.Amount -= oldAmount;

            newFromWallet.Amount -= moneyTransferHistory.Amount;
            newToWallet.Amount += moneyTransferHistory.Amount;

            await _DbContext.SaveChangesAsync();
        }

        public async Task HandleDeleteMoneyTransferHistoryAsync(MoneyTransferHistory moneyTransferHistory, List<AuditDataChange> dataChanges)
        {
            var fromWallet = _WalletManager.FindBy(w => w.Id == moneyTransferHistory.FromWalletId).FirstOrDefault();
            var toWallet = _WalletManager.FindBy(w => w.Id == moneyTransferHistory.ToWalletId).FirstOrDefault();

            if (fromWallet != null)
            {
                fromWallet.Amount += moneyTransferHistory.Amount;
            }

            toWallet.Amount -= moneyTransferHistory.Amount;

            await _DbContext.SaveChangesAsync();
        }
    }
}