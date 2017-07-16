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
    public class RawSmileVideoMsgPingModel : ModelBase
    {
        #region property

        [DataMember(Name = "content")]
        public string Content { get; set; }

        #endregion
    }
}
