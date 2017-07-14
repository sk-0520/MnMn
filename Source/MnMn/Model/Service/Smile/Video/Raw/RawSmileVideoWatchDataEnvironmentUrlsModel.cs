using System.Runtime.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw
{
    public class RawSmileVideoWatchDataEnvironmentUrlsModel : ModelBase
    {
        #region property

        [DataMember(Name = "playerHelp")]
        public string PlayerHelp { get;set;}

        #endregion
    }
}