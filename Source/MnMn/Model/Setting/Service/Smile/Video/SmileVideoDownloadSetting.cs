using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video
{
    public class SmileVideoDownloadSetting: SettingModelBase
    {
        #region property

        /// <summary>
        /// ダウンロードに新形式を使用するか。
        /// </summary>
        public bool UsingDmc { get; set; } =
#if DEBUG
            true
#else
            false
#endif
        ;

        /// <summary>
        /// 画質の重み。
        /// </summary>
        public int VideoWeight { get; set; } = 70;
        /// <summary>
        /// 音声の重み。
        /// </summary>
        public int AudioWeight { get; set; } = 45;

        #endregion
    }
}
