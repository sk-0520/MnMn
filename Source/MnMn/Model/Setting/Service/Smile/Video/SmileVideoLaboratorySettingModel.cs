using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video
{
    [DataContract]
    public class SmileVideoLaboratorySettingModel: SettingModelBase
    {
        #region property

        [DataMember]
        public string PlayInputVideoSourceFilePath { get; set; } 

        [DataMember]
        public string PlayInputMessageMessageFilePath { get; set; } 

        [DataMember]
        public SmileVideoPlayerSettingModel Player { get; set; } = new SmileVideoPlayerSettingModel();

        #endregion
    }
}
