using EmailNotification.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailNotification.Interfaces
{
    public interface IStatus
    {
        Status GetStatus(Guid id);
        void SetLastRun(Guid id);
    }
}
