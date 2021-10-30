using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace StorePanel.Infrastructure.ExtensionMethods
{
    public static class DateExtensionMethods
    {
        private static readonly long DatetimeMinTimeTicks =
            (new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).Ticks;

        public static long ToJavaScriptMilliseconds(this DateTime dt)
        {
            return (long)((dt.ToUniversalTime().Ticks - DatetimeMinTimeTicks) / 10000);
        }
        public static string EnglishNumberToPersian(this string persianStr)
        {
            Dictionary<char, char> LettersDictionary = new Dictionary<char, char>
            {
                ['۰'] = '0',
                ['۱'] = '1',
                ['۲'] = '2',
                ['۳'] = '3',
                ['۴'] = '4',
                ['۵'] = '5',
                ['۶'] = '6',
                ['۷'] = '7',
                ['۸'] = '8',
                ['۹'] = '9'
            };
            foreach (var item in persianStr)
            {
                try
                {
                    persianStr = persianStr.Replace(LettersDictionary[item], item);
                }
                catch (Exception)
                {
                    return persianStr;
                }
            }
            return persianStr;
        }

        public static string ToPersianDateStr(this string dateStr)
        {
            var dateArr = dateStr.Split('/'); //format "YYYY/MM/DD"
            var year = EnglishNumberToPersian(dateArr[0]);
            var month = EnglishNumberToPersian(dateArr[1]);
            var day = EnglishNumberToPersian(dateArr[2]);
            var newStr = $"{year}/{month}/{day}";
            return newStr;
        }
        public static string PersianNumberToEnglish(this string persianStr)
        {
            Dictionary<char, char> LettersDictionary = new Dictionary<char, char>
            {
                ['۰'] = '0',
                ['۱'] = '1',
                ['۲'] = '2',
                ['۳'] = '3',
                ['۴'] = '4',
                ['۵'] = '5',
                ['۶'] = '6',
                ['۷'] = '7',
                ['۸'] = '8',
                ['۹'] = '9'
            };
            foreach (var item in persianStr)
            {
                try
                {
                    persianStr = persianStr.Replace(item, LettersDictionary[item]);
                }
                catch (Exception e)
                {
                    return persianStr;
                }
            }
            return persianStr;
        }
        public static DateTime PersianDateToEnglishDate(this string dateStr)
        {
            var dateArr = dateStr.Split('/'); //format "YYYY/MM/DD"
            var pCalender = new PersianCalendar();
            DateTime dt = new DateTime(Convert.ToInt32(PersianNumberToEnglish(dateArr[0])), Convert.ToInt32(PersianNumberToEnglish(dateArr[1])), Convert.ToInt32(PersianNumberToEnglish(dateArr[2])), pCalender);
            return dt;
        }
    }
}
