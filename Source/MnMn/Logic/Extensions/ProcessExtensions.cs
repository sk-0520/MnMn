using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Extensions
{
    public static class ProcessExtensions
    {
        #region define

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool SetDllDirectory(string lpPathName);

        #endregion

        #region function

        /// <summary>
        /// <see cref="Process.Start"/>を実行する際に DLL の設定をぶった切る。
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        public static bool StartWithDllReset(this Process process)
        {
            if(process == null) {
                throw new ArgumentNullException(nameof(process));
            }

            try {
                SetDllDirectory(string.Empty);
                return process.Start();
            } finally {
                SetDllDirectory(Constants.WebNavigatorGeckoFxLibraryDirectoryPath);
            }
        }

        #endregion
    }
}
