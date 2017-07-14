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
using System.Runtime.Serialization;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw
{
    [Serializable]
    public class RawSmileVideoDmcSrcIdToMultiplexerModel: ModelBase
    {
        #region property

        [XmlArray("video_src_ids"), XmlArrayItem("string"), DataMember(Name = "video_src_ids")]
        public CollectionModel<string> VideoSrcIds { get; set; } = new CollectionModel<string>();

        [XmlArray("audio_src_ids"), XmlArrayItem("string"), DataMember(Name = "audio_src_ids")]
        public CollectionModel<string> AudioSrcIds { get; set; } = new CollectionModel<string>();

        #endregion
    }
}