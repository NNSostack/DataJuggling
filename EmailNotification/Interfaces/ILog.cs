using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailNotification.Interfaces
{
    public interface ILog
    {
        void Debug(String msg);
        void Info(String msg);
        void Msg(String msg);
        void Error(String msg);
    }
}
