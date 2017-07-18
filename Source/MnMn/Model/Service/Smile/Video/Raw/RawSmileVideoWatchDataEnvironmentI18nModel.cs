using System;
using System.Runtime.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw
{
    [Serializable, DataContract]
    public class RawSmileVideoWatchDataEnvironmentI18nModel : ModelBase
    {
        #region proeprty

        [DataMember(Name = "language")]
        public string Language { get; set; }
        [DataMember(Name = "locale")]
        public string Locale { get; set; }
        [DataMember(Name = "area")]
        public string Area { get; set; }
        [DataMember(Name = "footer")]
        public object Footer { get; set; }

        #endregion
    }
}