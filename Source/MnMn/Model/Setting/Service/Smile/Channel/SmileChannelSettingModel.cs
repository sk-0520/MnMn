using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Channel
{
    [DataContract]
    public class SmileChannelSettingModel: SettingModelBase
    {
        #region property

        [DataMember]
        public double GroupWidth { get; set; } = Constants.SettingServiceSmileChannelGroupAreaStar;
        [DataMember]
        public double ItemsWidth { get; set; } = Constants.SettingServiceSmileChannelItemsAreaStar;

        #endregion
    }
}
