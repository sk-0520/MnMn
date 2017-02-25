using System;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Raw;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Live.Raw
{
    [Serializable]
    public class RawSmileLiveGetPlayerStatusUserModel: ModelBase
    {
        #region property

        [XmlElement("user_id")]
        public string UserId { get; set; }
        [XmlElement("nickname")]
        public string NickName { get; set; }
        [XmlElement("is_premium")]
        public string IsPremium { get; set; }
        [XmlElement("userAge")]
        public string UserAge { get; set; }
        [XmlElement("userSex")]
        public string UserSex { get; set; }
        [XmlElement("userDomain")]
        public string UserDomain { get; set; }
        [XmlElement("userPrefecture")]
        public string UserPrefecture { get; set; }
        [XmlElement("userLanguage")]
        public string UserLanguage { get; set; }
        [XmlElement("room_label")]
        public string RoomLabel { get; set; }
        [XmlElement("room_seetno")]
        public string RoomSeetno { get; set; }
        [XmlElement("is_join")]
        public string IsJoin { get; set; }

        [XmlElement("twitter_info")]
        public RawSmileTwitterInfoModel TwitterInfo { get; set; } = new RawSmileTwitterInfoModel();

        #endregion
    }
}