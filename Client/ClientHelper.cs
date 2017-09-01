using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
namespace Client
{
    static class ClientHelper
    {
        public static bool IsSameWeek(DateTime a,DateTime b)
        {
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            Calendar cal = dfi.Calendar;
            if (a.Year == b.Year && cal.GetWeekOfYear(a, dfi.CalendarWeekRule, dfi.FirstDayOfWeek) == cal.GetWeekOfYear(b, dfi.CalendarWeekRule, dfi.FirstDayOfWeek))
                return true;
            else
                return false;
        }
    }
}
