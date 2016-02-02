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

namespace ContentTypeTextNet.MnMn.MnMn.Model.NicoNico.Video
{
    /// <summary>
    /// ランキング一覧。
    /// </summary>
    [Serializable, XmlRoot("ranking")]
    public class NicoNicoVideoRankingModel: ModelBase
    {
        [XmlElement("periods")]
        public NicoNicoVideoRankingGroupModel Periods { get; set; } = new NicoNicoVideoRankingGroupModel();

        [XmlElement("targets")]
        public NicoNicoVideoRankingGroupModel Targets { get; set; } = new NicoNicoVideoRankingGroupModel();

        /// <summary>
        /// 各カテゴリの大枠。
        /// </summary>
        [XmlElement("categories")]
        public CollectionModel<NicoNicoVideoCategoryGroupModel> Items { get; set; } = new CollectionModel<NicoNicoVideoCategoryGroupModel>();

    }
}