using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EmailNotification.Interfaces;
using NSubstitute;
using EmailNotification.Model;
using System.Collections.Generic;
using EmailNotification.Repositories;

namespace EmailNotificationTest
{
    [TestClass]
    public class UnitTest1
    {
        private EmailNotification.Interfaces.ILog _logging;
        private EmailNotification.Interfaces.IDate _date;
        private EmailNotification.Interfaces.ISettings _settings;
        private EmailNotification.Interfaces.IEmailNotificationItems _emailNotificationItems;
        private EmailNotification.Interfaces.IEmail _sendEmail;
        private EmailNotification.Interfaces.IStatus _status;
        private Settings _setting;
        private Status _statusItem;
        private List<EmailNotificationItem> _emailNotificationItemList = new List<EmailNotificationItem>();

        DateTime _now = new DateTime(2016, 02, 21);

        EmailNotification.EmailNotification Substitute()
        {
            return Substitute(_now);
        }

        EmailNotification.EmailNotification Substitute(DateTime now)
        {
            _logging = NSubstitute.Substitute.For<ILog>();
            _date = NSubstitute.Substitute.For<IDate>();
            _settings = NSubstitute.Substitute.For<ISettings>();
            _emailNotificationItems = NSubstitute.Substitute.For<IEmailNotificationItems>();
            _status = NSubstitute.Substitute.For<IStatus>();
            _sendEmail = NSubstitute.Substitute.For<IEmail>();

            _now = now;
            _date.GetNow().Returns(now);

            _setting = new Settings
                {
                    Id = Guid.NewGuid(),
                    CsvFile = "csvFile"
                };


            _statusItem = new Status
                {
                    LastRun = DateTime.MinValue
                };

            _settings.GetSettingsFromUrl("url").Returns(_setting);

            _status.GetStatus(_setting.Id).Returns(_statusItem);

            _emailNotificationItemList = new List<EmailNotificationItem> {
                    new EmailNotificationItem
                    {
                        Deadline = now.AddDays(-1),
                        Email = "nns@email1234.dk",
                        Text = "Text1234"
                    },

                    new EmailNotificationItem
                    {
                        Deadline = now,
                        Email = "nns@email.dk",
                        Text = "Text"
                    },
                    new EmailNotificationItem
                    {
                        Deadline = now.AddDays(3),
                        Email = "nns@email3.dk",
                        Text = "Text3"
                    },
                    new EmailNotificationItem
                    {
                        Deadline = now.AddDays(7),
                        Email = "nns@email7.dk",
                        Text = "Text7"
                    },
                    new EmailNotificationItem
                    {
                        Deadline = now.AddDays(14),
                        Email = "nns@email14.dk",
                        Text = "Text14"
                    },
                    new EmailNotificationItem
                    {
                        Deadline = now.AddDays(21),
                        Email = "nns@email21.dk",
                        Text = "Text21"
                    },
                    new EmailNotificationItem
                    {
                        Deadline = now.AddDays(25),
                        Email = "nns@email25.dk",
                        Text = "Text25"
                    },
                    new EmailNotificationItem
                    {
                        Deadline = now.AddDays(35),
                        Email = "nns@email35.dk",
                        Text = "Text35"
                    }
            };
            

            _emailNotificationItems.GetEmailNotificationItems(_setting).Returns(              
               _emailNotificationItemList
            );

            EmailNotification.EmailNotification repo = new EmailNotification
            .EmailNotification(_logging, _date, _settings, _emailNotificationItems, _sendEmail, _status, "emailFrom", "url");

            return repo;

        }


        [TestMethod]
        public void DailyRunNotRunToday()
        {
            var repo = Substitute();

            _setting.Frequence = eFrequence.eDaily;
            
            repo.Run();

            _status.Received().SetLastRun(_setting.Id);
            _logging.Received().Info("  Setting found, not run today: " + _setting.Id + ", " + _setting.CsvFile);
            _logging.Received().Msg("      Job started, Daily run");
            
            _sendEmail.Received().SendEmail("nns@email.dk", "emailFrom", "emailSubject", "Text");
        }

