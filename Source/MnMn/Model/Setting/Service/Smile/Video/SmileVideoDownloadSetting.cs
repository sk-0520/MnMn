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

namespace ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video
{
    [Serializable, DataContract]
    public class SmileVideoDownloadSetting: SettingModelBase
    {
        #region property

        /// <summary>
        /// ダウンロードにDMC形式を使用するか。
        /// </summary>
        [DataMember]
        public bool UsingDmc { get; set; } = Constants.SettingServiceSmileVideoDownloadUsingDmc;

        /// <summary>
        /// 画質の重み。
        /// </summary>
        [DataMember]
        public int VideoWeight { get; set; } = Constants.SettingServiceSmileVideoDownloadDmcVideoWeight;
        /// <summary>
        /// 音声の重み。
        /// </summary>
        [DataMember]
        public int AudioWeight { get; set; } = Constants.SettingServiceSmileVideoDownloadDmcAudioWeight;

        #endregion
    }
}
