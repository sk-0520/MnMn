using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using Newtonsoft.Json.Linq;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw
{
    [Serializable, DataContract]
    public class RawSmileVideoWatchDataModel : ModelBase
    {
        #region property

        [DataMember]
        public RawSmileVideoWatchDataApiModel Api { get; set; }

        [DataMember]
        public RawSmileVideoWatchDataEnvironmentModel Environment { get;set;}

        #endregion
    }
}
