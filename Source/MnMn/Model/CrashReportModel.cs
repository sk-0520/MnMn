using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.Logic;

namespace ContentTypeTextNet.MnMn.MnMn.Model
{
    [Serializable, XmlRoot("crash")]
    public class CrashReportModel: ModelBase
    {
        CrashReportModel()
        {
            Environments.Text = new AppInformationCollection().ToString();
        }

        public CrashReportModel(Exception ex, bool callerUiThread)
            : this()
        {
            Message.Text = ex.ToString();
            CallerUiThread = callerUiThread;
        }

        #region property

        [XmlElement("ui-thread")]
        public bool CallerUiThread { get; set; }

        [XmlElement("message")]
        public CDataModel Message { get; set; } = new CDataModel();

        [XmlElement("env")]
        public CDataModel Environments { get; set; } = new CDataModel();

        [XmlElement("log")]
        public CDataModel Logs { get; set; } = new CDataModel();

        [XmlElement("info")]
        public CDataModel Information { get; set; } = new CDataModel();

        [XmlElement("setting")]
        public CrashReportSettingModel Setting { get; set; } = new CrashReportSettingModel();

        #endregion
    }
}
