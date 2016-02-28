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
        public void SendEmail(string to, string from, string subject, string body)
        {
            SmtpClient client = new SmtpClient();
            MailMessage mm = new MailMessage { From = new MailAddress(from), Subject = subject, Body = body };
            mm.To.Add(to);
            client.Send(mm);
        }
    }
}
