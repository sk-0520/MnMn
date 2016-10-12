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
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw
{
    [Serializable]
    public class RawSmileVideoDmcSessionModel: ModelBase
    {
        #region property

        [XmlElement("id")]
        public string Id { get; set; }

        [XmlElement("recipe_id")]
        public string RecipeId { get; set; }

        [XmlElement("content_id")]
        public string ContentId { get; set; }

        [XmlArray("content_src_id_sets"), XmlArrayItem("content_src_id_set")]
        public CollectionModel<RawSmileVideoDmcContentSrcIdSetModel> ContentSrcIdSets { get; set; } = new CollectionModel<RawSmileVideoDmcContentSrcIdSetModel>();

        [XmlElement("content_type")]
        public string ContentType { get; set; }

        [XmlElement("timing_constraint")]
        public string TimingConstraint { get; set; }

        [XmlElement("keep_method")]
        public RawSmileVideoDmcKeepMethodModel KeepMethod { get; set; } = new RawSmileVideoDmcKeepMethodModel();

        [XmlElement("protocol")]
        public RawSmileVideoDmcProtocolModel Protocol { get; set; } = new RawSmileVideoDmcProtocolModel();

        [XmlElement("play_seek_time")]
        public string PlaySeekTime { get; set; }

        [XmlElement("play_speed")]
        public string PlaySpeed { get; set; }

        [XmlElement("session_operation_auth")]
        public RawSmileVideoDmcSessionOperationAuthModel OperationAuth { get; set; } = new RawSmileVideoDmcSessionOperationAuthModel();

        [XmlElement("content_auth")]
        public RawSmileVideoDmcContentAuthModel ContentAuth { get; set; } = new RawSmileVideoDmcContentAuthModel();

        [XmlElement("runtime_info")]
        public RawSmileVideoDmcRuntimeInformationModel RuntimeInformation { get; set; } = new RawSmileVideoDmcRuntimeInformationModel();

        [XmlElement("client_info")]
        public RawSmileVideoDmcClientInformationModel ClientInformation { get; set; } = new RawSmileVideoDmcClientInformationModel();

        [XmlElement("created_time")]
        public string CreatedTime { get; set; }

        [XmlElement("modified_time")]
        public string ModifiedTime { get; set; }

        [XmlElement("content_route")]
        public string ContentRoute { get; set; }

        #endregion
    }
}