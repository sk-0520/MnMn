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
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Define;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Setting
{
    [DataContract]
    public class FilteringItemSettingModel: SettingModelBase
    {
        #region property

        /// <summary>
        /// 有効無効。
        /// </summary>
        [DataMember]
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// フィルタリング方法。
        /// </summary>
        [DataMember]
        public FilteringType Type { get; set; }

        /// <summary>
        /// 大文字小文字を無視するか。
        /// </summary>
        [DataMember]
        public bool IgnoreCase { get; set; }

        /// <summary>
        /// フィルタリング文字列。
        /// </summary>
        [DataMember]
        public string Source { get; set; }

        /// <summary>
        /// フィルタ名。
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        #endregion
    }
}
