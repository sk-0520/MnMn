using System.Runtime.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw
{
    public class RawSmileVideoWatchDataThreadIdModel:ModelBase
    {
        #region property

        [DataMember(Name = "default")]
        public string Default { get; set; }
        [DataMember(Name = "nicos")]
        public string Nicos { get; set; }
        [DataMember(Name = "community")]
        public string Community { get; set; }

        #endregion
    }
}