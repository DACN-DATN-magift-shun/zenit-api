using SendGrid;
using SendGrid.Helpers.Mail;

using Zenit.Share.Business.Interfaces;
using Zenit.Share.Business.Requests;

namespace Zenit.Share.Business
{
    public abstract class EmailServiceBase : IEmailService
    {
        public SendGridClient Client { get; set; }
        public async Task<Response> SendEmailAsync(SendEmailRequest request)
        {
            var message = MailHelper.CreateSingleEmail(
                request.From,
                request.To,
                request.Subject,
                request.PlainTextContent,
                request.HtmlContent);

            var response = await Client.SendEmailAsync(message);

            return response;
        }
    }
}