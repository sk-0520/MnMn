using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.Setup
{
    public static class Constants
    {
        #region variable

        const string mutexName = "MnMn";

        #endregion

        #region property

#if DEBUG
        /// <summary>
        /// アプリケーション使用名。
        /// </summary>
        public static string MutexName { get; } = mutexName + "-debug";
#else
        /// <summary>
        /// アプリケーション使用名。
        /// </summary>
        public static string MutexName { get; } = mutexName;
#endif

        public static TimeSpan MutexWaitTime { get; } = TimeSpan.FromSeconds(3);

        #endregion
    }
}
