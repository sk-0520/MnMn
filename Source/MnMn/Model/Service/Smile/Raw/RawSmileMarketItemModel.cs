using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Raw
{
    /// <summary>
    /// 生なのかどうか微妙。
    /// </summary>
    public class SmileMarketItemModel:ModelBase
    {
        #region property

        /// <summary>
        /// 商品名。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 購入者数。
        /// </summary>
        public string BuyCount { get; set; }

        /// <summary>
        /// クリック数。
        /// </summary>
        public string ViewCount { get; set; }

        /// <summary>
        /// 市場へのURL。
        /// </summary>
        public string MarketUrl { get; set; }

        /// <summary>
        /// 売り場へのURL。
        /// </summary>
        public string CashRegisterUrl { get; set; }

        /// <summary>
        /// サムネイルURL。
        /// </summary>
        public string ThumbnailUrl { get; set; }

        #endregion
    }
}
