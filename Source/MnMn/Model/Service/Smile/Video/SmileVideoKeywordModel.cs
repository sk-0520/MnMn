using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video
{
    [Serializable, XmlRoot("keyword")]
    public class SmileVideoKeywordModel: ModelBase, IReadOnlySmileVideoKeyword
    {
        #region

        [XmlElement("element")]
        public CollectionModel<DefinedElementModel> Items { get; set; }
        IReadOnlyList<IReadOnlyDefinedElement> IReadOnlySmileVideoKeyword.Items => Items;

        #endregion
    }
}
