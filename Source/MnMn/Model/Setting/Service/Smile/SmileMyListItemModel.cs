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
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile
{
    [Serializable, DataContract]
    public class SmileMyListItemModel: ModelBase
    {
        #region property

        /// <summary>
        /// マイリストID。
        /// </summary>
        [DataMember]
        public string MyListId { get; set; }

        /// <summary>
        /// マイリスト名。
        /// </summary>
        [DataMember]
        public string MyListName { get; set; }

        /// <summary>
        /// 更新日時(作成日時)。
        /// </summary>
        [DataMember]
        public DateTime UpdateTimestamp { get; set; }

        #endregion
    }
}
