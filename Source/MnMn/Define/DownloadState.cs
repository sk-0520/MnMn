using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Attribute;

namespace ContentTypeTextNet.MnMn.MnMn.Define
{
    /// <summary>
    /// ダウンロード状態。
    /// </summary>
    public enum DownloadState
    {
        [EnumResourceDisplay(nameof(Properties.Resources.String_App_Define_DownloadState_None))]
        None,
        [EnumResourceDisplay(nameof(Properties.Resources.String_App_Define_DownloadState_Waiting))]
        Waiting,
        [EnumResourceDisplay(nameof(Properties.Resources.String_App_Define_DownloadState_Preparation))]
        Preparation,
        [EnumResourceDisplay(nameof(Properties.Resources.String_App_Define_DownloadState_Downloading))]
        Downloading,
        [EnumResourceDisplay(nameof(Properties.Resources.String_App_Define_DownloadState_Completed))]
        Completed,
        [EnumResourceDisplay(nameof(Properties.Resources.String_App_Define_DownloadState_Failure))]
        Failure,
    }
}
