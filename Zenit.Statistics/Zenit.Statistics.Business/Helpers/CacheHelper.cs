using System.Security.Cryptography;
using System.Text;

namespace Zenit.Statistics.Business.Helpers
{
    public static class CacheHelper
    {
        public static string GetMD5Hash(string cachedKey)
        {
            using var md5 = MD5.Create();
            var result = md5.ComputeHash(Encoding.ASCII.GetBytes(cachedKey));
            return Encoding.ASCII.GetString(result);
        }
    }
}