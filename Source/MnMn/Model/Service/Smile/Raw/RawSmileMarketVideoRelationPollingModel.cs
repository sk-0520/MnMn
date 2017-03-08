using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Raw
{
    [Serializable, DataContract]
    public class RawSmileMarketVideoRelationPollingModel: ModelBase
    {
        #region property

        [DataMember(Name = "shortIntarval")]
        public string  ShortIntarval { get; set; }
        [DataMember(Name = "longIntarval")]
        public string LongIntarval { get; set; }
        [DataMember(Name = "defaultIntarval")]
        public string DefaultIntarval { get; set; }
        [DataMember(Name = "maxNoChangeCount")]
        public string MaxNoChangeCount { get; set; }

        #endregion
    }
}
