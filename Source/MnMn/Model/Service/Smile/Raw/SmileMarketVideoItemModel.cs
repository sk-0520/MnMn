using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Raw
{
    public class SmileMarketVideoItemModel: SmileMarketItemModel
    {
        #region property

        /// <summary>
        /// この動画からのクリック数。
        /// </summary>
        public string ReferenceViewCount { get; set; }

        #endregion
    }
}
