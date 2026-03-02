using SendGrid.Helpers.Mail;

namespace Zenit.Share.Business.Requests
{
    public class SendEmailRequest
    {
        public required EmailAddress From { get; set; }
        public required EmailAddress To { get; set; }
        public required string Subject { get; set; }
        public required string PlainTextContent { get; set; }
        public string? HtmlContent { get; set; }
    }
}