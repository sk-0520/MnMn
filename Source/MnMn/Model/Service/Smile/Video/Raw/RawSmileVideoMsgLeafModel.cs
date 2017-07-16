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
    public class RawSmileVideoMsgLeafModel : ModelBase
    {
        #region property

        [DataMember(Name = "thread")]
        public string Thread { get; set; }

        [DataMember(Name = "leaf")]
        public string Leaf { get; set; }

        [DataMember(Name = "count")]
        public string Count { get; set; }

        #endregion
    }
}
