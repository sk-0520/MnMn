using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video
{
    public static class SmileVideoVideoItemUtility
    {
        #region function

        public static bool IsEquals(SmileVideoVideoItemModel a, SmileVideoVideoItemModel b)
        {
            if(a == null) {
                throw new ArgumentNullException(nameof(a));
            }
            if(b == null) {
                throw new ArgumentNullException(nameof(b));
            }

            if(a.VideoId == b.VideoId) {
                return true;
            }

            if(a.WatchUrl.OriginalString == b.WatchUrl.OriginalString) {
                return true;
            }

            return false;
        }

        #endregion
    }
}
