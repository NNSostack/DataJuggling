﻿using EmailNotification.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailNotification.Services
{
    public class LogService : ILog
    {
        private bool _includeInfo;
        private string _path;

        public LogService(Boolean includeInfo, String path)
        {
            _includeInfo = includeInfo;
            _path = path;
        }

        public void Info(string msg)
        {
            if( _includeInfo )
                Msg("INFO: " + msg);
        }

        static Object myLock = new object();
        public void Msg(string msg)
        {
            var file = System.IO.Path.Combine(_path, "log-" + DateTime.Now.Date.ToString("YYYYmmdd") + ".log"); 
            lock(myLock)
            {
                System.IO.File.AppendAllLines(file, new String[] { msg });
            }
        }

        public void Error(string msg)
        {
            Msg("ERROR: " + msg);
        }
    }
}