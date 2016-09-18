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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.Define;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Utility
{
    public static class AppUtility
    {
        #region function

        public static string ReplaceString(string s, IReadOnlyDictionary<string, string> map)
        {
            return s.ReplaceRangeFromDictionary("${", "}", (Dictionary<string, string>)map);
        }

        /// <summary>
        /// 現在の言語を取得。
        /// </summary>
        /// <returns></returns>
        public static string GetCultureLanguage()
        {
            var culture = CultureInfo.CurrentCulture;
            if(culture.IsNeutralCulture) {
                return culture.Name;
            } else {
                return culture.Parent.Name;
            }
        }

        /// <summary>
        /// 現在の国を取得。
        /// </summary>
        /// <returns></returns>
        public static string GetCultureCountry()
        {
            return RegionInfo.CurrentRegion.TwoLetterISORegionName;
        }

        /// <summary>
        /// 現在のカルチャ名を取得。
        /// </summary>
        /// <returns>言語/国</returns>
        public static string GetCultureName()
        {
            return CultureInfo.CurrentCulture.Name;
        }

        #endregion
    }
}
