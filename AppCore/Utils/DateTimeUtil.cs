using System;

namespace AppCore.Utils
{
    // Tarihler için DateTime - string dönüşümlerinin yapıldığı utility class
    public static class DateTimeUtil
    {
        public static DateTime? GetDateFromStringWithTRformat(string date)
        {
            date = date.Trim();
            if (date.Equals(""))
                return null;
            if (!date.Contains("."))
                return null;
            if (date.Split('.').Length != 3)
                return null;
            try
            {
                DateTime result = new DateTime(Convert.ToInt32(date.Split('.')[2]), Convert.ToInt32(date.Split('.')[1]), Convert.ToInt32(date.Split('.')[0]));
                return result;
            }
            catch (Exception exc)
            {
                return null;
            }
        }

        public static DateTime? GetDateFromStringWithTRformat(string date, string time)
        {
            date = date.Trim();
            time = time.Trim();
            if (date.Equals(""))
                return null;
            if (!date.Contains("."))
                return null;
            if (date.Split('.').Length != 3)
                return null;
            if (time.Equals(""))
                return null;
            if (!time.Contains(":"))
                return null;
            if (time.Split(':').Length != 3)
                return null;
            try
            {
                DateTime result = new DateTime(Convert.ToInt32(date.Split('.')[2]), Convert.ToInt32(date.Split('.')[1]), Convert.ToInt32(date.Split('.')[0]),
                    Convert.ToInt32(time.Split(':')[0]), Convert.ToInt32(time.Split(':')[1]), Convert.ToInt32(time.Split(':')[2]));
                return result;
            }
            catch (Exception exc)
            {
                return null;
            }
        }

        public static string GetStringFromDateWithTRformat(DateTime dateTime, bool returnTime = true)
        {
            string day = dateTime.Day.ToString().PadLeft(2, '0');
            string month = dateTime.Month.ToString().PadLeft(2, '0');
            string year = dateTime.Year.ToString().PadLeft(4, '0');
            string hour = dateTime.Hour.ToString().PadLeft(2, '0');
            string minute = dateTime.Minute.ToString().PadLeft(2, '0');
            string second = dateTime.Second.ToString().PadLeft(2, '0');
            if (returnTime)
                return day + "." + month + "." + year + " " + hour + ":" + minute + ":" + second;
            return day + "." + month + "." + year;
        }

        public static DateTime? GetDateFromStringWithENformat(string date)
        {
            date = date.Trim();
            if (date.Equals(""))
                return null;
            if (!date.Contains("/"))
                return null;
            if (date.Split('/').Length != 3)
                return null;
            try
            {
                DateTime result = new DateTime(Convert.ToInt32(date.Split('/')[2]), Convert.ToInt32(date.Split('/')[0]), Convert.ToInt32(date.Split('/')[1]));
                return result;
            }
            catch (Exception exc)
            {
                return null;
            }
        }

        public static DateTime? GetDateFromStringWithENformat(string date, string time)
        {
            date = date.Trim();
            time = time.Trim();
            if (date.Equals(""))
                return null;
            if (!date.Contains("/"))
                return null;
            if (date.Split('/').Length != 3)
                return null;
            if (time.Equals(""))
                return null;
            if (!time.Contains(":"))
                return null;
            if (time.Split(':').Length != 3)
                return null;
            try
            {
                DateTime result = new DateTime(Convert.ToInt32(date.Split('/')[2]), Convert.ToInt32(date.Split('/')[0]), Convert.ToInt32(date.Split('/')[1]),
                    Convert.ToInt32(time.Split(':')[0]), Convert.ToInt32(time.Split(':')[1]), Convert.ToInt32(time.Split(':')[2]));
                return result;
            }
            catch (Exception exc)
            {
                return null;
            }
        }

        public static string GetStringFromDateWithENformat(DateTime dateTime, bool returnTime = true)
        {
            string day = dateTime.Day.ToString().PadLeft(2, '0');
            string month = dateTime.Month.ToString().PadLeft(2, '0');
            string year = dateTime.Year.ToString().PadLeft(4, '0');
            string hour = dateTime.Hour.ToString().PadLeft(2, '0');
            string minute = dateTime.Minute.ToString().PadLeft(2, '0');
            string second = dateTime.Second.ToString().PadLeft(2, '0');
            if (returnTime)
                return month + "/" + day + "/" + year + " " + hour + ":" + minute + ":" + second;
            return month + "/" + day + "/" + year;
        }

