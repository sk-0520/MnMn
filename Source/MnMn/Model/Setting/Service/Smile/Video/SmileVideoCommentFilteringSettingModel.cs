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
    public class SmileVideoCommentFilteringSettingModel: SettingModelBase
    {
        /// <summary>
        /// フィルタリングデータ。
        /// </summary>
        [DataMember]
        public CollectionModel<SmileVideoCommentFilteringItemSettingModel> Items { get; set; } = new CollectionModel<SmileVideoCommentFilteringItemSettingModel>();

        /// <summary>
        /// 重複したコメントを無視するか。
        /// </summary>
        [DataMember]
        public bool IgnoreOverlapWord { get; set; } = Constants.SettingServiceSmileVideoFilteringIgnoreOverlapWord;

        /// <summary>
        /// <see cref="IgnoreOverlapWord"/>が有効な際の重複判定時間。
        /// </summary>
        [DataMember]
        public TimeSpan IgnoreOverlapTime { get; set; } = Constants.SettingServiceSmileVideoFilteringIgnoreOverlapTime;

        [DataMember]
        public CollectionModel<string> DefineKeys { get; set; } = new CollectionModel<string>();
    }
}
