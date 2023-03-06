using JobExchangeAPI.Models.RequestModels;

namespace JobExchangeAPI.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(EmailModel emailModel);
    }
}
