using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Setting
{
    [Serializable]
    public class NetworkUserAgentSettingModel: IReadOnlyUserAgent
    {
        #region IReadOnlyUserAgent

        [DataMember]
        public bool UsingCustomUserAgent { get; set; }

        [DataMember]
        public string CustomUserAgentFormat { get; set; }

        #endregion
    }
}
