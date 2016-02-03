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
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video
{
    public static class SmileVideoGetthumbinfoUtility
    {
        #region function

        /// <summary>
        /// サムネイル情報取得のステータス確認。
        /// </summary>
        /// <param name="rawModel"></param>
        /// <returns></returns>
        public static bool IsSuccessResponse(RawSmileVideoThumbResponseModel rawModel)
        {
            return string.Compare(rawModel.Status.Trim(), "ok", true) == 0;
        }

        public static TimeSpan ConvertTimeSpan(string s)
        {
            var result = RawValueUtility.ConvertTimeSpan(s);
            if(result != RawValueUtility.UnknownTimeSpan) {
                return result;
            }

            // mmm:ss形式を考慮
            var values = s.Split(':');
            if(values.Length != 2) {
                return RawValueUtility.UnknownTimeSpan;
            }
            var mm = RawValueUtility.ConvertInteger(values[0]);
            if(mm == RawValueUtility.UnknownInteger) {
                return RawValueUtility.UnknownTimeSpan;
            }
            var ss = RawValueUtility.ConvertInteger(values[1]);
            if(ss == RawValueUtility.UnknownInteger) {
                return RawValueUtility.UnknownTimeSpan;
            }
            var hh = mm % 60;

            return new TimeSpan(hh, mm - (hh * 60), ss);
        }

        public static SmileVideoMovieType ConvertMovieType(string s)
        {
            SmileVideoMovieType result;
            if(Enum.TryParse<SmileVideoMovieType>(s, out result)) {
                return result;
            }

            return SmileVideoMovieType.Unknown;
        }

        public static SmileVideoThumbType ConvertThumbType(string s)
        {
            SmileVideoThumbType result;
            if(Enum.TryParse<SmileVideoThumbType>(s, out result)) {
                return result;
            }

            return SmileVideoThumbType.Unknown;
        }

        public static bool IsEmbeddable(string s)
        {
            return RawValueUtility.ConvertBoolean(s);
        }

        public static bool IsLivePlay(string s)
        {
            return !RawValueUtility.ConvertBoolean(s);
        }

        public static bool IsCategory(RawSmileVideoTagItemModel tag)
        {
            return RawValueUtility.ConvertBoolean(tag.Category);
        }

        public static bool IsLocked(RawSmileVideoTagItemModel tag)
        {
            return RawValueUtility.ConvertBoolean(tag.Lock);
        }

        #endregion

    }
}
