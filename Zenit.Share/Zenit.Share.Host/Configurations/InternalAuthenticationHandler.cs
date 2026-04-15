using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Zenit.Share.Host.Configurations
{
    public class InternalAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        IConfiguration configuration
    ) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
    {
        private readonly IConfiguration _configuration = configuration;

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue("Authorization", out var header))
                return Task.FromResult(AuthenticateResult.NoResult());

            // Chỉ xử lý Basic, còn Bearer thì nhường cho JwtBearer handler
            if (!header.ToString().StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
                return Task.FromResult(AuthenticateResult.NoResult());
            try
            {
                var internalAuthUserName = _configuration["InternalAuthentication:Username"];
                var internalAuthPassword = _configuration["InternalAuthentication:Password"];

                var authorizationHeader = Request.Headers.Authorization;
                var authHeader = AuthenticationHeaderValue.Parse(authorizationHeader!);
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter!);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split([':'], 3);
                var username = credentials[0];
                var password = credentials[1];
                var accountId = credentials[2];

                if (username != internalAuthUserName || password != internalAuthPassword)
                {
                    return Task.FromResult(AuthenticateResult.Fail("Invalid Username or Password"));
                }

                var authenticationTicket = new AuthenticationTicket(
                    new ClaimsPrincipal(
                        new ClaimsIdentity(
                            new[] { 
                                new Claim(ClaimTypes.Name, username),
                                new Claim(ClaimTypes.NameIdentifier, accountId)
                            },
                            Scheme.Name
                        )
                    ),
                    Scheme.Name
                );

                return Task.FromResult(AuthenticateResult.Success(authenticationTicket));
            }
            catch
            {
                return Task.FromResult(AuthenticateResult.Fail("Error Occurred. Authorization failed."));
            }
        }
    }
}