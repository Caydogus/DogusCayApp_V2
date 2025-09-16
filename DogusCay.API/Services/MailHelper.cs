using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace DogusCay.API.Helpers
{
    public class MailHelper
    {
        private readonly IConfiguration _config;

        public MailHelper(IConfiguration config)
        {
            _config = config;
        }

        public void Send(string to, string subject, string body)
        {
            var from = _config["EmailSettings:From"];
            var host = _config["EmailSettings:SmtpServer"];
            var port = int.Parse(_config["EmailSettings:Port"]);
            var enableSsl = bool.Parse(_config["EmailSettings:EnableSsl"]);
            var username = _config["EmailSettings:Username"];
            var password = _config["EmailSettings:Password"];

            using (var client = new SmtpClient(host, port))
            {
                client.EnableSsl = enableSsl;
                client.Credentials = new NetworkCredential(username, password);

                var mail = new MailMessage(from, to, subject, body);
                client.Send(mail);
            }
        }

        public string GetNotifyAddress()
        {
            return _config["EmailSettings:NotifyAddress"];
        }
    }
}