        public static DateTime? GetDateFromStringWithSQLformat(string date)
        {
            date = date.Trim();
            if (date.Equals(""))
                return null;
            if (!date.Contains("-"))
                return null;
            if (date.Split('-').Length != 3)
                return null;
            try
            {
                DateTime result = new DateTime(Convert.ToInt32(date.Split('-')[0]), Convert.ToInt32(date.Split('-')[1]), Convert.ToInt32(date.Split('-')[2]));
                return result;
            }
            catch (Exception exc)
            {
                return null;
            }
        }

        public static DateTime? GetDateFromStringWithSQLformat(string date, string time)
        {
            date = date.Trim();
            time = time.Trim();
            if (date.Equals(""))
                return null;
            if (!date.Contains("-"))
                return null;
            if (date.Split('-').Length != 3)
                return null;
            if (time.Equals(""))
                return null;
            if (!time.Contains(":"))
                return null;
            if (time.Split(':').Length != 3)
                return null;
            try
            {
                DateTime result = new DateTime(Convert.ToInt32(date.Split('-')[0]), Convert.ToInt32(date.Split('-')[1]), Convert.ToInt32(date.Split('-')[2]),
                    Convert.ToInt32(time.Split(':')[0]), Convert.ToInt32(time.Split(':')[1]), Convert.ToInt32(time.Split(':')[2]));
                return result;
            }
            catch (Exception exc)
            {
                return null;
            }
        }

        public static string GetStringFromDateWithSQLformat(DateTime dateTime, bool returnTime = true)
        {
            string day = dateTime.Day.ToString().PadLeft(2, '0');
            string month = dateTime.Month.ToString().PadLeft(2, '0');
            string year = dateTime.Year.ToString().PadLeft(4, '0');
            string hour = dateTime.Hour.ToString().PadLeft(2, '0');
            string minute = dateTime.Minute.ToString().PadLeft(2, '0');
            string second = dateTime.Second.ToString().PadLeft(2, '0');
            if (returnTime)
                return year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + second;
            return year + "-" + month + "-" + day;
        }

        public static string GetStringFromDateWithSQLformat(DateTime dateTime, string dateTimeSeperator)
        {
            string day = dateTime.Day.ToString().PadLeft(2, '0');
            string month = dateTime.Month.ToString().PadLeft(2, '0');
            string year = dateTime.Year.ToString().PadLeft(4, '0');
            string hour = dateTime.Hour.ToString().PadLeft(2, '0');
            string minute = dateTime.Minute.ToString().PadLeft(2, '0');
            string second = dateTime.Second.ToString().PadLeft(2, '0');
            return year + "-" + month + "-" + day + dateTimeSeperator + hour + ":" + minute + ":" + second;
        }

        public static DateTime AddTimeToDate(DateTime date, int hours = 0, int minutes = 0, int seconds = 0)
        {
            date = date.AddHours(hours);
            date = date.AddMinutes(minutes);
            date = date.AddSeconds(seconds);
            return date;
        }

        public static DateTime? GetDateFromStringWithYYmmDDFormat(string date, string yearPrefix = "20")
        {
            if (String.IsNullOrWhiteSpace(date))
                return null;
            string year = date.Substring(0, 2);
            string month = date.Substring(2, 2);
            string day = date.Substring(4, 2);
            try
            {
                DateTime result = new DateTime(Convert.ToInt32(yearPrefix + year), Convert.ToInt32(month), Convert.ToInt32(day));
                return result;
            }
            catch (Exception exc)
            {
                return null;
            }
        }

        public static string GetStringWithYYmmDDFormatFromDate(DateTime dateTime)
        {
            string day = dateTime.Day.ToString().PadLeft(2, '0');
            string month = dateTime.Month.ToString().PadLeft(2, '0');
            string year = dateTime.Year.ToString().Substring(2, 2);
            return year + month + day;
        }
    }
}
