using System.Threading.Tasks;

namespace Phoneden.Web.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
