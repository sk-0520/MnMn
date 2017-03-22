using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model
{
    [Serializable, XmlRoot("eazy-update")]
    public class EazyUpdateModel:ModelBase
    {
        #region property

        /// <summary>
        /// アップデートファイル作成時間。
        /// <para>保持している前回アップデート時間より未来であればアップデート対象。</para>
        /// </summary>
        [XmlElement("timestamp")]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// このバージョン以下であればアップデートする。
        /// </summary>
        [XmlElement("version")]
        public string Version { get; set; }

        [XmlArray("targets"), XmlArrayItem("element")]
        public CollectionModel<DefinedElementModel> Targets { get; set; } = new CollectionModel<DefinedElementModel>();

        #endregion
    }
}
