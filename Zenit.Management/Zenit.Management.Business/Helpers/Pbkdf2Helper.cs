using System.Security.Cryptography;

using Microsoft.AspNetCore.Cryptography.KeyDerivation;


namespace Zenit.Management.Business.Helpers
{
    public class Pbkdf2Helpers
    {
        public static string HashPassword(string password, byte[] salt)
        {
            byte[] hashedPassword = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 32
            );

            byte[] finalPasswordBytes = new byte[48];
            Array.Copy(salt, 0, finalPasswordBytes, 0, 16);
            Array.Copy(hashedPassword, 0, finalPasswordBytes, 16, 32);

            return Convert.ToBase64String(finalPasswordBytes);
        }

        public static byte[] GenerateSalt()
        {
            byte[] salt = new byte[16];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            return salt;
        }
    }
}
