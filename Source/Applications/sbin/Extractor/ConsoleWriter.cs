using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.SystemApplications.Extractor
{
    /// <summary>
    /// <see cref="TextWriter.WriteLine(string)"/>を取得する。
    /// </summary>
    public sealed class ConsoleWriter: StringWriter
    {
        #region property

        public Action<string> WriteLineAction { get; set; }

        #endregion

        #region StringWriter

        public override void WriteLine(string value)
        {
            WriteLineAction?.Invoke(value);
            base.WriteLine(value);
        }

        #endregion
    }
}
