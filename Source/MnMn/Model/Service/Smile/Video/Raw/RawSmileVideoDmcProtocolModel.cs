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
    public class RawSmileVideoDmcProtocolModel: ModelBase
    {
        #region property

        [XmlElement("name"), DataMember(Name = "name")]
        public string Name { get; set; }

        [XmlArray("parameters"), XmlArrayItem("http_parameters"), DataMember(Name = "parameters")]
        public CollectionModel<RawSmileVideoDmcProtocolParameterModel> HttpParameters { get; set; } = new CollectionModel<RawSmileVideoDmcProtocolParameterModel>();

        #endregion
    }
}