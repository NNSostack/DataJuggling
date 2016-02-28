using EmailNotification.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailNotification.Repositories
{
    public class StatusRepository : IStatus
    {
        private string _path;
        public StatusRepository(String path)
        {
            _path = path;
        }

        public Model.Status GetStatus(Guid id)
        {
            var file = GetFilePath(id);
            if (!System.IO.File.Exists(file))
                return new Model.Status { LastRun = DateTime.MinValue };

            String[] items = System.IO.File.ReadAllText(file).Split(';');
            return new Model.Status
            {
                LastRun = DateTime.Parse(items[0])
            };

        }

        public void SetLastRun(Guid id)
        {
            var file = GetFilePath(id);
            
            System.IO.File.WriteAllText(file, DateTime.Now.ToString());

        }

        String GetFilePath(Guid id)
        {
            return System.IO.Path.Combine(_path, id.ToString("N")) + ".status.txt";
        }
    }
}
