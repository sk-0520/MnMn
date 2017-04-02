using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;

namespace ContentTypeTextNet.MnMn.SystemApplications.Extractor
{
    public class LogItemViewModel:ViewModelBase
    {
        public LogItemViewModel(LogKind logKind, string message, string detail)
        {
            Kind = logKind;
            Message = message;
            Detail = detail;
        }

        #region property

        public DateTime Timestamp { get; } = DateTime.Now;

        public LogKind Kind { get; }
        public string Message { get; }
        public string Detail { get; }

        public bool HasDetail => !string.IsNullOrWhiteSpace(Detail);

        #endregion
    }
}
