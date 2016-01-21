using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.NicoNico.Video
{
    /// <summary>
    /// カテゴリ。
    /// </summary>
    [Serializable]
    public class RankingGroupModel: ModelBase
    {
        #region property

        /// <summary>
        /// カテゴリ一覧。
        /// </summary>
        [XmlElement("element")]
        public CollectionModel<ElementModel> Items { get; set; } = new CollectionModel<ElementModel>();

        #endregion
    }
}
