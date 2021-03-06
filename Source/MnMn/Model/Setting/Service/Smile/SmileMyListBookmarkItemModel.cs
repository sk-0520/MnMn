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
    public class SmileMyListBookmarkItemModel: SmileMyListItemModel
    {
        #region property

        /// <summary>
        /// 編集した名前。
        /// </summary>
        [DataMember]
        public string MyListCustomName { get; set; }

        /// <summary>
        /// 登録動画。
        /// </summary>
        [DataMember]
        public CollectionModel<string> Videos { get; set; } = new CollectionModel<string>();

        /// <summary>
        /// タグ。
        /// <para>色々試したけど直にカンマ区切りとかのほうが使いやすかった。</para>
        /// </summary>
        [DataMember]
        public string TagNames { get; set; }

        #endregion
    }
}
