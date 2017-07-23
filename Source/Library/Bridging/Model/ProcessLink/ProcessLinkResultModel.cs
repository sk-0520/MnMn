using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.Library.Bridging.Model.ProcessLink
{
    [Serializable, DataContract]
    public class ProcessLinkResultModel : ModelBase
    {
        public ProcessLinkResultModel(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }

        #region property

        [DataMember]
        public bool IsSuccess { get; set; }

        [DataMember]
        public string Message { get; set; }

        #endregion
    }
}
