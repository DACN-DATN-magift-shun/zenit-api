using Resend;

using Zenit.Share.Business.Interfaces;
using Zenit.Share.Business.Requests;

namespace Zenit.Share.Business
{
    public abstract class ResendEmailServiceBase : IResendEmailService
    {
        public ResendClient Client { get; set; }
        public async Task<ResendResponse> SendEmailAsync(ResendEmailRequest request)
        {
            var message = new EmailMessage
            {
                From = request.From,
                To = new EmailAddressList { request.To },
                Subject = request.Subject,
                HtmlBody = request.HtmlBody
            };

            var response = await Client.EmailSendAsync(message);

            return response;
        }
    }
}