using SendGrid;

using Zenit.Share.Business.Requests;

namespace Zenit.Share.Business.Interfaces
{
    public interface IEmailService
    {
        Task<Response> SendEmailAsync(SendEmailRequest request);
    }
}