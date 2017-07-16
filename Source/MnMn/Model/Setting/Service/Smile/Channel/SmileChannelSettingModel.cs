using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Channel
{
    [DataContract]
    public class SmileChannelSettingModel: SettingModelBase
    {
        #region property

        [DataMember]
        public double GroupWidth { get; set; } = Constants.SettingServiceSmileChannelGroupAreaStar;
        [DataMember]
        public double ItemsWidth { get; set; } = Constants.SettingServiceSmileChannelItemsAreaStar;

        /// <summary>
        /// ブックマーク。
        /// </summary>
        [DataMember]
        public CollectionModel<SmileChannelBookmarkItemModel> Bookmark { get; set; } = new CollectionModel<SmileChannelBookmarkItemModel>();

        /// <summary>
        /// 履歴。
        /// </summary>
        [DataMember]
        public FixedSizeCollectionModel<SmileChannelItemModel> History { get; set; } = new FixedSizeCollectionModel<SmileChannelItemModel>(Constants.ServiceSmileChannelHistoryCount, false);

        #endregion
    }
}
