using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Live.Raw
{
    [Serializable, XmlRoot("getplayerstatus")]
    public class RawSmileLiveGetPlayerStatusModel: ModelBase
    {
        #region property

        [XmlAttribute("status")]
        public string Status { get; set; }

        [XmlAttribute("time")]
        public string Time { get; set; }

        [XmlElement("error")]
        public RawSmileLiveGetPlayerStatusErrorModel Error { get; set; } = new RawSmileLiveGetPlayerStatusErrorModel();

        [XmlElement("stream")]
        public RawSmileLiveGetPlayerStatusStreamModel Stream { get; set; } = new RawSmileLiveGetPlayerStatusStreamModel();

        [XmlElement("user")]
        public RawSmileLiveGetPlayerStatusUserModel User { get; set; } = new RawSmileLiveGetPlayerStatusUserModel();

        [XmlElement("rtmp")]
        public RawSmileLiveGetPlayerStatusRtmpModel Rtmp { get; set; } = new RawSmileLiveGetPlayerStatusRtmpModel();

        [XmlElement("ms")]
        public RawSmileLiveGetPlayerStatusMsModel Ms { get; set; } = new RawSmileLiveGetPlayerStatusMsModel();

        #endregion
    }
}
