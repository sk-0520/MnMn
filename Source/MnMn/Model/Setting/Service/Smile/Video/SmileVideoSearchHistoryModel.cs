﻿/*
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

namespace ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video
{
    [Serializable, DataContract]
    public class SmileVideoSearchHistoryModel: SettingModelBase
    {
        #region property

        /// <summary>
        /// 検索クエリ。
        /// </summary>
        [DataMember]
        public string Query { get; set; }
        /// <summary>
        /// 検索方法。
        /// </summary>
        [DataMember]
        public SearchType SearchType { get; set; }
        /// <summary>
        /// 検索結果数。
        /// </summary>
        [DataMember]
        public int TotalCount { get; set; }
        /// <summary>
        /// 最終使用日時。
        /// </summary>
        [DataMember]
        public DateTime LastTimestamp { get; set; }
        /// <summary>
        /// 使用回数。
        /// </summary>
        [DataMember]
        public int Count { get; set; }

        #endregion
    }
}
