using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly;

namespace ContentTypeTextNet.MnMn.MnMn.Model
{
    public class MappingItemReplaceModel : MappingItemNodeBase, IReadOnlyMappingItemReplace
    {
        #region variable

        Regex _regex = null;

        #endregion

        #region IReadOnlyMappingItemReplace

        [XmlAttribute("regex-options")]
        public RegexOptions RegexOptions { get; set; }

        [XmlElement("pattern")]
        public CDataModel Pattern { get; set; }
        IReadOnlyCData IReadOnlyMappingItemReplace.Pattern => Pattern;

        [XmlIgnore]
        public Regex Regex
        {
            get
            {
                if(this._regex == null) {
                    this._regex = new Regex(Pattern.Text, RegexOptions);
                }

                return this._regex;
            }
        }

        [XmlElement("pair")]
        public CollectionModel<MappingItemReplacePairModel> Pairs { get; set; } = new CollectionModel<MappingItemReplacePairModel>();
        IReadOnlyList<IReadOnlyMappingItemReplacePair> IReadOnlyMappingItemReplace.Pairs => Pairs;

        #endregion
    }
}
