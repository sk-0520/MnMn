using System.Runtime.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw
{
    public class RawSmileVideoWatchDataOwnerModel:ModelBase
    {
        #region property

        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "nickname")]
        public string Nickname { get; set; }
        [DataMember(Name = "stampExp")]
        public string StampExp { get; set; }
        [DataMember(Name = "iconURL")]
        public string IconURL { get; set; }
        [DataMember(Name = "nicoliveInfo")]
        public string NicoliveInfo { get; set; }
        [DataMember(Name = "channelInfo")]
        public string ChannelInfo { get; set; }
        [DataMember(Name = "isUserVideoPublic")]
        public string IsUserVideoPublic { get; set; }
        [DataMember(Name = "isUserMyVideoPublic")]
        public string IsUserMyVideoPublic { get; set; }
        [DataMember(Name = "isUserOpenListPublic")]
        public string IsUserOpenListPublic { get; set; }

        #endregion
    }
}