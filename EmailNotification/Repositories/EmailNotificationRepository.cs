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
        public List<Model.EmailNotificationItem> GetEmailNotificationItems(Model.Settings settings)
        {
            List<Model.EmailNotificationItem> list = new List<Model.EmailNotificationItem>();
            var csvList = Csv.CsvListRepository.GetList(settings.CsvFile, settings.HasColumnHeaders, "", "", "", false, new String[] { settings.DeadlineField, settings.EmailField, settings.TextField }); 

            var cols = csvList.ColumnNames.Select(x => x.Name).ToList();

            int iDateField = cols.Select(x => x.ToLower()).ToList().IndexOf(settings.DeadlineField.ToLower());
            int iEmailField = cols.Select(x => x.ToLower()).ToList().IndexOf(settings.EmailField.ToLower());
            int iTextField = cols.Select(x => x.ToLower()).ToList().IndexOf(settings.TextField.ToLower());

            if (iDateField == -1)
                throw new ApplicationException("Deadlinefield '" + settings.DeadlineField + "' can't be found in csv!!");

            if (iEmailField == -1)
                throw new ApplicationException("Emailfield '" + settings.EmailField + "' can't be found in csv!!");

            if (iTextField == -1)
                throw new ApplicationException("Textfield '" + settings.TextField + "' can't be found in csv!!");


            foreach (var row in csvList.First().Rows)
            {
                list.Add(new Model.EmailNotificationItem
                {
                    Deadline = (DateTime)row.Values[iDateField].Value,
                    Email = row.Values[iEmailField].Value as String,
                    Text = row.Values[iTextField].Value as String
                });
             }
            
            return list;
        }

        public void SetEmailNotifications(string csvFile)
        {
            throw new NotImplementedException();
        }
    }
}
