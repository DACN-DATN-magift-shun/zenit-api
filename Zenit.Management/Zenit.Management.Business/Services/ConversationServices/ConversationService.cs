using Mapster;

using Zenit.Management.Business.Managers.ConversationManager;
using Zenit.Management.Contract.Requests.ConversationRequests;
using Zenit.Management.Data.Entities;
using Zenit.Share.Contract.Models;

namespace Zenit.Management.Business.Services.ConversationServices
{
    public class ConversationService(IServiceProvider serviceProvider) : ManagementApplicationService(serviceProvider)
    {
        private ConversationManager _ConversationManager => GetService<ConversationManager>();

        public async Task<CreateConversationResponse> Create(CreateConversationRequest request)
        {
            var conversation = Mapper.Map<Conversation>(request);
            conversation.Id = Guid.NewGuid();
            conversation.AccountId = CurrentAccount.Id;

            _ConversationManager.Add(conversation);
            await UnitOfWork.SaveChangesAsync();

            return Mapper.Map<CreateConversationResponse>(conversation);
        }

        public Task<GetAllConversationResponse> GetAll(GetAllConversationRequest request)
        {
            var conversationQuery = _ConversationManager.GetAll().Where(c => c.AccountId == CurrentAccount.Id && c.IsDeleted == false);

            return Task.FromResult(Mapper.Map<GetAllConversationResponse>(
                PaginationResponse<Conversation>.Create(conversationQuery, request)
            ));
        }

        public Task<GetDetailConversationResponse> GetDetail(GetDetailConversationRequest request)
        {
            var conversation = _ConversationManager.FindBy(c => c.Id == request.Id).FirstOrDefault();
            if (conversation == null)
            {
                throw new Exception("Conversation not found");
            }
            return Task.FromResult(Mapper.Map<GetDetailConversationResponse>(conversation));
        }

        public async Task Delete(DeleteConversationRequest request)
        {
            var conversation = _ConversationManager.FindBy(c => c.Id == request.Id).FirstOrDefault();
            if (conversation == null)
            {
                throw new Exception("Conversation not found");
            }

            _ConversationManager.Delete(conversation);
            await UnitOfWork.SaveChangesAsync();
        }

        public async Task<UpdateConversationResponse> Update(UpdateConversationRequest request)
        {
            var conversation = _ConversationManager.FindBy(c => c.Id == request.Id).FirstOrDefault();
            if (conversation == null)
            {
                throw new Exception("Conversation not found");
            }

            request.Adapt(conversation);
            _ConversationManager.Update(conversation);
            await UnitOfWork.SaveChangesAsync();

            return Mapper.Map<UpdateConversationResponse>(conversation);
        }
    }
}