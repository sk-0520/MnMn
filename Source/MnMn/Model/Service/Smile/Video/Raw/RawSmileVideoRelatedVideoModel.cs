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
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw
{
    [Serializable, XmlRoot("related_video")]
    public class RawSmileVideoRelatedVideoModel:ModelBase
    {
        #region property

        [XmlAttribute("status")]
        public string Status { get; set; }

        [XmlElement("total_count")]
        public string TotalCount { get; set; }

        [XmlElement("page_count")]
        public string PageCount { get; set; }

        [XmlElement("data_count")]
        public string DataCount { get; set; }

        [XmlElement("video")]
        public CollectionModel<RawSmileVideoRelatedVideoItemModel> Videos { get; set; } = new CollectionModel<RawSmileVideoRelatedVideoItemModel>();

        #endregion
    }
}
