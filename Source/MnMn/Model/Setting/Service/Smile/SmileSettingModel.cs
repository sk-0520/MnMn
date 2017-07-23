/*
This file is part of MnMn.

MnMn is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

MnMn is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with MnMn.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Channel;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Live;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.User;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile
{
    [Serializable, DataContract]
    public class SmileSettingModel : ModelBase
    {
        #region property

        [DataMember]
        public SmileVideoSettingModel Video { get; set; } = new SmileVideoSettingModel();

        [DataMember]
        public SmileUserAccountModel Account { get; set; } = new SmileUserAccountModel();

        [DataMember]
        public SmileMyListSettingModel MyList { get; set; } = new SmileMyListSettingModel();

        [DataMember]
        public SmileLiveSettingModel Live { get; set; } = new SmileLiveSettingModel();

        [DataMember]
        public SmileUserSettingModel User { get; set; } = new SmileUserSettingModel();

        [DataMember]
        public SmileChannelSettingModel Channel { get; set; } = new SmileChannelSettingModel();

        [DataMember]
        public SmileIdleTalkMutterSettingModel IdleTalkMutter { get; set; } = new SmileIdleTalkMutterSettingModel();

        #endregion
    }
}
