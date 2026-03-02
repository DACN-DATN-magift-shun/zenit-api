using Resend;

using Zenit.Share.Business.Requests;

namespace Zenit.Share.Business.Interfaces
{
    public interface IResendEmailService
    {
        Task<ResendResponse> SendEmailAsync(ResendEmailRequest request);
    }
}