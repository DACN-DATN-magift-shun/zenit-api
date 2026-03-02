using SendGrid;

using Zenit.Share.Business;
using Zenit.Share.Common.Constants;

namespace Zenit.Management.Business
{
    public class ManagementEmailService : EmailServiceBase
    {
        public ManagementEmailService()
        {
            string apiKey = Environment.GetEnvironmentVariable(EnvConstants.SENDGRID_API_KEY) ?? throw new Exception("SendGrid API Key is not set.");
            Client = new SendGridClient(apiKey);
        }
    }
}