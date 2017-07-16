using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw
{
    [Serializable, DataContract]
    public class RawSmileVideoWatchDataEnvironmentModel : RawModelBase
    {
        #region property

        [DataMember(Name = "baseURL")]
        public RawSmileVideoWatchDataEnvironmentBaseUrlModel BaseUrl { get; set; }

        [DataMember(Name = "playlistToken")]
        public string PlaylistToken { get; set; }

        [DataMember(Name = "i18n")]
        public RawSmileVideoWatchDataEnvironmentI18nModel I18n { get; set; }

        [DataMember(Name = "urls")]
        public RawSmileVideoWatchDataEnvironmentUrlsModel Urls { get; set; }

        [DataMember(Name = "isMonitoringLogUser")]
        public string IsMonitoringLogUser { get; set; }

        #endregion
    }
}
