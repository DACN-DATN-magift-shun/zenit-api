using Microsoft.AspNetCore.Http;

using Share.Common.Interfaces;
using Share.Host.Interfaces;


namespace Share.Host.Middlewares
{
    public abstract class CurrentAccountMiddlewareBase<T, TID>(RequestDelegate next) : ICurrentAccountMiddleware
        where T : ICurrentAccount<TID>
    {
        protected readonly RequestDelegate _next = next;

        public abstract Task InvokeAsync(HttpContext context, T currentAccount);
    }
}