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
using ContentTypeTextNet.MnMn.MnMn.Define;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video
{
    [DataContract]
    public class SmileVideoSettingModel: SettingModelBase
    {
        #region property

        /// <summary>
        /// コメント設定。
        /// </summary>
        [DataMember]
        public SmileVideoCommentSettingModel Comment { get; set; } = new SmileVideoCommentSettingModel();

        [DataMember]
        public SmileVideoSearchSettingModel Search { get; set; } = new SmileVideoSearchSettingModel();

        [DataMember]
        public SmileVideoPlayerSettingModel Player { get; set; } = new SmileVideoPlayerSettingModel();

        [DataMember]
        public SmileVideoRankingSettingModel Ranking { get; set; } = new SmileVideoRankingSettingModel();

        [DataMember]
        public FixedSizeCollectionModel<SmileVideoPlayHistoryModel> History { get; set; } = new FixedSizeCollectionModel<SmileVideoPlayHistoryModel>(100, false);

        #endregion
    }
}
