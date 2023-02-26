using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Miracle.Core.Api.Services
{
    public class MailService : IMailService
    {
        private SmtpClient client;
        public List<Exception> Exceptions { get; set; }
        public MailService()
        {
            client = new SmtpClient();
        }

        /// <summary>
        ///  Initialize = Mail gönderme servisinin yapılandırılması
        ///  Send  = Mail gönderme işlevini gerçekleştirir
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="enableSSL"></param>
        /// <returns></returns>
        public void Initialize(string host, int port, string email, string password, bool enableSSL)
        {
            client.Host = host;
            client.Port = port;
            client.EnableSsl = enableSSL;
            client.Credentials = new NetworkCredential(email, password);
        }
        public bool Send(MailMessage message)
        {
            try
            {
                client.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                Exceptions.Add(ex);
                return false;
            }
        }
        public async Task SendAsync(MailMessage message)
        {
            try
            {
                await client.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                Exceptions.Add(ex);
            }
        }
    }
}