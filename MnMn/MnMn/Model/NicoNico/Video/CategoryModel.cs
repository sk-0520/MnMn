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
    public class CategoryModel
    {
        #region property

        /// <summary>
        /// カテゴリの検索キー。
        /// </summary>
        [XmlAttribute("key")]
        public string Key { get; set; }
        /// <summary>
        /// カテゴリタイトル。
        /// </summary>
        [XmlArray("words"), XmlArrayItem("word")]
        public CollectionModel<WordModel> Words { get; set; } = new CollectionModel<WordModel>();


        #endregion
    }
}
