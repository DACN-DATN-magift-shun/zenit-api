using Microsoft.AspNetCore.Http;

using Zenit.Share.Common.Interfaces;
using Zenit.Share.Host.Interfaces;


namespace Zenit.Share.Host.Middlewares
{
    public abstract class CurrentAccountMiddlewareBase<T>(RequestDelegate next) : ICurrentAccountMiddleware
        where T : ICurrentAccount
    {
        protected readonly RequestDelegate _next = next;

        public abstract Task InvokeAsync(HttpContext context, T currentAccount);
    }
}
