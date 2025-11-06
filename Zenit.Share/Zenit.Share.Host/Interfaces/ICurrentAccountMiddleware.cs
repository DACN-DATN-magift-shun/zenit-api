using Microsoft.AspNetCore.Http;

using Zenit.Share.Common.Interfaces;


namespace Zenit.Share.Host.Interfaces
{
    public interface ICurrentAccountMiddleware
    {

    }

    public interface ICurrentAccountMiddleware<T> : ICurrentAccountMiddleware
        where T : ICurrentAccount
    {
        Task InvokeAsync(HttpContext context, T currentAccount);
    }
}
