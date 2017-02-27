/*
This file is part of MnMn.

MnMn is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

MnMn is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with MnMn.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Media;
using ContentTypeTextNet.MnMn.MnMn.Logic.Attribute;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Utility
{
    public static class RawValueUtility
    {
        #region define

        delegate bool ConvertTryPaese<T>(string s, out T result);

        static readonly DateTime unixTimeBase = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);

        #endregion

        #region property

        public static int UnknownInteger { get; } = int.MinValue;
        public static long UnknownLong { get; } = long.MinValue;
        public static DateTime UnknownDateTime { get; } = DateTime.MinValue;
        public static TimeSpan UnknownTimeSpan { get; } = TimeSpan.MinValue;

        public static string Rss2DateTimeFormat { get; } = "ddd, d MMM yyyy HH:mm:ss K";
        public static CultureInfo Rss2DateTimeCultureInfo { get; } = CultureInfo.CreateSpecificCulture("en-US");

        #endregion

        #region function

        static T ConvertT<T>(string s, ConvertTryPaese<T> parse, T resultFailValue)
        {
            T result;
            if(parse(s, out result)) {
                return result;
            } else {
                return resultFailValue;
            }
        }

        public static int ConvertInteger(string s)
        {
            if(s == null) {
                return UnknownInteger;
            }
            if(s.Any(c => c == ',')) {
                s = string.Concat(s.Where(c => c != ','));
            }

            return ConvertT(s, int.TryParse, UnknownInteger);
        }

        public static long ConvertLong(string s)
        {
            if(s == null) {
                return UnknownLong;
            }
            if(s.Any(c => c == ',')) {
                s = string.Concat(s.Where(c => c != ','));
            }

            return ConvertT(s, long.TryParse, UnknownLong);
        }

        public static DateTime ConvertDateTime(string s)
        {
            if(string.IsNullOrEmpty(s)) {
                return UnknownDateTime;
            }

            var trimedValue = s
                .Replace("年", "/")
                .Replace("月", "/")
                .Replace("日", "")
                .Replace("：", ":")
                .Trim()
            ;
            return ConvertT(trimedValue, DateTime.TryParse, UnknownDateTime);
        }

        public static TimeSpan ConvertTimeSpan(string s)
        {
            return ConvertT(s, TimeSpan.TryParse, UnknownTimeSpan);
        }

        public static Uri ConvertUri(string s)
        {
            Uri result;
            if(Uri.TryCreate(s, UriKind.RelativeOrAbsolute, out result)) {
                return result;
            } else {
                return null;
            }
        }

        public static bool ConvertBoolean(string s)
        {
            if(string.IsNullOrEmpty(s)) {
                return false;
            }

            if(s.Length == 1) {
                if(s[0] == '1') {
                    return true;
                } else {
                    return false;
                }
            }
            return Convert.ToBoolean(s);
        }

        public static TResultModel ConvertNameModelFromWWWFormData<TResultModel>(string rawWwwFormData)
            where TResultModel : new()
        {
            var rawParameters = HttpUtility.ParseQueryString(rawWwwFormData);
            var parameters = rawParameters.AllKeys
                .ToDictionary(k => k, k => rawParameters.GetValues(k).First())
            ;
            var result = new TResultModel();
            var map = NameAttributeUtility.GetNames(result);
            foreach(var pair in map) {
                string value;
                if(parameters.TryGetValue(pair.Key, out value)) {
                    pair.Value.SetValue(result, value);
                }
            }

            return result;
        }

        /// <summary>
        /// Unix時間(数値)を<see cref="DateTime"/>に変換。
        /// </summary>
        /// <param name="unixTime"></param>
        /// <returns></returns>
        public static DateTime ConvertUnixTime(long unixTime)
        {
            return unixTimeBase.AddSeconds(unixTime).ToLocalTime();
        }
        /// <summary>
        /// Unix時間(文字列)を<see cref="DateTime"/>に変換。
        /// </summary>
        /// <param name="unixTime"></param>
        /// <returns></returns>
        public static DateTime ConvertUnixTime(string unixTime)
        {
            return ConvertUnixTime(ConvertLong(unixTime));
        }
        /// <summary>
        /// ミリ秒付Unix時間を<see cref="DateTime"/>に変換。
        /// </summary>
        /// <param name="unixTime">[Unix時間][ミリ秒]形式。</param>
        /// <param name="width">[ミリ秒]の文字列長。</param>
        /// <returns></returns>
        public static DateTime ConvertUnixTimeWithMilliseconds(string unixTime, int width)
        {
            if(width == 0) {
                return ConvertUnixTime(unixTime);
            }
            var s = unixTime.Substring(0, unixTime.Length - width);
            var ms = unixTime.Substring(s.Length);
            var time = ConvertUnixTime(ConvertLong(s));

            return time.AddMilliseconds(ConvertInteger(ms));
        }

        /// <summary>
        /// <see cref="DateTime"/>からUnix時間(数値)に変換。
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static long ConvertRawUnixTime(DateTime time)
        {
            TimeSpan elapsedTime = time.ToUniversalTime() - unixTimeBase.ToUniversalTime();

            return (long)elapsedTime.TotalSeconds;
        }

        /// <summary>
        /// <see cref="DateTime"/>からミリ秒付Unix時間(文字列)に変換。
        /// </summary>
        /// <param name="time"></param>
        /// <param name="width">ミリ秒の幅。超過した分は切り落とされる</param>
        /// <returns></returns>
        public static string ConvertRawUnixTimeWithMilliseconds(DateTime time, int width)
        {
            var unixSec = ConvertRawUnixTime(time);
            var ms = time.Millisecond.ToString().PadRight(width, '0');
            if(width < ms.Length) {
                ms = ms.Substring(0, width);
            }

            return unixSec + ms;
        }

        public static string ConvertHumanLikeByte(long byteSize, string[] terms)
        {
            double size = byteSize;
            int order = 0;
            while(size >= 1024 && ++order < terms.Length) {
                size = size / 1024;
            }

            return $"{size:0.00} {terms[order]}";
        }

        public static string ConvertHumanLikeByte(long byteSize)
        {
            var terms = new[] { "byte", "KB", "MB", "GB" };
            return ConvertHumanLikeByte(byteSize, terms);
        }

        public static Color ConvertColor(string text)
        {
            if(string.IsNullOrWhiteSpace(text)) {
                return Colors.Transparent;
            }

            return (Color)ColorConverter.ConvertFromString(text);
        }

        #endregion
    }
}
