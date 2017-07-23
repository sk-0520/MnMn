using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile
{
    /// <summary>
    /// Twitter連携。
    /// </summary>
    [Serializable, DataContract]
    public class SmileIdleTalkMutterSettingModel: SettingModelBase
    {
        #region property

        /// <summary>
        /// 動画タイトルを付与する。
        /// </summary>
        [DataMember]
        public bool AutoInputVideoTitle { get; set; } = true;

        /// <summary>
        /// 資料ページURIを付与する。
        /// </summary>
        [DataMember]
        public bool AutoInputWatchPageUri { get; set; } = true;

        /// <summary>
        /// ハッシュタグの自動設定。
        /// </summary>
        [DataMember]
        public string AutoInputHashTags { get; set; }

        #endregion
    }
}
