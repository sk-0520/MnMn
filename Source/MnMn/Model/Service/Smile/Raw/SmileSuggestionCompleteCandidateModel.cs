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
    public class SmileSuggestionCompleteCandidateModel: ModelBase
    {
        #region property

        [DataMember(Name = "candidates")]
        public CollectionModel<string> Items { get; set; }

        #endregion
    }
}
