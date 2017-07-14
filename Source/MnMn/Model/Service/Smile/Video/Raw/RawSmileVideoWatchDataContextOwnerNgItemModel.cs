using System.Runtime.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw
{
    public class RawSmileVideoWatchDataContextOwnerNgItemModel: ModelBase
    {
        #region property
        [DataMember(Name = "Source")]
        public string Source { get; set; }
        [DataMember(Name = "Destination")]
        public string Destination { get; set; }
        #endregion
    }
}