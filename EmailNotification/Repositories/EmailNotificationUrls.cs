using EmailNotification.Interfaces;
using EmailNotification.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailNotification.Repositories
{
    public class EmailNotificationUrls : IEmailNotificationUrls
    {
        private string _settingsFile;
        
        public EmailNotificationUrls(String settingsFile)
        {
            _settingsFile = settingsFile;
        }

        
        public List<string> GetEmailNotificationUrls()
        {
            return System.IO.File.ReadAllLines(_settingsFile).ToList();
        }
    }
}
