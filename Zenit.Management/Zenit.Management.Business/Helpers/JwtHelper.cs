using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

using Zenit.Management.Data.Entities;
using Zenit.Management.Data.Models;
using Zenit.Share.Common.Constants;


namespace Zenit.Management.Business.Helpers
{
    public class JwtHelpers
    {
        public static AccountTokenModel GenerateJwtTokens(Account account)
        {
            try
            {
                var accessToken = GenerateAccessToken(account);
                var refreshToken = GenerateRefreshToken();

                return new AccountTokenModel
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                };
            }
            catch
            {
                throw;
            }
        }

        public static string GenerateAccessToken(Account account)
        {
            var claims = new List<Claim> {
                new(ClaimTypes.NameIdentifier, account.Id.ToString()),
                new(ClaimTypes.Name, account.Username),
                new(ClaimTypes.Email, account.Email),
                new(ClaimTypes.MobilePhone, account.Phone ?? string.Empty),
                new(ClaimTypes.StreetAddress, account.Address ?? string.Empty)
            };

            var jwtSecret = Environment.GetEnvironmentVariable(EnvConstants.JWT_SECRET);
            if (string.IsNullOrEmpty(jwtSecret))
            {
                throw new Exception("JWT secret is not set.");
            }

            var jwtToken = new JwtSecurityToken(
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSecret)
                    ),
                    SecurityAlgorithms.HmacSha256Signature)
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }

        public static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
            }

            return Convert.ToBase64String(randomNumber);
        }
    }
}