        [TestMethod]
        public void DailyRunAlreadyRunToday()
        {
            var repo = Substitute();

            _setting.Frequence = eFrequence.eDaily;
            _statusItem.LastRun = _now;

            repo.Run();

            _status.DidNotReceiveWithAnyArgs().SetLastRun(_setting.Id);
            _logging.DidNotReceive().Info("  Setting found, not run today: " + _setting.Id + ", " + _setting.CsvFile);
            _logging.DidNotReceive().Msg("      Job started, Daily run");

            _sendEmail.DidNotReceiveWithAnyArgs().SendEmail("nns@email.dk", "emailFrom", "emailSubject", "Text");
        }


        [TestMethod]
        public void WeeklyRunNotSpecificWeekDay()
        {
            var repo = Substitute();

            _setting.Frequence = eFrequence.eWeekly;
            _setting.WeekFrequence = eWeekFrequence.eSpecificWeekDayOfWeek;
             // Now is sunday set in Substitute
            _setting.DayOfWeek = DayOfWeek.Monday;

            repo.Run();

            _status.Received().SetLastRun(_setting.Id);
            _logging.Received().Info("  Setting found, not run today: " + _setting.Id + ", " + _setting.CsvFile);

            _sendEmail.DidNotReceiveWithAnyArgs().SendEmail("nns@email.dk", "emailFrom", "emailSubject", "Text");

        }

        [TestMethod]
        public void WeeklyRunSpecificWeekDay()
        {
            var repo = Substitute(_now.AddDays(-3));

            _setting.Frequence = eFrequence.eWeekly;
            _setting.WeekFrequence = eWeekFrequence.eSpecificWeekDayOfWeek;
            _setting.DayOfWeek = _now.DayOfWeek;

            repo.Run();

            _status.Received().SetLastRun(_setting.Id);
            _logging.Received().Info("  Setting found, not run today: " + _setting.Id + ", " + _setting.CsvFile);
            _logging.Received().Msg("      Job started, Weekly run on specific day of week: " + _now.DayOfWeek);

            _sendEmail.Received().SendEmail("nns@email.dk", "emailFrom", "emailSubject", "Text");
            _sendEmail.Received().SendEmail("nns@email3.dk", "emailFrom", "emailSubject", "Text3");
            _sendEmail.Received().SendEmail("nns@email7.dk", "emailFrom", "emailSubject", "Text7");
            _sendEmail.DidNotReceive().SendEmail("nns@email14.dk", "emailFrom", "emailSubject", "Text14");

        }

        [TestMethod]
        public void WeeklyRunFirstDayOfWeek()
        {
            var repo = Substitute(_now.AddDays(1));

            _setting.Frequence = eFrequence.eWeekly;
            _setting.WeekFrequence = eWeekFrequence.eFirstDayOfWeek;

            repo.Run();

            _status.Received().SetLastRun(_setting.Id);
            _logging.Received().Info("  Setting found, not run today: " + _setting.Id + ", " + _setting.CsvFile);
            _logging.Received().Msg("      Job started, Weekly run on first day of week");

            _sendEmail.Received().SendEmail("nns@email.dk", "emailFrom", "emailSubject", "Text");
            _sendEmail.Received().SendEmail("nns@email3.dk", "emailFrom", "emailSubject", "Text3");
            _sendEmail.Received().SendEmail("nns@email7.dk", "emailFrom", "emailSubject", "Text7");
            _sendEmail.DidNotReceive().SendEmail("nns@email14.dk", "emailFrom", "emailSubject", "Text14");
        }

        [TestMethod]
        public void WeeklyRunFirstDayOfWeekNotFirstDay()
        {
            var repo = Substitute();

            _setting.Frequence = eFrequence.eWeekly;
            _setting.WeekFrequence = eWeekFrequence.eFirstDayOfWeek;

            repo.Run();

            _status.Received().SetLastRun(_setting.Id);
            _logging.Received().Info("  Setting found, not run today: " + _setting.Id + ", " + _setting.CsvFile);
            _logging.DidNotReceive().Msg("      Job started, Weekly run on first day of week");

            _sendEmail.DidNotReceiveWithAnyArgs().SendEmail("nns@email.dk", "emailFrom", "emailSubject", "Text");
        }

