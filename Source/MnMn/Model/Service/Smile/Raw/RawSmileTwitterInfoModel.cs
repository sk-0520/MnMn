using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Raw
{
    [Serializable]
    public class RawSmileTwitterInfoModel:ModelBase
    {
        #region property

        [XmlElement("status")]
        public string Status { get; set; }
        [XmlElement("screen_name")]
        public string ScreenName { get; set; }
        [XmlElement("followers_count")]
        public string FollowersCount { get; set; }
        [XmlElement("is_vip")]
        public string IsVip { get; set; }
        [XmlElement("profile_image_url")]
        public string ProfileImageUrl { get; set; }
        [XmlElement("after_auth")]
        public string AfterAuth { get; set; }
        [XmlElement("tweet_token")]
        public string TweetToken { get; set; }
        #endregion
    }
}
