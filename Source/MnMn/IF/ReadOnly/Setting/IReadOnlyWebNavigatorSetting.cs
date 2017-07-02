using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.Setting
{
    public interface IReadOnlyWebNavigatorSetting
    {
        #region property

        /// <summary>
        /// プラグインをシステムから読み込むか。
        /// </summary>
        bool GeckoFxScanPlugin { get; }

        /// <summary>
        /// ブラウザからのファイルダウンロード先。
        /// </summary>
        string DownloadDirectoryPath { get; }

        #endregion
    }
}
