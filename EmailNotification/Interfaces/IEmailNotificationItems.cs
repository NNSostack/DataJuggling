using EmailNotification.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailNotification.Interfaces
{
    public interface IEmailNotificationItems
    {
        List<EmailNotificationItem> GetEmailNotificationItems(Settings settings);
        void SetEmailNotifications(String csvFile);

    }
}
