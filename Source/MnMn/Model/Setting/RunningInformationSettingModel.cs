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
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.Setting;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Setting
{
    [Serializable, DataContract]
    public class RunningInformationSettingModel: SettingModelBase, IReadOnlyRunningInformationSetting
    {
        #region IReadOnlyRunningInformationSetting

        /// <summary>
        /// 使用許諾OK。
        /// </summary>
        [DataMember]
        public bool Accept { get; set; }

        /// <summary>
        /// 前回終了時のバージョン。
        /// </summary>
        [DataMember]
        public Version LastExecuteVersion { get; set; }

        /// <summary>
        /// 使用回数。
        /// </summary>
        [DataMember]
        public int ExecuteCount { get; set; }
        /// <summary>
        /// 初回起動バージョン。
        /// </summary>
        [DataMember]
        public Version FirstVersion { get; set; }
        /// <summary>
        /// 初回起動時間。
        /// </summary>
        [DataMember]
        public DateTime FirstTimestamp { get; set; }

        ///// <summary>
        ///// アップデートチェックを行うか。
        ///// </summary>
        //[DataMember]
        //public bool CheckUpdateRelease { get; set; }
        ///// <summary>
        ///// RCアップデートチェックを行うか。
        ///// </summary>
        //[DataMember]
        //public bool CheckUpdateRC { get; set; }
        /// <summary>
        /// アップデートチェックで無視するバージョン。
        /// </summary>
        [DataMember]
        public Version IgnoreUpdateVersion { get; set; }

        /// <summary>
        /// ユーザー識別子。
        /// <para>#474: ユーザー環境に対する識別子の設定</para>
        /// </summary>
        [DataMember]
        public string UserId { get; set; }

        /// <summary>
        /// 簡易アップデート実施日時。
        /// </summary>
        [DataMember]
        public DateTime LightweightUpdateTimestamp { get; set; } = Constants.LightweightUpdateNone;

        /// <summary>
        /// クラッシュレポート自動送信。
        /// </summary>
        [DataMember]
        public bool AutoSendCrashReport { get; set; } = true;

        #endregion
    }
}
