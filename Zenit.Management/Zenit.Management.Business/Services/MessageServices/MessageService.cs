using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Unicode;

using Mapster;

using Microsoft.OpenApi.Any;

using Zenit.Management.Business.Managers.MessageManager;
using Zenit.Management.Common.Constants;
using Zenit.Management.Contract.Requests.MessageRequests;
using Zenit.Management.Data.Entities;
using Zenit.Share.Common.Constants;
using Zenit.Share.Common.Values;
using Zenit.Share.Contract.Models;

namespace Zenit.Management.Business.Services.MessageServices
{
    public class MessageService(IServiceProvider serviceProvider) : ManagementApplicationService(serviceProvider)
    {
        private MessageManager _MessageManager => GetService<MessageManager>();

        public async Task<CreateMessageResponse> Create(CreateMessageRequest request)
        {
            var message = Mapper.Map<Message>(request);
            message.Id = Guid.CreateVersion7();
            message.AccountId = CurrentAccount.Id;

            _MessageManager.Add(message);
            await UnitOfWork.SaveChangesAsync();

            return Mapper.Map<CreateMessageResponse>(message);
        }

        public Task<GetAllMessageResponse> GetAll(GetAllMessageRequest request)
        {
            var messageQuery = _MessageManager.GetAll().Where(m =>
                m.ConversationId == request.ConversationId &&
                (m.AccountId == CurrentAccount.Id || m.AccountId == ChatbotConstants.ID) &&
                m.IsDeleted == false
            );

            return Task.FromResult(Mapper.Map<GetAllMessageResponse>(
                PaginationResponse<Message>.Create(messageQuery, request)
            ));
        }

        public Task<GetDetailMessageResponse> GetDetail(GetDetailMessageRequest request)
        {
            var message = _MessageManager.FindBy(m => m.Id == request.Id).FirstOrDefault();
            if (message == null)
            {
                throw new Exception("Message not found");
            }
            return Task.FromResult(Mapper.Map<GetDetailMessageResponse>(message));
        }

        public async Task Delete(DeleteMessageRequest request)
        {
            var message = _MessageManager.FindBy(m => m.Id == request.Id).FirstOrDefault();
            if (message == null)
            {
                throw new Exception("Message not found");
            }

            _MessageManager.Delete(message);
            await UnitOfWork.SaveChangesAsync();
        }

        public async Task<UpdateMessageResponse> Update(UpdateMessageRequest request)
        {
            var message = _MessageManager.FindBy(m => m.Id == request.Id).FirstOrDefault();
            if (message == null)
            {
                throw new Exception("Message not found");
            }

            request.Adapt(message);
            _MessageManager.Update(message);
            await UnitOfWork.SaveChangesAsync();

            return Mapper.Map<UpdateMessageResponse>(message);
        }

        public async Task<object> HandleCreateMessageAsync(Message message, List<AuditDataChange>? dataChanges)
        {
            var chatbotBaseUrl = Environment.GetEnvironmentVariable(EnvConstants.CHATBOT_BASE_URL);

            using var httpClient = new HttpClient();
            var payload = new
            {
                message = message.Text,
                conversation_id = message.ConversationId.ToString(),
                account_id = CurrentAccount.Id.ToString(),
            };

            var jsonOptions = new JsonSerializerOptions
            {
                // Cho phép tất cả các ký tự Unicode (Bao gồm tiếng Việt)
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, 
                WriteIndented = true 
            };

            var jsonPayload = new StringContent(
                JsonSerializer.Serialize(payload, jsonOptions),
                Encoding.UTF8,
                "application/json"
            );

            try
            {
                var response = await httpClient.PostAsync(
                    $"{chatbotBaseUrl}/messages",
                    jsonPayload
                );

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    
                    // Parse nội dung trả về thành một JsonNode
                    var chatbotResponse = JsonNode.Parse(content);

                    // In ra console với định dạng dễ nhìn (Pretty print)
                    var prettyJson = chatbotResponse?.ToJsonString(jsonOptions);
                    Console.WriteLine($"Chatbot response:\n{prettyJson}\n================================");

                    var chatbotMessage = new Message
                    {
                        Id = Guid.CreateVersion7(),
                        ConversationId = message.ConversationId,
                        AccountId = ChatbotConstants.ID,
                        Text = chatbotResponse["response"]?.ToString() ?? string.Empty,
                    };

                    _MessageManager.Add(chatbotMessage);
                    await UnitOfWork.SaveChangesAsync();

                    var suggestionsArray = chatbotResponse["suggestions"];
                    Console.WriteLine($"Chatbot suggestions:\n{suggestionsArray?.ToJsonString(jsonOptions)}\n================================");

                    if (suggestionsArray != null)
                    {
                        return suggestionsArray.AsArray()
                            .Select(item => new Dictionary<string, object>
                            {
                                { "category", item["category"]?.ToString() ?? string.Empty },
                                { "wallet", item["wallet"]?.ToString() ?? string.Empty }
                            })
                            .ToList();
                    }
                }
                return new List<Dictionary<string, object>>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to send message to chatbot: {ex.Message}");
            }
        }
    }
}