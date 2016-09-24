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
using ContentTypeTextNet.MnMn.MnMn.Define;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Live
{
    [DataContract]
    public class SmileLiveExecuteSettingModel: SettingModelBase
    {
        #region property

        /// 動画再生方法。
        /// </summary>
        [DataMember]
        public ExecuteOrOpenMode OpenMode { get; set; } = Constants.SettingServiceSmileLiveExecuteOpenMode;

        /// <summary>
        /// プレイヤーを新規ウィンドウで表示する｡
        /// </summary>
        [DataMember]
        public bool OpenPlayerInNewWindow { get; set; } = Constants.SettingServiceSmileVideoExecuteOpenPlayerInNewWindow;

        /// <summary>
        /// 外部プログラムのパス。
        /// </summary>
        [DataMember]
        public string LauncherPath { get; set; }

        /// <summary>
        /// 外部プログラム実行時のパラメータ。
        /// </summary>
        [DataMember]
        public string LauncherParameter { get; set; }


        #endregion
    }
}
