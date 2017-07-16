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
using System.Runtime.Serialization;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw
{
    [Serializable, DataContract]
    public class RawSmileVideoDmcSessionModel: ModelBase
    {
        #region property

        [XmlElement("service_id"), DataMember(Name = "service_id")]
        public string ServiceId { get; set; }

        [XmlElement("id"), DataMember(Name ="id")]
        public string Id { get; set; }

        [XmlElement("recipe_id"), DataMember(Name = "recipe_id")]
        public string RecipeId { get; set; }

        [XmlElement("content_id"), DataMember(Name = "content_id")]
        public string ContentId { get; set; }

        [XmlArray("content_src_id_sets"), XmlArrayItem("content_src_id_set"), DataMember(Name = "content_src_id_sets")]
        public CollectionModel<RawSmileVideoDmcContentSrcIdSetModel> ContentSrcIdSets { get; set; } = new CollectionModel<RawSmileVideoDmcContentSrcIdSetModel>();

        [XmlElement("content_type"), DataMember(Name = "content_type")]
        public string ContentType { get; set; }

        [XmlElement("timing_constraint"), DataMember(Name = "timing_constraint")]
        public string TimingConstraint { get; set; }

        [XmlElement("keep_method"), DataMember(Name = "keep_method")]
        public RawSmileVideoDmcKeepMethodModel KeepMethod { get; set; } = new RawSmileVideoDmcKeepMethodModel();

        [XmlElement("protocol"), DataMember(Name = "protocol")]
        public RawSmileVideoDmcProtocolModel Protocol { get; set; } = new RawSmileVideoDmcProtocolModel();

        [XmlElement("play_seek_time"), DataMember(Name = "play_seek_time")]
        public string PlaySeekTime { get; set; }

        [XmlElement("play_speed"), DataMember(Name = "play_speed")]
        public string PlaySpeed { get; set; }

        [XmlElement("content_uri"), DataMember(Name = "content_uri")]
        public string ContentUri { get; set; }

        [XmlElement("session_operation_auth"), DataMember(Name = "session_operation_auth")]
        public RawSmileVideoDmcSessionOperationAuthModel OperationAuth { get; set; } = new RawSmileVideoDmcSessionOperationAuthModel();

        [XmlElement("content_auth"), DataMember(Name = "content_auth")]
        public RawSmileVideoDmcContentAuthModel ContentAuth { get; set; } = new RawSmileVideoDmcContentAuthModel();

        [XmlElement("runtime_info"), DataMember(Name = "runtime_info")]
        public RawSmileVideoDmcRuntimeInformationModel RuntimeInformation { get; set; } = new RawSmileVideoDmcRuntimeInformationModel();

        [XmlElement("client_info"), DataMember(Name = "client_info")]
        public RawSmileVideoDmcClientInformationModel ClientInformation { get; set; } = new RawSmileVideoDmcClientInformationModel();

        [XmlElement("created_time"), DataMember(Name = "created_time")]
        public string CreatedTime { get; set; }

        [XmlElement("modified_time"), DataMember(Name = "modified_time")]
        public string ModifiedTime { get; set; }

        [XmlElement("priority"), DataMember(Name = "priority")]
        public string Priority { get; set; }

        [XmlElement("content_route"), DataMember(Name = "content_route")]
        public string ContentRoute { get; set; }

        [XmlElement("version"), DataMember(Name = "version")]
        public string Version { get; set; }

        #endregion
    }
}