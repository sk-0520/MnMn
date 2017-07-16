using System;
using System.Runtime.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw
{
    [Serializable, DataContract]
    public class RawSmileVideoWatchDataEnvironmentBaseUrlModel : ModelBase
    {
        #region property

        [DataMember(Name = "web")]
        public string Web { get; set; }
        [DataMember(Name = "res")]
        public string Res { get; set; }
        [DataMember(Name = "dic")]
        public string Dic { get; set; }
        [DataMember(Name = "flapi")]
        public string Flapi { get; set; }
        [DataMember(Name = "riapi")]
        public string Riapi { get; set; }
        [DataMember(Name = "live")]
        public string Live { get; set; }
        [DataMember(Name = "com")]
        public string Com { get; set; }
        [DataMember(Name = "ch")]
        public string Ch { get; set; }
        [DataMember(Name = "secureCh")]
        public string SecureCh { get; set; }
        [DataMember(Name = "commons")]
        public string Commons { get; set; }
        [DataMember(Name = "commonsAPI")]
        public string CommonsAPI { get; set; }
        [DataMember(Name = "embed")]
        public string Embed { get; set; }
        [DataMember(Name = "ext")]
        public string Ext { get; set; }
        [DataMember(Name = "nicoMs")]
        public string NicoMs { get; set; }
        [DataMember(Name = "ichiba")]
        public string Ichiba { get; set; }
        [DataMember(Name = "uadAPI")]
        public string UadAPI { get; set; }
        [DataMember(Name = "ads")]
        public string Ads { get; set; }
        [DataMember(Name = "account")]
        public string Account { get; set; }
        [DataMember(Name = "secure")]
        public string Secure { get; set; }
        [DataMember(Name = "ex")]
        public string Ex { get; set; }
        [DataMember(Name = "qa")]
        public string Qa { get; set; }
        [DataMember(Name = "publicAPI")]
        public string PublicAPI { get; set; }
        [DataMember(Name = "uad")]
        public string Uad { get; set; }
        [DataMember(Name = "app")]
        public string App { get; set; }
        [DataMember(Name = "appClientAPI")]
        public string AppClientAPI { get; set; }
        [DataMember(Name = "point")]
        public string Point { get; set; }
        [DataMember(Name = "enquete")]
        public string Enquete { get; set; }
        [DataMember(Name = "notification")]
        public string Notification { get; set; }
        [DataMember(Name = "upload")]
        public string Upload { get; set; }
        [DataMember(Name = "sugoiSearchSystem")]
        public string SugoiSearchSystem { get; set; }

        #endregion
    }
}