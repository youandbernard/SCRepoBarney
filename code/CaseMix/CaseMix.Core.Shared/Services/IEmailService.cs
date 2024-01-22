using System.Threading.Tasks;

namespace CaseMix.Core.Shared.Services
{
    public interface IEmailService
    {
        Task SendAsync(string toName, string toEmail, string subject, string body);
    }
}
