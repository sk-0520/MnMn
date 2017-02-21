using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Live.Raw;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Live
{
    public static class SmileLiveGetPlayerStatusUtility
    {
        #region function

        /// <summary>
        /// サムネイル情報取得のステータス確認。
        /// </summary>
        /// <param name="rawModel"></param>
        /// <returns></returns>
        public static bool IsSuccessResponse(RawSmileLiveGetPlayerStatusModel rawModel)
        {
            return string.Compare(rawModel.Status.Trim(), "ok", true) == 0;
        }


        #endregion
    }
}
