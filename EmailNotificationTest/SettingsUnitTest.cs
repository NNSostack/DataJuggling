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
    public class SettingsUnitTest
    {
        [TestMethod]
        public void SettingsTestWithMissingQS()
        {
            SettingsRepository settings = new SettingsRepository(
             String.Format("http://www.datajonglering.dk?id={0}&csvfile={1}",
             "myId",
             "myCsvFile"));
             

            try
            {
                var mySettings = settings.GetSettings();
                Assert.Fail("An exception needs to be thrown");
            }
            catch (Exception)
            {
            }
        }
        


        [TestMethod]
        public void SettingsTestWithDefaults()
        {
            Guid id = Guid.NewGuid();
            SettingsRepository settings = new SettingsRepository(
                String.Format("http://www.datajonglering.dk?id={0}&csvfile={1}&deadlinefield={2}&textfield={3}&emailfield={4}",
                id.ToString(),
                "myCsvFile",
                "myDeadlineField",
                "myTextField", 
                "myEmailField"));

            var mySettings = settings.GetSettings();
            Assert.AreEqual(1, mySettings.Count);
            
            Settings mySetting = mySettings[0];

            Assert.AreEqual(id, mySetting.Id);
            Assert.AreEqual("myCsvFile", mySetting.CsvFile);
            Assert.AreEqual(eFrequence.eMonthly, mySetting.Frequence);

            Assert.AreEqual(eWeekFrequence.eFirstDayOfWeek, mySetting.WeekFrequence);
            Assert.AreEqual(eMonthFrequence.eFirstDayOfMonth, mySetting.MonthFrequence);
            Assert.AreEqual(DayOfWeek.Monday, mySetting.DayOfWeek);
            Assert.AreEqual(1, mySetting.DayOfMonth);
            Assert.AreEqual(true, mySetting.HasColumnHeaders);

            Assert.AreEqual("myEmailField", mySetting.EmailField);
            Assert.AreEqual("myTextField", mySetting.TextField);
            Assert.AreEqual("myEmailField", mySetting.EmailField);


        }


        [TestMethod]
        public void SettingsTestWithNoDefaults()
        {
            Guid id = Guid.NewGuid();
            SettingsRepository settings = new SettingsRepository(
                String.Format("http://www.datajonglering.dk?id={0}&csvfile={1}&frequence={2}&deadlinefield={3}&textfield={4}&emailfield={5}&hascolumnheaders={6}&weekfrequence={7}&monthfrequence={8}&dayofweek={9}&dayofmonth={10}",
                id.ToString(),
                "myCsvFile",
                eFrequence.eDaily,
                "myDeadlineField",
                "myTextField",
                "myEmailField",
                false,
                eWeekFrequence.eSpecificWeekDayOfWeek,
                eMonthFrequence.eLastDayOfMonth,
                DayOfWeek.Saturday,
                13));

            var mySettings = settings.GetSettings();
            Assert.AreEqual(1, mySettings.Count);

            Settings mySetting = mySettings[0];

            Assert.AreEqual(id, mySetting.Id);
            Assert.AreEqual("myCsvFile", mySetting.CsvFile);
            Assert.AreEqual(eFrequence.eDaily, mySetting.Frequence);

            Assert.AreEqual(eWeekFrequence.eSpecificWeekDayOfWeek, mySetting.WeekFrequence);
            Assert.AreEqual(eMonthFrequence.eLastDayOfMonth, mySetting.MonthFrequence);
            Assert.AreEqual(DayOfWeek.Saturday, mySetting.DayOfWeek);
            Assert.AreEqual(13, mySetting.DayOfMonth);
            Assert.AreEqual(false, mySetting.HasColumnHeaders);

            Assert.AreEqual("myEmailField", mySetting.EmailField);
            Assert.AreEqual("myTextField", mySetting.TextField);
            Assert.AreEqual("myEmailField", mySetting.EmailField);


        }
    }
}    
    