        [TestMethod]
        public void WeeklyRunLastDayOfWeek()
        {
            var repo = Substitute();

            _setting.Frequence = eFrequence.eWeekly;
            _setting.WeekFrequence = eWeekFrequence.eLastDayOfWeek;

            repo.Run();

            _status.Received().SetLastRun(_setting.Id);
            _logging.Received().Info("  Setting found, not run today: " + _setting.Id + ", " + _setting.CsvFile);
            _logging.Received().Msg("      Job started, Weekly run on last day of week");

            _sendEmail.Received().SendEmail("nns@email.dk", "emailFrom", "emailSubject", "Text");
            _sendEmail.Received().SendEmail("nns@email3.dk", "emailFrom", "emailSubject", "Text3");
            _sendEmail.Received().SendEmail("nns@email7.dk", "emailFrom", "emailSubject", "Text7");
            _sendEmail.DidNotReceive().SendEmail("nns@email14.dk", "emailFrom", "emailSubject", "Text14");
        }

        [TestMethod]
        public void WeeklyRunLastDayOfWeekNotLastDayOfWeek()
        {
            var repo = Substitute(_now.AddDays(3));

            _setting.Frequence = eFrequence.eWeekly;
            _setting.WeekFrequence = eWeekFrequence.eLastDayOfWeek;

            repo.Run();

            _status.Received().SetLastRun(_setting.Id);
            _logging.Received().Info("  Setting found, not run today: " + _setting.Id + ", " + _setting.CsvFile);
            _logging.DidNotReceive().Msg("      Job started, Weekly run on last day of week");

            _sendEmail.DidNotReceiveWithAnyArgs().SendEmail("nns@email.dk", "emailFrom", "emailSubject", "Text");
        }

        [TestMethod]
        public void MonthlyRunFirstDayOfMonth()
        {
            var repo = Substitute(new DateTime(2016, 02, 01));

            _setting.Frequence = eFrequence.eMonthly;
            _setting.MonthFrequence = eMonthFrequence.eFirstDayOfMonth;

            repo.Run();

            _status.Received().SetLastRun(_setting.Id);
            _logging.Received().Info("  Setting found, not run today: " + _setting.Id + ", " + _setting.CsvFile);
            _logging.Received().Msg("      Job started, Monthly run on first day of month");

            _sendEmail.Received().SendEmail("nns@email.dk", "emailFrom", "emailSubject", "Text");
            _sendEmail.Received().SendEmail("nns@email3.dk", "emailFrom", "emailSubject", "Text3");
            _sendEmail.Received().SendEmail("nns@email7.dk", "emailFrom", "emailSubject", "Text7");
            _sendEmail.Received().SendEmail("nns@email14.dk", "emailFrom", "emailSubject", "Text14");
            _sendEmail.Received().SendEmail("nns@email21.dk", "emailFrom", "emailSubject", "Text21");
            _sendEmail.Received().SendEmail("nns@email25.dk", "emailFrom", "emailSubject", "Text25");
            _sendEmail.DidNotReceive().SendEmail("nns@email35.dk", "emailFrom", "emailSubject", "Text35");
        }

        [TestMethod]
        public void MonthlyRunFirstDayOfMonthNotFirstDayOfMonth()
        {
            var repo = Substitute();

            _setting.Frequence = eFrequence.eMonthly;
            _setting.MonthFrequence = eMonthFrequence.eFirstDayOfMonth;

            repo.Run();

            _status.Received().SetLastRun(_setting.Id);
            _logging.Received().Info("  Setting found, not run today: " + _setting.Id + ", " + _setting.CsvFile);
            _logging.DidNotReceive().Msg("      Job started, Monthly run on first day of month");

            _sendEmail.DidNotReceiveWithAnyArgs().SendEmail("nns@email.dk", "emailFrom", "emailSubject", "Text");
        }


