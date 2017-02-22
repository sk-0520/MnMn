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
    public class RawSmileLiveGetPlayerStatusStreamContentModel: ModelBase
    {
        #region property

        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("disableAudio")]
        public string DisableAudio { get; set; }

        [XmlAttribute("disableVideo")]
        public string DisableVideo { get; set; }

        [XmlAttribute("start_time")]
        public string StartTime { get; set; }

        [XmlText]
        public string Url { get; set; }

        #endregion
    }
}
