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

namespace ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video
{
    [Serializable, DataContract]
    public class SmileVideoDmcItemModel: ModelBase
    {
        /// <summary>
        /// 読み込み完了済みか。
        /// <para>たぶん使うことはないと思う。</para>
        /// </summary>
        [DataMember]
        public bool IsLoaded { get; set; }
        /// <summary>
        /// 動画サイズ。
        /// <para>HTTPという性善説に基づいていてニコニコというデータ提供先を信じ切るという世は信頼で成り立つ素晴らしい情報！</para>
        /// </summary>
        [DataMember]
        public long Length { get; set; }
        /// <summary>
        /// キーに依存しない動画の品質。
        /// </summary>
        [DataMember]
        public string Video { get; set; }
        /// <summary>
        /// キーに依存しない音声の品質。
        /// </summary>
        [DataMember]
        public string Audio { get; set; }
    }
}
