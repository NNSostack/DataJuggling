using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailNotification.Model
{
    public class EmailNotificationItem
    {
        public String Email { get; set; }
        public DateTime Deadline { get; set; }
        public String Text { get; set; }
    }
}
