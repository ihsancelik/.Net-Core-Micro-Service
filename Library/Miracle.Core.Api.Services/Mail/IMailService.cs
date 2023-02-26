using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Miracle.Core.Api.Services
{
    public interface IMailService
    {
        public List<Exception> Exceptions { get; set; }
        public void Initialize(string host, int port, string email, string password, bool enableSSL);
        public bool Send(MailMessage message);
        public Task SendAsync(MailMessage message);
    }
}
