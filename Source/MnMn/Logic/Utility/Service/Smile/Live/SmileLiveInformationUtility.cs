using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Live;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Live
{
    public static class SmileLiveInformationUtility
    {
        #region define

        public const string launcherParameterVideoId = "video-id";
        public const string launcherParameterVideoTitle = "video-title";
        public const string launcherParameterVideoPage = "video-page";

        #endregion

        #region function

        /// <summary>
        /// 外部プログラムで実行する際のパラメータを生成する。
        /// </summary>
        /// <param name="information"></param>
        /// <param name="baseParameter"></param>
        /// <returns></returns>
        public static string MakeLauncherParameter(SmileLiveInformationViewModel information, string baseParameter)
        {
            var map = new Dictionary<string, string>() {
                { launcherParameterVideoId, information.Id },
                { launcherParameterVideoTitle, information.Title },
                { launcherParameterVideoPage, information.WatchUrl.OriginalString },
            };
            var result = AppUtility.ReplaceString(baseParameter, map);

            return result;
        }

        #endregion
    }
}
