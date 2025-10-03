using Microsoft.AspNetCore.Http;

using Share.Common.Interfaces;


namespace Share.Host.Interfaces
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