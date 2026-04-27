using MediatR;

using Zenit.Management.Business;
using Zenit.Management.Common.Models;
using Zenit.Management.Contract.Requests.MessageRequests;
using Zenit.Share.Host.Middlewares;

namespace Zenit.Management.Host.RequestHandlers
{
    public class StreamMessageHandler(
        ManagementSseService sseService,
        IHttpContextAccessor httpContextAccessor,
        ManagementCurrentAccount currentAccount  
    ) : IRequestHandler<StreamMessageRequest, StreamMessageResponse>
    {
        public async Task<StreamMessageResponse> Handle(StreamMessageRequest request, CancellationToken cancellationToken)
        {
            var response = httpContextAccessor.HttpContext.Response;
            response.ContentType = "text/event-stream";
            response.Headers["Cache-Control"] = "no-cache";
            response.Headers["X-Accel-Buffering"] = "no";

            // Send initial ping so client knows connection is alive
            await response.WriteAsync("data: {\"type\":\"connected\"}\n\n", cancellationToken);
            await response.Body.FlushAsync(cancellationToken);

            sseService.AddConnection(currentAccount.Id, response);

            try
            {
                // Keep-alive ping every 15 seconds so connection doesn't time out
                while (!cancellationToken.IsCancellationRequested)
                {
                    await Task.Delay(TimeSpan.FromSeconds(15), cancellationToken);

                    await response.WriteAsync(": ping\n\n", cancellationToken);
                    await response.Body.FlushAsync(cancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
                // Client disconnected — clean exit
            }
            finally
            {
                sseService.RemoveConnection(currentAccount.Id);
                await response.CompleteAsync(); // ← This is what was missing
            }

            return new StreamMessageResponse { ConversationId = request.ConversationId };
        }
    }
}