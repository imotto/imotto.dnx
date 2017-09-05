using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace iMotto.Common
{
    public class Utils
    {
        private static readonly Random rd = new Random((int)DateTime.Now.Ticks);
        private static readonly MD5 md5 = MD5.Create();
        private static readonly Regex mobilePattern = new Regex(@"^1[345789]\d{9}$", RegexOptions.Compiled);

        public static int GetTheDay(DateTime dt)
        {
            return dt.Year * 10000 + dt.Month * 100 + dt.Day;
        }

        public static string HashPassword(string input)
        {
            var bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(string.Format("B150m{0}Pro4s", input)));

            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// 生成随机码
        /// </summary>
        /// <returns></returns>
        public static string GenRandomCode()
        {
            return (rd.Next(9999999)/10).ToString("D6");
        }

        /// <summary>
        /// 生成不带分隔符的大写GUID字符串
        /// </summary>
        /// <returns></returns>
        public static string GenId()
        {
            return Guid.NewGuid().ToString("N").ToUpper();
        }

        /// <summary>
        /// 验证是否合法手机号。
        /// 验证规则：以13、14、15、18开头的11位数字
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static bool IsValidMobile(string mobile)
        {
            if (string.IsNullOrWhiteSpace(mobile))
            {
                return false;
            }

            return mobilePattern.IsMatch(mobile);
        }


        public static bool TryGetDateTime(int theDay, out DateTime dt)
        {
            if (theDay < 29990000 && theDay > 20160401)
            {
                var year = theDay / 10000;
                var month = theDay % 10000 / 100;
                var day = theDay % 100;
                try
                {
                    dt = new DateTime(year, month, day);
                    return true;
                }
                catch
                {
                }
            }

            dt = DateTime.Today;
            return false;
        }

        public static double Hot(int ups, int downs, DateTime date)
        {
            var s = ups - downs + 1;
            var order = Math.Log10(Math.Max(Math.Abs(s), 1));
            var sign = 1;
            if (s < 0)
            {
                sign = -1;
                order = order * sign;
            }
            else if (s == 0)
            {
                sign = 0;
            }

            var offset = (DateTime.Today - date.Date).TotalDays;

            var seconds = (DateTime.Today - date).TotalSeconds;

            return Math.Round(order + sign * (1 - seconds / 86400) * Math.Pow(0.5, offset), 7);
        }

        public static double AblumHot(double score, double weeks, double penalties)
        {
            if (score <= 1)
                return 0;

            return Math.Pow(score - 1, 0.7) / Math.Pow(weeks, 0.7) * penalties;
        }

    }
}
