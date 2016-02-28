using EmailNotification.Interfaces;
using EmailNotification.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailNotification.Repositories
{
    public class SettingsRepository : ISettings
    {
        private string _settingsFile;
        public SettingsRepository(String settingsFile)
        {
            _settingsFile = settingsFile;
        }
        
        public List<Model.Settings> GetSettings()
        {
            List<Model.Settings> list = new List<Model.Settings>();
            foreach (var line in System.IO.File.ReadAllLines(_settingsFile))
            {
                String[] items = line.Split(';');
                Settings setting = new Settings
                {
                    Id = new Guid(items[0]),
                    CsvFile = items[1],
                    Frequence = (eFrequence)Enum.Parse(typeof(eFrequence), items[2]),
                    WeekFrequence = (eWeekFrequence)Enum.Parse(typeof(eWeekFrequence), items[3]),
                    MonthFrequence = (eMonthFrequence)Enum.Parse(typeof(eMonthFrequence), items[4]),
                    DayOfWeek = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), items[5]),
                    DayOfMonth = int.Parse(items[6])
                };

                list.Add(setting);

            }
            return list;
        }
    }
}
