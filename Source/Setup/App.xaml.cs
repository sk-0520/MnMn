using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;

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

        #region function

        string GetInstalledApplicationPathFromRegistry()
        {
            using(var reg = Registry.CurrentUser.OpenSubKey(Constants.BaseRegistryPath)) {
                if(reg == null) {
                    return null;
                }
                var rawPath = reg.GetValue(Constants.ApplicationPathName, null);
                if(rawPath == null) {
                    return null;
                }

                var kind = reg.GetValueKind(Constants.ApplicationPathName);
                if(!kind.HasFlag(RegistryValueKind.String)) {
                    return null;
                }

                var path = (string)rawPath;
                if(string.IsNullOrWhiteSpace(path)) {
                    return null;
                }

                var expandedPath = Environment.ExpandEnvironmentVariables(path);
                if(!File.Exists(expandedPath)) {
                    return null;
                }

                //TODO: 実行形式かどうかまで判定してあげるべきなんかね, GetBinaryType 

                return expandedPath;
            }
        }

        string GetInstalledApplicationPathFromDefaultPath()
        {
            var path = Environment.ExpandEnvironmentVariables(Path.Combine(Constants.InstallDirectoryPath, Constants.ApplicationFileName));
            if(File.Exists(path)) {
                return path;
            }

            return null;
        }

        /// <summary>
        /// インストール済みパスの取得。
        /// </summary>
        /// <returns>インストール済みパス, null ならインストールされていない。</returns>
        string GetInstalledApplicationPath()
        {
            var regPath = GetInstalledApplicationPathFromRegistry();
            if(regPath != null) {
                return regPath;
            }

            var defaultPath = GetInstalledApplicationPathFromDefaultPath();

            return defaultPath;
        }

        #endregion

        #region Application

        protected override void OnStartup(StartupEventArgs e)
        {
            // インストーラかセットアップが動いてれば終了
            if(!Mutex.WaitOne(Constants.MutexWaitTime, false)) {
                Shutdown();
                return;
            }
            
            // すでにインストールされていればそちらを実行
            var installedPath = GetInstalledApplicationPath();
            if(installedPath != null) {
                try {
                    Process.Start(installedPath);
                    Shutdown();
                } catch(Exception ex) {
                    // 起動失敗ならインストール処理を継続
                    Trace.WriteLine(ex);
                }
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
