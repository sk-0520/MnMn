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
    public class RawSmileMarketVideoRelationModel: ModelBase
    {
        #region property

        [DataMember(Name = "pickup")]
        public string Pickup { get; set; }

        [DataMember(Name = "main")]
        public string Main { get; set; }

        [DataMember(Name = "polling")]
        public RawSmileMarketVideoRelationPollingModel Polling { get; set; } = new RawSmileMarketVideoRelationPollingModel();

        #endregion
    }
}
