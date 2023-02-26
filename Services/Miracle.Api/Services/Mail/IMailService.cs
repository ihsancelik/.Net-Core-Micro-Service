using System.Net.Mail;
using System.Threading.Tasks;

namespace Miracle.Api.Services
{
    public interface IMailService
    {
        public void Initialize(string host, int port, string email, string password, bool enableSSL);
        public bool Send(MailMessage message);
        public Task SendAsync(MailMessage message);
    }
}
