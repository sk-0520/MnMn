using System.Runtime.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw
{
    public class RawSmileVideoWatchDataDmcQualityVideoResolutionModel: ModelBase
    {
        #region property

        [DataMember(Name = "width")]
        public string Width { get; set; }

        [DataMember(Name = "height")]
        public string Height { get; set; }

        #endregion
    }
}