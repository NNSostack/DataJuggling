using EmailNotification.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace EmailNotification.Services
{
    public class EmailService : IEmail
    {
        ILog _logging { get; set; }

        public EmailService(ILog logging)
        {
            _logging = logging;

        }

        public void SendEmail(string to, string from, string subject, string body)
        {
            SmtpClient client = new SmtpClient();
            MailMessage mm = new MailMessage { From = new MailAddress(from), Subject = subject, Body = body };
            mm.To.Add(to);

#if !DEBUG
            client.Send(mm);
#else
            _logging.Info(String.Format("Mail sent: To: {0}, Body: {1}, Subject: {2}", to, subject, body.Replace("\r\n", "<br/>")));     
#endif
        }

        
    }
}
