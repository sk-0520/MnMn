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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video
{
    public static class SmileVideoMsgUtility
    {
        public static SmileVideoUserKind ConvertUserKind(string s)
        {
            if(s == null) {
                return SmileVideoUserKind.Noraml;
            }

            var map = new Dictionary<string, SmileVideoUserKind>() {
                { "0", SmileVideoUserKind.Noraml },
                { "1", SmileVideoUserKind.Premium },
                { "2", SmileVideoUserKind.Alert },
                { "3", SmileVideoUserKind.Real },
                { "6", SmileVideoUserKind.Official },
            };

            SmileVideoUserKind result;
            if(map.TryGetValue(s, out result)) {
                return result;
            }

            return SmileVideoUserKind.Noraml;
        }

        /// <summary>
        /// 1/100秒に変換。
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static TimeSpan ConvertElapsedTime(string s)
        {
            var time = RawValueUtility.ConvertLong(s);
            return TimeSpan.FromMilliseconds(time * 10);
        }
    }
}
