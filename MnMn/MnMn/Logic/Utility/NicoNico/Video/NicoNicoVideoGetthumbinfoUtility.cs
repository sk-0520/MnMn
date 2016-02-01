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
using ContentTypeTextNet.MnMn.MnMn.Define.NicoNico.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.NicoNico.Video.Raw;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Utility.NicoNico.Video
{
    public static class NicoNicoVideoGetthumbinfoUtility
    {
        #region function

        /// <summary>
        /// サムネイル情報取得のステータス確認。
        /// </summary>
        /// <param name="rawModel"></param>
        /// <returns></returns>
        public static bool IsSuccessResponse(RawNicoNicoVideoThumbResponseModel rawModel)
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

        public static NicoNicoVideoMovieType ConvertMovieType(string s)
        {
            NicoNicoVideoMovieType result;
            if(Enum.TryParse<NicoNicoVideoMovieType>(s, out result)) {
                return result;
            }

            return NicoNicoVideoMovieType.Unknown;
        }

        public static NicoNicoVideoThumbType ConvertThumbType(string s)
        {
            NicoNicoVideoThumbType result;
            if(Enum.TryParse<NicoNicoVideoThumbType>(s, out result)) {
                return result;
            }

            return NicoNicoVideoThumbType.Unknown;
        }

        public static bool IsEmbeddable(string s)
        {
            return RawValueUtility.ConvertBoolean(s);
        }

        public static bool IsLivePlay(string s)
        {
            return !RawValueUtility.ConvertBoolean(s);
        }

        public static bool IsCategory(RawNicoNicoVideoTagItemModel tag)
        {
            return RawValueUtility.ConvertBoolean(tag.Category);
        }

        public static bool IsLocked(RawNicoNicoVideoTagItemModel tag)
        {
            return RawValueUtility.ConvertBoolean(tag.Lock);
        }

        #endregion

    }
}
