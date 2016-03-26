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
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video
{
    /// <summary>
    /// 動画個別設定。
    /// </summary>
    [DataContract]
    public class SmileVideoIndividualVideoSettingModel: SettingModelBase
    {
        /// <summary>
        /// 通常画質読み込み済み。
        /// </summary>
        [DataMember]
        public bool LoadedNormal { get; set; } = false;
        /// <summary>
        /// エコノミー読み込み済み。
        /// </summary>
        [DataMember]
        public bool LoadedEconomyMode { get; set; } = false;

        [DataMember]
        public bool ConvertedSwf { get; set; } = false;

        /// <summary>
        /// フィルタリングデータ。
        /// </summary>
        [DataMember]
        public CollectionModel<SmileVideoFilteringSettingModel> FilteringItems { get; set; } = new CollectionModel<SmileVideoFilteringSettingModel>();

    }
}
