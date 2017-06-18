using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video
{
    [DataContract]
    public class SmileVideoSearchBookmarkItemModel: SettingModelBase, IReadOnlySmileVideoSearchBookmarkItem
    {
        #region proeprty

        /// <summary>
        /// 検索クエリ。
        /// </summary>
        [DataMember]
        public string Query { get; set; }

        /// <summary>
        /// 検索方法。
        /// </summary>
        [DataMember]
        public SearchType SearchType { get; set; }

        ///// <summary>
        ///// 並び順。
        ///// </summary>
        //[DataMember]
        //public OrderBy OrderBy { get; set; }

        /// <summary>
        /// 更新チェックを行うか。
        /// </summary>
        [DataMember]
        public bool IsCheckUpdate { get; set; } = Constants.SettingServiceSmileVideoSearchIsCheckUpdate;

        /// <summary>
        /// 現在の差分動画。
        /// </summary>
        [DataMember]
        public CollectionModel<string> Videos { get; set; } = new CollectionModel<string>();

        #endregion

        #region IReadOnlySmileVideoSearchBookmarkItem

        public IReadOnlyList<string> VideoIds => Videos;

        #endregion
    }
}
