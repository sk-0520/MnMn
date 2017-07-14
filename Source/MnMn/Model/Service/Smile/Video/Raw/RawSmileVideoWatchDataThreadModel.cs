using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw
{
    public class RawSmileVideoWatchDataThreadModel: ModelBase
    {
        #region property

        [DataMember(Name = "commentCount")]
        public string CommentCount { get; set; }
        [DataMember(Name = "hasOwnerThread")]
        public string HasOwnerThread { get; set; }
        [DataMember(Name = "mymemoryLanguage")]
        public string MymemoryLanguage { get; set; }
        [DataMember(Name = "serverUrl")]
        public string ServerUrl { get; set; }
        [DataMember(Name = "subServerUrl")]
        public string SubServerUrl { get; set; }

        [DataMember(Name = "ids")]
        public RawSmileVideoWatchDataThreadIdModel Ids { get; set; }

        #endregion
    }
}
