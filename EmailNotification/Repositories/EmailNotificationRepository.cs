using EmailNotification.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailNotification.Repositories
{
    public class EmailNotificationRepository : IEmailNotificationItems
    {
        public List<Model.EmailNotificationItem> GetEmailNotificationItems(string csvFile)
        {
            return new List<Model.EmailNotificationItem>();
        }

        public void SetEmailNotifications(string csvFile)
        {
            throw new NotImplementedException();
        }
    }
}
