using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailNotification.Model
{
    public enum eFrequence
    {
        eDaily = 0,
        eWeekly = 1,
        eMonthly = 2
    }

    public enum eWeekFrequence 
    {
        eFirstDayOfWeek = 0,
        eLastDayOfWeek = 1,
        eSpecificWeekDayOfWeek = 2
    }
    
    public enum eMonthFrequence 
    {
        eFirstDayOfMonth = 0,
        eLastDayOfMonth = 1,
        eSpecificDayOfMonth = 2
    }


    public class Settings
    {
        public Guid Id { get; set; }
        public String CsvFile { get; set; }
        public eFrequence Frequence { get; set; }
        public eMonthFrequence MonthFrequence { get; set; }
        public eWeekFrequence WeekFrequence { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public int DayOfMonth { get; set; }
        public String DeadlineField { get; set; }
        public String TextField { get; set; }
        public String EmailField { get; set; }
        public Boolean HasColumnHeaders { get; set; }

        public string EmailSubject { get; set; }

        public string Email { get; set; }
    }
}
