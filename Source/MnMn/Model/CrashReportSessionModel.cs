using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.Define;

namespace ContentTypeTextNet.MnMn.MnMn.Model
{
    [Serializable]
    public class CrashReportSessionModel: ModelBase
    {
        #region property

        public LoginState LoginState { get; set; }

        public CDataModel Extension { get; set; } = new CDataModel();

        #endregion
    }
}
