using System.Collections.Concurrent;
using System.Text.Encodings.Web;
using System.Text.Json;

using Microsoft.AspNetCore.Http;

using Zenit.Share.Business.Interfaces;

namespace Zenit.Share.Business
{
    public abstract class SseServiceBase : ISseService
    {
        private readonly ConcurrentDictionary<Guid, HttpResponse> _connections = new();

        public void AddConnection(Guid accountId, HttpResponse response) =>
            _connections[accountId] = response;

        public void RemoveConnection(Guid accountId) =>
            _connections.TryRemove(accountId, out _);

        public async Task SendAsync(Guid accountId, object data)
        {
            if (!_connections.TryGetValue(accountId, out var response)) return;

            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });

            await response.WriteAsync($"data: {json}\n\n");
            await response.Body.FlushAsync();
        }
    }
}