using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile
{
    [DataContract]
    public class SmileUserSettingModel: SettingModelBase
    {
        #region property

        /// <summary>
        /// ブックマーク。
        /// </summary>
        public CollectionModel<SmileUserBookmarkItemModel> Bookmark { get; set; } = new CollectionModel<SmileUserBookmarkItemModel>();

        /// <summary>
        /// 履歴。
        /// </summary>
        public FixedSizeCollectionModel<SmileUserItemModel> History { get; set; } = new FixedSizeCollectionModel<SmileUserItemModel>(Constants.ServiceSmileUserHistoryCount, false);

        #endregion
    }
}
