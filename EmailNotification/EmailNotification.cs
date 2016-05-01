using EmailNotification.Interfaces;
using EmailNotification.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailNotification
{
    public class EmailNotification
    {
        private ISettings _settings;
        private IEmailNotificationItems _emailNotificationItems;
        private IEmail _sendEmail;
        private IStatus _status;
        private IDate _date;
        private ILog _log;
        String _emailFrom = "";
        String _url = "";

        public EmailNotification(ILog logging, IDate date, ISettings settings, IEmailNotificationItems emailNotificationItems, IEmail sendEmail, IStatus status,
            String emailFrom, String url)
        {
            _settings = settings;
            _emailNotificationItems = emailNotificationItems;
            _sendEmail = sendEmail;
            _status = status;
            _date = date;
            _log = logging;
            _emailFrom = emailFrom;
            _url = url;

        }

        public void Run()
        {
            List<Settings> settings = new List<Settings>();

            if (!String.IsNullOrEmpty(_url))
                settings.Add(_settings.GetSettingsFromUrl(_url));
            else
                settings.AddRange(_settings.GetSettings());

            foreach (var setting in settings)
            {
                try
                {

                    _log.Info("*********************************************************");
                    _log.Info("  Setting found: " + setting.Id + ", " + setting.CsvFile + ", last run: " + _status.GetStatus(setting.Id).LastRun);
                    var status = _status.GetStatus(setting.Id);
                    Boolean doRun = false;

                    DateTime now = _date.GetNow();
                    String info = "";

                    Func<EmailNotificationItem, bool> predicate = null;

                    if (!RunToday(status))
                    {
                        _log.Msg("*********************************************************");

                        _log.Msg("  Setting found, not run today: " + setting.Id + ", " + setting.CsvFile);
                        if (setting.Frequence == Model.eFrequence.eDaily)
                        {
                            predicate = new Func<EmailNotificationItem, bool>(x => x.Deadline <= now.AddDays(2));

                            info = "Daily run";
                            doRun = true;
                        }
                        else if (setting.Frequence == Model.eFrequence.eWeekly)
                        {
                            predicate = new Func<EmailNotificationItem, bool>(x => x.Deadline <= now.AddDays(10));

                            if (setting.WeekFrequence == Model.eWeekFrequence.eFirstDayOfWeek && now.DayOfWeek == DayOfWeek.Monday)
                            {

                                info = "Weekly run on first day of week";
                                doRun = true;
                            }
                            else if (setting.WeekFrequence == Model.eWeekFrequence.eLastDayOfWeek && now.DayOfWeek == DayOfWeek.Sunday)
                            {
                                info = "Weekly run on last day of week";
                                doRun = true;
                            }
                            else if (setting.WeekFrequence == Model.eWeekFrequence.eSpecificWeekDayOfWeek && setting.DayOfWeek == now.DayOfWeek)
                            {
                                info = "Weekly run on specific day of week: " + setting.DayOfWeek;
                                doRun = true;
                            }
                        }
                        else if (setting.Frequence == Model.eFrequence.eMonthly)
                        {
                            predicate = new Func<EmailNotificationItem, bool>(x => x.Deadline <= now.AddMonths(2));

                            if (setting.MonthFrequence == Model.eMonthFrequence.eFirstDayOfMonth && now.Date.Day == 1)
                            {
                                info = "Monthly run on first day of month";
                                doRun = true;
                            }
                            else if (setting.MonthFrequence == Model.eMonthFrequence.eLastDayOfMonth && now.Date.AddDays(1).Month != now.Month)
                            {
                                info = "Monthly run on last day of month";
                                doRun = true;
                            }
                            else if (setting.MonthFrequence == Model.eMonthFrequence.eSpecificDayOfMonth && now.Day == setting.DayOfMonth)
                            {
                                info = "Monthly run on specific day of month: " + setting.DayOfMonth;
                                doRun = true;
                            }
                        }

                        if (doRun)
                        {
                            _log.Msg("      Job started, " + info);
                            RunJob(setting, predicate);
                        }

                        _log.Msg("Last run set");
                        _status.SetLastRun(setting.Id);
                    }
                }
                catch (Exception ex)
                {
                    _sendEmail.SendEmail(setting.Email, _emailFrom, "Error found in: " + setting.EmailSubject, ex.ToString());
                    _sendEmail.SendEmail("nns@email.dk", _emailFrom, "Error found in: " + setting.EmailSubject + " sendt til " + setting.Email, ex.ToString());
                    
                }

            }
            _log.Info("*********************************************************");
        }

        private void RunJob(Settings setting, Func<EmailNotificationItem, bool> predicateBuilder)
        {
            var emailNotificationItems = _emailNotificationItems.GetEmailNotificationItems(setting)
                .Where(x => x.Deadline.Date >= _date.GetNow().Date).Where(predicateBuilder);

            foreach (var grp in emailNotificationItems.GroupBy(x => x.Email))
            {
                if (!String.IsNullOrEmpty(grp.Key))
                {
                    _log.Msg("          Email sent to: " + grp.Key);
                    _sendEmail.SendEmail(grp.Key, _emailFrom, setting.EmailSubject, String.Join("\r\n", grp.OrderBy(x => x.Deadline).Select(x => x.Deadline.ToString("dd-MM-yyyy") + " - " + x.Text).ToArray()));
                }
            }
        }


        private bool RunToday(Model.Status status)
        {
            return status.LastRun.Date == _date.GetNow().Date;
        }
    }
}
