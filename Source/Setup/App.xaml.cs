using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ContentTypeTextNet.MnMn.Setup
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App: Application
    {
        #region property

        Mutex Mutex { get; } = new Mutex(false, Constants.MutexName);

        #endregion

        #region Application

        protected override void OnStartup(StartupEventArgs e)
        {
            if(!Mutex.WaitOne(Constants.MutexWaitTime, false)) {
                Shutdown();
                return;
            }

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if(Mutex != null) {
                Mutex.Dispose();
            }

            base.OnExit(e);
        }

        #endregion
    }
}
