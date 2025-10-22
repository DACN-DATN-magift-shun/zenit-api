using System.Security.Claims;

using Microsoft.AspNetCore.Http;

using MongoDB.Bson;

using Zenit.Accounts.Common.Models;
using Zenit.Share.Host.Middlewares;


namespace Zenit.Accounts.Business.Middlewares
{
    public class CurrentAccountMiddleware(RequestDelegate next)
        : CurrentAccountMiddlewareBase<CurrentAccount>(next)
    {
        public override async Task InvokeAsync(HttpContext context, CurrentAccount currentAccount)
        {
            var user = context.User;

            bool authorized = user?.Identity?.IsAuthenticated ?? false;

            if (authorized)
            {
                var claimsPrincipal = context.User;

                var identifier = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var username = claimsPrincipal.FindFirst(ClaimTypes.Name)?.Value;
                var email = claimsPrincipal.FindFirst(ClaimTypes.Email)?.Value;
                var phone = claimsPrincipal.FindFirst(ClaimTypes.MobilePhone)?.Value;
                var address = claimsPrincipal.FindFirst(ClaimTypes.StreetAddress)?.Value;

                currentAccount.Id = Guid.TryParse(identifier, out var id) ? id : Guid.Empty;

                currentAccount.Username = username ?? string.Empty;
                currentAccount.Email = email ?? string.Empty;
                currentAccount.Phone = phone;
                currentAccount.Address = address;
            }

            await _next(context);
        }
    }
}
