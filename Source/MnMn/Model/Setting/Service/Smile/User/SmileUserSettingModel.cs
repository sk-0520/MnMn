using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.User
{
    [DataContract]
    public class SmileUserSettingModel: SettingModelBase
    {
        #region property

        /// <summary>
        /// ブックマーク。
        /// </summary>
        [DataMember]
        public CollectionModel<SmileUserBookmarkItemModel> Bookmark { get; set; } = new CollectionModel<SmileUserBookmarkItemModel>();

        /// <summary>
        /// 履歴。
        /// </summary>
        [DataMember]
        public FixedSizeCollectionModel<SmileUserItemModel> History { get; set; } = new FixedSizeCollectionModel<SmileUserItemModel>(Constants.ServiceSmileUserHistoryCount, false);

        #endregion
    }
}
