using Mapster;

using Zenit.Management.Business.Managers.GoalManager;
using Zenit.Management.Contract.Requests.GoalRequests;
using Zenit.Management.Data.Entities;
using Zenit.Share.Contract.Models;

namespace Zenit.Management.Business.Services.GoalServices
{
    public class GoalService(IServiceProvider serviceProvider) : ManagementApplicationService(serviceProvider)
    {
        private GoalManager _GoalManager => GetService<GoalManager>();

        public async Task<CreateGoalResponse> Create(CreateGoalRequest request)
        {
            var goal = Mapper.Map<Goal>(request);
            goal.Id = Guid.NewGuid();
            goal.AccountId = CurrentAccount.Id;

            _GoalManager.Add(goal);
            await UnitOfWork.SaveChangesAsync();

            return Mapper.Map<CreateGoalResponse>(goal);
        }

        public Task<GetAllGoalResponse> GetAll(GetAllGoalRequest request)
        {
            var goalQuery = _GoalManager.GetAll().Where(g => g.AccountId == CurrentAccount.Id && g.IsDeleted == false);

            return Task.FromResult(Mapper.Map<GetAllGoalResponse>(
                PaginationResponse<Goal>.Create(goalQuery, request)
            ));
        }

        public Task<GetDetailGoalResponse> GetDetail(GetDetailGoalRequest request)
        {
            var goal = _GoalManager.FindBy(g => g.Id == request.Id).FirstOrDefault();
            if (goal == null)
            {
                throw new Exception("Goal not found");
            }
            return Task.FromResult(Mapper.Map<GetDetailGoalResponse>(goal));
        }

        public async Task Delete(DeleteGoalRequest request)
        {
            var goal = _GoalManager.FindBy(g => g.Id == request.Id).FirstOrDefault();
            if (goal == null)
            {
                throw new Exception("Goal not found");
            }

            _GoalManager.Delete(goal);
            await UnitOfWork.SaveChangesAsync();
        }

        public async Task<UpdateGoalResponse> Update(UpdateGoalRequest request)
        {
            var goal = _GoalManager.FindBy(g => g.Id == request.Id).FirstOrDefault();
            if (goal == null)
            {
                throw new Exception("Goal not found");
            }

            request.Adapt(goal);
            _GoalManager.Update(goal);
            await UnitOfWork.SaveChangesAsync();

            return Mapper.Map<UpdateGoalResponse>(goal);
        }
    }
}