using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw
{
    [Serializable]
    public class RawSmileVideoDmcMetaModel: ModelBase
    {
        #region property

        [XmlAttribute("message")]
        public string Message { get; set; }

        [XmlAttribute("status")]
        public string Status { get; set; }

        #endregion
    }
}
