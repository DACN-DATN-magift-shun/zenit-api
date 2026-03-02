using SendGrid;

using Zenit.Share.Business.Requests;

namespace Zenit.Share.Business.Interfaces
{
    public interface ISenGridEmailService
    {
        Task<Response> SendEmailAsync(SendGridEmailRequest request);
    }
}