using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.Logic;

namespace ContentTypeTextNet.MnMn.MnMn.Model
{
    [Serializable, DataContract]
    public class CrashReportModel: ModelBase
    {
        CrashReportModel()
        {
            Environments = new AppInformationCollection().ToString();
        }

        public CrashReportModel(Exception ex, bool callerUiThread)
            : this()
        {
            CrashString = ex.ToString();
            CallerUiThread = callerUiThread;
        }

        #region property

        [DataMember]
        public string CrashString { get; set; }

        [DataMember]
        public bool CallerUiThread { get; set; }

        [DataMember]
        public string Environments { get; set; }

        [DataMember]
        public string LogList { get; set; }

        #endregion
    }
}
