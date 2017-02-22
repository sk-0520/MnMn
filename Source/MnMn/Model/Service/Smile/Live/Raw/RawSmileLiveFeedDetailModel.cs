using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Live.Raw
{
    public class RawSmileLiveFeedDetailModel: ModelBase
    {
        #region property

        /// <summary>
        /// 大きい方のサムネイル。
        /// </summary>
        public string PictureUrl { get; set; }

        /// <summary>
        /// 開場。
        /// </summary>
        public string DoorsOpen { get; set; }

        /// <summary>
        /// 開園。
        /// </summary>
        public string Opening { get; set; }


    #endregion
}
}
