using EmailNotification.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailNotification.Services
{
    public class DateService : IDate
    {
        DateTime _now = DateTime.Now;
        public DateTime GetNow()
        {
            return _now;
        }
    }
}
