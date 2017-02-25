using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Live;
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

        public static Uri GetWatchUrl(RawSmileLiveGetPlayerStatusModel rawModel)
        {
            //直打ち
            var baseUri = Constants.ServiceSmileLiveQatchUrlBase;
            var watchUri = new Uri(baseUri, rawModel.Stream.Id);
            return watchUri;
        }

        public static SmileLiveType ConvertType(string s)
        {
            switch(s) {
                case "channel":
                    return SmileLiveType.Channel;

                case "community":
                    return SmileLiveType.Community;

                default:
                    return SmileLiveType.Unknown;
            }
        }




        #endregion
    }
}
