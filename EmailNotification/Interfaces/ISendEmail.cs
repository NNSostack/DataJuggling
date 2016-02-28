using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace EmailNotification.Interfaces
{
    public interface IEmail
    {
        void SendEmail(String to, String from, String subject, String body);
    }
}
