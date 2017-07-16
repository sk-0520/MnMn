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
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw
{
    [Serializable, DataContract]
    public class RawSmileVideoMsgChatModel: ModelBase
    {
        [XmlAttribute("thread"), DataMember(Name = "thread")]
        public string Thread { get; set; }
        [XmlAttribute("anonymity"), DataMember(Name = "anonymity")]
        public string Anonymity { get; set; }
        [XmlAttribute("mail"), DataMember(Name = "mail")]
        public string Mail { get; set; }
        [XmlAttribute("date"), DataMember(Name = "date")]
        public string Date { get; set; }
        [XmlAttribute("vpos"), DataMember(Name = "vpos")]
        public string VPos { get; set; }
        [XmlAttribute("no"), DataMember(Name = "no")]
        public string No { get; set; }
        [XmlAttribute("user_id"), DataMember(Name = "user_id")]
        public string UserId { get; set; }
        [XmlAttribute("premium"), DataMember(Name = "premium")]
        public string Premium { get; set; }
        [XmlAttribute("score"), DataMember(Name = "score")]
        public string Score { get; set; }
        [XmlAttribute("fork"), DataMember(Name = "fork")]
        public string Fork { get; set; }
        [XmlText, DataMember(Name = "content")]
        public string Content { get; set; }
    }
}
