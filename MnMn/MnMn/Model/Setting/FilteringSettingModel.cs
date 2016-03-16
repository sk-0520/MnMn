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
using ContentTypeTextNet.MnMn.MnMn.Define;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Setting
{
    public class FilteringSettingModel: SettingModelBase
    {
        #region property

        /// <summary>
        /// フィルタリング方法。
        /// </summary>
        public FilteringType FilteringType { get; set; }

        /// <summary>
        /// 大文字小文字を無視するか。
        /// </summary>
        public bool IgnoreCase { get; set; }

        /// <summary>
        /// 全角半角を無視するか。
        /// </summary>
        public bool IgnoreWidth { get; set; }

        /// <summary>
        /// フィルタリング文字列。
        /// </summary>
        public string Source { get; set; }

        #endregion
    }
}
