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

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Utility
{
    public static class RawValueUtility
    {
        #region define

        delegate bool ConvertTryPaese<T>(string s, out T result);

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
            return ConvertT(s, int.TryParse, UnknownInteger);
        }

        public static long ConvertLong(string s)
        {
            return ConvertT(s, long.TryParse, UnknownLong);
        }

        public static DateTime ConvertDateTime(string s)
        {
            return ConvertT(s, DateTime.TryParse, UnknownDateTime);
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
            return Convert.ToBoolean(s);
        }

        #endregion
    }
}
