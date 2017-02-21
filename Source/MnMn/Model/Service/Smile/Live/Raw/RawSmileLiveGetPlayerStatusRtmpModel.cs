using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Live.Raw
{
    [Serializable]
    public class RawSmileLiveGetPlayerStatusRtmpModel:ModelBase
    {
        #region property

        [XmlAttribute("is_fms")]
        public string IsFms { get; set; }

        [XmlAttribute("rtmpt_port")]
        public string RtmptPort { get; set; }

        [XmlElement("url")]
        public string Url { get; set; }

        [XmlElement("ticket")]
        public string Ticket { get; set; }

        #endregion
    }
}
