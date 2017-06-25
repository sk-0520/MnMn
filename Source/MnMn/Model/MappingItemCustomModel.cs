using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly;

namespace ContentTypeTextNet.MnMn.MnMn.Model
{
    public class MappingItemCustomModel: MappingItemNodeBase, IReadOnlyMappingItemCustom
    {
        #region IReadOnlyMappingItemCustom

        [XmlAttribute("rep-from")]
        public string ReplaceFrom { get; set; }

        [XmlAttribute("rep-to")]
        public string ReplaceTo { get; set; }

        #endregion
    }
}
