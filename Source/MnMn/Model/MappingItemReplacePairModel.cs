using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly;

namespace ContentTypeTextNet.MnMn.MnMn.Model
{
    public class MappingItemReplacePairModel : MappingItemNodeBase, IReadOnlyMappingItemReplacePair
    {
        #region IReadOnlyMappingItemReplacePair

        [XmlAttribute("src")]
        public string Source { get; set; }

        [XmlAttribute("dst")]
        public string Destination { get; set; }

        #endregion
    }
}
