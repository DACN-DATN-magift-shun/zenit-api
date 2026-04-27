using Microsoft.AspNetCore.Http;

namespace Zenit.Share.Business.Interfaces
{
    public interface ISseService
    {
        void AddConnection(Guid accountId, HttpResponse response);
        void RemoveConnection(Guid accountId);
        Task SendAsync(Guid accountId, object data);
    }

}