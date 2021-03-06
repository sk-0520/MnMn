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
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video
{
    /// <summary>
    /// ランキング一覧。
    /// </summary>
    [Serializable, XmlRoot("ranking")]
    public class SmileVideoRankingModel: ModelBase, IReadOnlySmileVideoRanking
    {
        #region IReadOnlySmileVideoRanking

        [XmlElement("periods")]
        public SmileVideoRankingGroupModel Periods { get; set; } = new SmileVideoRankingGroupModel();
        IReadOnlySmileVideoRankingGroup IReadOnlySmileVideoRanking.Periods => Periods;

        [XmlElement("targets")]
        public SmileVideoRankingGroupModel Targets { get; set; } = new SmileVideoRankingGroupModel();
        IReadOnlySmileVideoRankingGroup IReadOnlySmileVideoRanking.Targets => Targets;

        /// <summary>
        /// 各カテゴリの大枠。
        /// </summary>
        [XmlElement("categories")]
        public CollectionModel<SmileVideoCategoryGroupModel> Items { get; set; } = new CollectionModel<SmileVideoCategoryGroupModel>();
        IReadOnlyList<IReadOnlySmileVideoCategoryGroup> IReadOnlySmileVideoRanking.Items => Items;

        #endregion
    }
}
