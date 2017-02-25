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
    public class RawSmileLiveGetPlayerStatusStreamTelopModel:ModelBase
    {
        #region property

        [XmlElement("enabled")]
        public string Enabled { get; set; }

        #endregion
    }
}
