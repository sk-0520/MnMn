using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.Applications.CrashReporter
{
    public static class Constants
    {
        #region property

        /// <summary>
        /// アプリケーションパス。
        /// </summary>
        public static string AssemblyPath { get; } = Assembly.GetExecutingAssembly().Location;
        /// <summary>
        /// アプリケーション親ディレクトリパス。
        /// </summary>
        public static string AssemblyRootDirectoryPath { get; } = Path.GetDirectoryName(AssemblyPath);

        /// <summary>
        /// ライブラリ周りを MnMn と共有。
        /// </summary>
        public static string LibraryDirectoryPath { get; } = Path.Combine(AssemblyRootDirectoryPath, "..", "..", "lib");

        #endregion
    }
}
