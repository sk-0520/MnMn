using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model
{
    [Serializable]
    public class CrashReportSettingModel: ModelBase
    {
        #region property

        public DateTime WakeUpTimestamp { get; set; }
        public string RunningTime { get; set; }

        public string CacheDirectoryPath { get; set; }
        //public string UsingCacheDirectoryPath { get; set; }
        public string CacheLifeTime { get; set; }

        public CrashReportSessionModel SmileSession { get; set; } = new CrashReportSessionModel();

        #region RunningInformation

        public string FirstVersion { get; set; }
        public DateTime FirstTimestamp { get; set; }

        #endregion

        #region WebNavigator

        public bool GeckoFxScanPlugin { get; set; }

        #endregion

        #endregion
    }
}
