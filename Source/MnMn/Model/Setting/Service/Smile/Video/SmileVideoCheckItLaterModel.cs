using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video
{
    public class SmileVideoCheckItLaterModel: SmileVideoVideoItemModel
    {
        #region property

        /// <summary>
        /// 列挙された日時。
        /// </summary>
        [DataMember]
        public DateTime CheckTimestamp { get; set; }

        /// <summary>
        /// すでにチェック済みか。
        /// </summary>
        [DataMember]
        public bool IsChecked { get; set; }

        #endregion
    }
}
