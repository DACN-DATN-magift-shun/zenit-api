using MediatR;

using Zenit.Management.Business.Services.MessageServices;
using Zenit.Management.Common.Constants;
using Zenit.Management.Data.Entities;
using Zenit.Share.Common.Values;
using Zenit.Share.Data.Events.Handlers;

namespace Zenit.Management.Host.EventHandlers
{
    public class CreateMessageEventHandler(MessageService messageService) : EventCreateHandler<Message>
    {
        public override async Task<object> Handle(Message data, List<AuditDataChange>? dataChanges)
        {
            if (data.AccountId != ChatbotConstants.ID)
            {
                return await messageService.HandleCreateMessageAsync(data, dataChanges);
            }
            return null;
        }
    }
}