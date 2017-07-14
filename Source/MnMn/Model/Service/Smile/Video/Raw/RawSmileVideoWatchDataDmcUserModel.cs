using System.Runtime.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw
{
    public class RawSmileVideoWatchDataDmcUserModel:ModelBase
    {
        #region property

        [DataMember(Name = "user_id")]
        public string UserId { get; set; }

        [DataMember(Name = "is_premium")]
        public string IsPremium { get; set; }

        [DataMember(Name = "nickname")]
        public string Nickname { get; set; }

        #endregion
    }
}