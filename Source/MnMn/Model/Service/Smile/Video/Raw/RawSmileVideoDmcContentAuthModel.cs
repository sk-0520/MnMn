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
    public class RawSmileVideoDmcContentAuthModel: ModelBase
    {
        #region property

        [XmlElement("auth_type")]
        public string AuthType { get; set; }

        [XmlElement("max_content_count")]
        public string MaxContentCount { get; set; }

        [XmlElement("content_key_timeout")]
        public string ContentKeyTimeout { get; set; }

        [XmlElement("service_id")]
        public string ServiceId { get; set; }

        [XmlElement("service_user_id")]
        public string ServiceUserId { get; set; }

        [XmlElement("content_auth_info")]
        public RawSmileVideoDmcContentAuthInfoModel Information { get; set; } = new RawSmileVideoDmcContentAuthInfoModel();

        #endregion
    }
}