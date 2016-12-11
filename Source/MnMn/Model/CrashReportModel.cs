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
            CrashMessage = ex.Message;
            CallerUiThread = callerUiThread;
        }

        #region property

        [DataMember]
        public string CrashMessage { get; set; }

        [DataMember]
        public bool CallerUiThread { get; set; }

        [DataMember]
        public string Environments { get; set; }

        #endregion
    }
}
