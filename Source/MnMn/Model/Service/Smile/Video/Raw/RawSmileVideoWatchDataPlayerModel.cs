using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw
{
    [Serializable, DataContract]
    public class RawSmileVideoWatchDataPlayerModel : ModelBase
    {
        #region property

        [DataMember(Name = "playerInfoXMLUpdateTIme")]
        public string PlayerInfoXMLUpdateTIme { get; set; }
        [DataMember(Name = "isContinuous")]
        public string IsContinuous { get; set; }

        #endregion
    }
}
