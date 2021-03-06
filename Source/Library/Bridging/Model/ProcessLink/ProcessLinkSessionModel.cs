﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.Library.Bridging.IF.ReadOnly;

namespace ContentTypeTextNet.MnMn.Library.Bridging.Model.ProcessLinker
{
    [Serializable, DataContract]
    public class ProcessLinkSessionModel : ModelBase, IReadOnlyProcessLinkSession
    {
        #region IReadOnlyProcessLinkSession

        [DataMember]
        public string ClientName { get; set; }

        [DataMember]
        public string ClientId { get; set; }

        #endregion
    }
}
