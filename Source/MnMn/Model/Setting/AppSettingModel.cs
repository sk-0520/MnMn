﻿/*
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
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Setting
{
    /// <summary>
    /// 設定基底。
    /// <para>一ファイルにすべて集約させる。</para>
    /// <para>Peみたいに分割するのもいいけどなんかだるい。</para>
    /// </summary>
    [Serializable, DataContract]
    public class AppSettingModel: SettingModelBase
    {
        #region property

        /// <summary>
        /// 言語。
        /// <para>nullでカレント。</para>
        /// </summary>
        [DataMember]
        public string CultureName { get; set; }

        /// <summary>
        /// キャッシュディレクトリ。
        /// </summary>
        [DataMember]
        public string CacheDirectoryPath { get; set; }
        /// <summary>
        /// キャッシュ有効期間。
        /// </summary>
        [DataMember]
        public TimeSpan CacheLifeTime { get; set; } = Constants.SettingApplicationCacheLifeTime;
        /// <summary>
        /// メインウィンドウ。
        /// </summary>
        [DataMember]
        public WindowStatusModel Window { get; set; } = new WindowStatusModel() {
            Left = Constants.SettingApplicationWindowLeft,
            Top = Constants.SettingApplicationWindowTop,
            Width = Constants.SettingApplicationWindowWidth,
            Height = Constants.SettingApplicationWindowHeight,
        };

        [DataMember]
        public double ViewScale { get; set; } = Constants.SettingApplicationViewScale;

        [DataMember]
        public RunningInformationSettingModel RunningInformation { get; set; } = new RunningInformationSettingModel();

        [DataMember]
        public WebNavigatorSettingModel WebNavigator { get; set; } = new WebNavigatorSettingModel();

        [DataMember]
        public ThemeSettingModel Theme { get; set; } = new ThemeSettingModel();

        [DataMember]
        public bool SystemBreakSuppression { get; set; }= Constants.SettingApplicationSystemBreakSuppression;

        [DataMember]
        public NetworkSettingModel Network { get; set; } = new NetworkSettingModel();

        #region service

        [DataMember]
        public SmileSettingModel ServiceSmileSetting { get; set; } = new SmileSettingModel();

        #endregion

        #endregion
    }
}
