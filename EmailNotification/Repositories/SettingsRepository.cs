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
        //private string _settingsFile;
        IEmailNotificationUrls _emailNotificationUrls;
        String _url;
        //public SettingsRepository(String settingsFile)
        //{
        //    _settingsFile = settingsFile;
        //}
        
        public SettingsRepository(IEmailNotificationUrls emailNotificationUrls)
        {
            _emailNotificationUrls = emailNotificationUrls;
        }

        public SettingsRepository(String url)
        {
            _url = url;
        }
        
        public List<Model.Settings> GetSettings()
        {
            List<Model.Settings> list = new List<Model.Settings>();

            if (_emailNotificationUrls != null)
            {
                return GetSettingsFromUrls();
            }

            if (!String.IsNullOrEmpty(_url))
                return new List<Settings> { GetSettingsFromUrl(_url) };
            
            
            //if (!String.IsNullOrEmpty(_settingsFile))
            //{
            //    GetSettingsFromFile(list);
            //}
            
            
            //GetSettingsFromKeyValuePair(_keyValuePair);

            return list;
        }

        public Model.Settings GetSettingsFromUrl(string url)
        {
            Uri uri = new Uri(url);
            String query = uri.Query;
            
            if( query.StartsWith("?") )                
                query = query.Substring(1);
            var items = query.Split(new[] { '&' });
            var dict = items.Select(item => item.Split(new[] { '=' })).ToDictionary(pair => pair[0].ToLower(), pair => System.Web.HttpUtility.UrlDecode(pair[1]));
            return GetSettingsFromKeyValuePair(dict);
        }

        private Model.Settings GetSettingsFromKeyValuePair(Dictionary<String, String> keyValuePair)
        {
        //            public String DeadlineField { get; set; }
        //public String TextField { get; set; }
        //public String EmailField { get; set; }


            if (!keyValuePair.ContainsKey("id") || !keyValuePair.ContainsKey("csvfile") ||
                !keyValuePair.ContainsKey("deadlinefield") ||
                !keyValuePair.ContainsKey("textfield") ||
                !keyValuePair.ContainsKey("emailfield")
                )
                throw new ApplicationException("id, csvfile, deadlinefield, textfield and emailfield are required settings items");

            if (!keyValuePair.ContainsKey("frequence"))
                keyValuePair.Add("frequence", eFrequence.eMonthly.ToString());
                        
            if (!keyValuePair.ContainsKey("hascolumnheaders"))
                keyValuePair.Add("hascolumnheaders", "true");

            if (!keyValuePair.ContainsKey("weekfrequence"))
                keyValuePair.Add("weekfrequence", eWeekFrequence.eFirstDayOfWeek.ToString());

            if (!keyValuePair.ContainsKey("monthfrequence"))
                keyValuePair.Add("monthfrequence", eMonthFrequence.eFirstDayOfMonth.ToString());

            if (!keyValuePair.ContainsKey("dayofweek"))
                keyValuePair.Add("dayofweek", DayOfWeek.Monday.ToString());

            if (!keyValuePair.ContainsKey("dayofmonth"))
                keyValuePair.Add("dayofmonth", 1.ToString());

            if (!keyValuePair.ContainsKey("subject"))
                keyValuePair.Add("subject", "Email reminder");

            if (!keyValuePair.ContainsKey("email"))
                keyValuePair.Add("email", "nns@email.dk");

            var settings = new Model.Settings
            {
                Id = new Guid(keyValuePair["id"]),
                CsvFile = keyValuePair["csvfile"],
                Frequence = (eFrequence)Enum.Parse(typeof(eFrequence), keyValuePair["frequence"]),
                WeekFrequence = (eWeekFrequence)Enum.Parse(typeof(eWeekFrequence), keyValuePair["weekfrequence"]),
                MonthFrequence = (eMonthFrequence)Enum.Parse(typeof(eMonthFrequence), keyValuePair["monthfrequence"]),
                DayOfWeek = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), keyValuePair["dayofweek"]),
                DayOfMonth = int.Parse(keyValuePair["dayofmonth"]),
                TextField = keyValuePair["textfield"],
                EmailField = keyValuePair["emailfield"],
                DeadlineField = keyValuePair["deadlinefield"],
                HasColumnHeaders = Boolean.Parse(keyValuePair["hascolumnheaders"]),
                EmailSubject = keyValuePair["subject"],
                Email = keyValuePair["email"] 

            };

            return settings;

        }

        private List<Model.Settings> GetSettingsFromUrls()
        {
            List<Model.Settings> list = new List<Settings>();
            foreach( var url in _emailNotificationUrls.GetEmailNotificationUrls() )
                list.Add(GetSettingsFromUrl(url));
 
            return list;
        }

        //private void GetSettingsFromFile(List<Model.Settings> list)
        //{
        //    foreach (var line in System.IO.File.ReadAllLines(_settingsFile))
        //    {
        //        String[] items = line.Split(';');
        //        Settings setting = new Settings
        //        {
        //            Id = new Guid(items[0]),
        //            CsvFile = items[1],
        //            Frequence = (eFrequence)Enum.Parse(typeof(eFrequence), items[2]),
        //            WeekFrequence = (eWeekFrequence)Enum.Parse(typeof(eWeekFrequence), items[3]),
        //            MonthFrequence = (eMonthFrequence)Enum.Parse(typeof(eMonthFrequence), items[4]),
        //            DayOfWeek = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), items[5]),
        //            DayOfMonth = int.Parse(items[6])
        //        };

        //        list.Add(setting);

        //    }
        //}
    }
}
