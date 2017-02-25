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
    public class RawSmileLiveGetPlayerStatusMsModel: ModelBase
    {
        #region property

        [XmlElement("addr")]
        public string Addr { get; set; }
        [XmlElement("port")]
        public string Port { get; set; }
        [XmlElement("thread")]
        public string Thread { get; set; }

        #endregion
    }
}
