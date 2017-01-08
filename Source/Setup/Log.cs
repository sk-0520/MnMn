using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.Setup
{
    public enum LogKind
    {
        Message,
        Warning,
        Error
    }

    public class LogItem
    {
        public LogItem(LogKind kind, string message)
        {
            Kind = kind;
            Message = message;
        }

        #region property

        public LogKind Kind { get; }

        public string Message { get; }

        #endregion
    }
}
