using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video
{
    [DataContract]
    public class SmileVideoCommonSettingModel: SettingModelBase
    {
        #region property

        [DataMember]
        public string CustomCopyFormat { get; set; }

        #endregion
    }
}
