using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw
{
    [Serializable, DataContract]
    public class RawSmileVideoWatchDataDmcSessionApiModel : ModelBase
    {
        #region property

        [DataMember(Name = "recipe_id")]
        public string RecipeId { get; set; }

        [DataMember(Name = "player_id")]
        public string PlayerId { get; set; }

        [DataMember(Name = "videos")]
        public CollectionModel<string> Videos { get; set; }

        [DataMember(Name = "audios")]
        public CollectionModel<string> Audios { get; set; }

        [DataMember(Name = "movies")]
        public CollectionModel<string> Movies { get; set; }

        [DataMember(Name = "protocols")]
        public CollectionModel<string> Protocols { get; set; }

        [DataMember(Name = "auth_types")]
        public Dictionary<string, string> AuthTypes { get; set; }

        [DataMember(Name = "service_user_id")]
        public string ServiceUserId { get; set; }

        [DataMember(Name = "token")]
        public string Token { get; set; }

        [DataMember(Name = "signature")]
        public string Signature { get; set; }

        [DataMember(Name = "content_id")]
        public string ContentId { get; set; }

        [DataMember(Name = "heartbeat_lifetime")]
        public string HeartbeatLifetime { get; set; }

        [DataMember(Name = "content_key_timeout")]
        public string ContentKeyTimeout { get; set; }

        [DataMember(Name = "priority")]
        public string Priority { get; set; }

        [DataMember(Name = "urls")]
        public CollectionModel<RawSmileVideoWatchDataDmcSessionApiUrlModel> Urls { get; set; }


        #endregion
    }
}