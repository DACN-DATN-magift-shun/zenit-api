using Microsoft.AspNetCore.Http;

using Zenit.Share.Common.Interfaces;


namespace Zenit.Share.Host.Interfaces
{
    public interface ICurrentAccountMiddleware
    {
        
    }

    public interface ICurrentAccountMiddleware<T, TID> : ICurrentAccountMiddleware
        where T : ICurrentAccount<TID>
    {
        Task InvokeAsync(HttpContext context, T currentAccount);
    }
}