        [TestMethod]
        public void MonthlyRunLastDayOfMonth()
        {
            var repo = Substitute(new DateTime(2016, 02, 01).AddDays(-1));

            _setting.Frequence = eFrequence.eMonthly;
            _setting.MonthFrequence = eMonthFrequence.eLastDayOfMonth;

            repo.Run();

            _status.Received().SetLastRun(_setting.Id);
            _logging.Received().Info("  Setting found, not run today: " + _setting.Id + ", " + _setting.CsvFile);
            _logging.Received().Msg("      Job started, Monthly run on last day of month");

            _sendEmail.Received().SendEmail("nns@email.dk", "emailFrom", "emailSubject", "Text");
            _sendEmail.Received().SendEmail("nns@email3.dk", "emailFrom", "emailSubject", "Text3");
            _sendEmail.Received().SendEmail("nns@email7.dk", "emailFrom", "emailSubject", "Text7");
            _sendEmail.Received().SendEmail("nns@email14.dk", "emailFrom", "emailSubject", "Text14");
            _sendEmail.Received().SendEmail("nns@email21.dk", "emailFrom", "emailSubject", "Text21");
            _sendEmail.Received().SendEmail("nns@email25.dk", "emailFrom", "emailSubject", "Text25");
            _sendEmail.DidNotReceive().SendEmail("nns@email35.dk", "emailFrom", "emailSubject", "Text35");
        }



        [TestMethod]
        public void MonthlyRunLastDayOfMonthNotLastDay()
        {
            var repo = Substitute();

            _setting.Frequence = eFrequence.eMonthly;
            _setting.MonthFrequence = eMonthFrequence.eLastDayOfMonth;

            repo.Run();

            _status.Received().SetLastRun(_setting.Id);
            _logging.Received().Info("  Setting found, not run today: " + _setting.Id + ", " + _setting.CsvFile);
            _logging.DidNotReceive().Msg("      Job started, Monthly run on last day of month");

            _sendEmail.DidNotReceiveWithAnyArgs().SendEmail("nns@email.dk", "emailFrom", "emailSubject", "Text");
        }


        [TestMethod]
        public void MonthlyRunSpecificDay()
        {
            var repo = Substitute();

            _setting.Frequence = eFrequence.eMonthly;
            _setting.MonthFrequence = eMonthFrequence.eSpecificDayOfMonth;
            _setting.DayOfMonth = 21;

            repo.Run();

            _status.Received().SetLastRun(_setting.Id);
            _logging.Received().Info("  Setting found, not run today: " + _setting.Id + ", " + _setting.CsvFile);
            _logging.Received().Msg("      Job started, Monthly run on specific day of month: " + _setting.DayOfMonth);

            _sendEmail.Received().SendEmail("nns@email.dk", "emailFrom", "emailSubject", "Text");
            _sendEmail.Received().SendEmail("nns@email3.dk", "emailFrom", "emailSubject", "Text3");
            _sendEmail.Received().SendEmail("nns@email7.dk", "emailFrom", "emailSubject", "Text7");
            _sendEmail.Received().SendEmail("nns@email14.dk", "emailFrom", "emailSubject", "Text14");
            _sendEmail.Received().SendEmail("nns@email21.dk", "emailFrom", "emailSubject", "Text21");
            _sendEmail.Received().SendEmail("nns@email25.dk", "emailFrom", "emailSubject", "Text25");
            _sendEmail.DidNotReceive().SendEmail("nns@email35.dk", "emailFrom", "emailSubject", "Text35");
        }
        
        [TestMethod]
        public void MonthlyRunSpecificDayNotSpecificDay()
        {
            var repo = Substitute();

            _setting.Frequence = eFrequence.eMonthly;
            _setting.MonthFrequence = eMonthFrequence.eSpecificDayOfMonth;
            _setting.DayOfMonth = 3;

            repo.Run();

            _status.Received().SetLastRun(_setting.Id);
            _logging.Received().Info("  Setting found, not run today: " + _setting.Id + ", " + _setting.CsvFile);
            _logging.DidNotReceive().Msg("      Job started, Monthly run on specific day of month: " + _setting.DayOfMonth);

            _sendEmail.DidNotReceiveWithAnyArgs().SendEmail("nns@email.dk", "emailFrom", "emailSubject", "Text");
        }

    }
}
