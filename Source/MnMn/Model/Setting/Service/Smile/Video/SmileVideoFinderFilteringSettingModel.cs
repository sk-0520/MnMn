using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video
{
    [Serializable, DataContract]
    public class SmileVideoFinderFilteringSettingModel: SettingModelBase
    {
        #region property

        /// <summary>
        /// フィルタリングデータ。
        /// </summary>
        [DataMember]
        public CollectionModel<SmileVideoFinderFilteringItemSettingModel> Items { get; set; } = new CollectionModel<SmileVideoFinderFilteringItemSettingModel>();

        #endregion
    }
}
