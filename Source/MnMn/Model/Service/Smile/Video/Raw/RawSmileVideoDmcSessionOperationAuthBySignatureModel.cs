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
    [Serializable, DataContract]
    public class RawSmileVideoDmcSessionOperationAuthBySignatureModel: ModelBase
    {
        #region property

        [XmlElement("created_time"), DataMember(Name = "created_time")]
        public string CreatedTime { get; set; }

        [XmlElement("expire_time"), DataMember(Name = "expire_time")]
        public string ExpireTime { get; set; }

        [XmlElement("token"), DataMember(Name = "token")]
        public string Token { get; set; }

        [XmlElement("signature"), DataMember(Name = "signature")]
        public string Signature { get; set; }

        #endregion
    }
}