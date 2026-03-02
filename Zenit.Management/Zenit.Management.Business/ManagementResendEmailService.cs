using Resend;
using Microsoft.Extensions.Options;
using Zenit.Share.Business;
using Zenit.Share.Common.Constants;

namespace Zenit.Management.Business
{
    public class ManagementResendEmailService : ResendEmailServiceBase
    {
        public ManagementResendEmailService()
        {
            var apiKey = Environment.GetEnvironmentVariable(EnvConstants.RESEND_API_KEY)
                ?? throw new Exception("Resend API Key is not set.");

            var options = new ResendClientOptions { ApiToken = apiKey };
            var snapshot = new OptionsSnapshot<ResendClientOptions>(options);
            var httpClient = new HttpClient();

            Client = new ResendClient(snapshot, httpClient);
        }

        private class OptionsSnapshot<T> : IOptionsSnapshot<T> where T : class, new()
        {
            private readonly T _value;
            public OptionsSnapshot(T value) => _value = value;
            public T Value => _value;
            public T Get(string? name) => _value;
        }
    }
}