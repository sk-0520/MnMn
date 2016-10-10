/*
This file is part of MnMn.

MnMn is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

MnMn is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with MnMn.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Navigation;
using System.Windows.Threading;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting;
using ContentTypeTextNet.MnMn.MnMn.View.Controls;
using ContentTypeTextNet.MnMn.MnMn.ViewModel;
using ContentTypeTextNet.Pe.PeMain.Logic.Utility;
using Gecko;

namespace ContentTypeTextNet.MnMn.MnMn
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App: Application
    {
        App()
        {
            SplashWindow = new SplashWindow();
        }

        #region property

        Mutex Mutex { get; } = new Mutex(false, Constants.ApplicationUsingName);

        SplashWindow SplashWindow { get; }

        Mediation Mediation { get; set; }
        MainWindow View { get; set; }
        AppManagerViewModel AppManager { get; set; }

        #endregion

        #region function

        void CatchUnhandleException(Exception ex, bool callerUiThread)
        {
            if(Mediation != null && Mediation.Logger != null) {
                Mediation.Logger.Fatal(ex);
            } else {
                Debug.WriteLine(ex);
            }
        }

        CheckResultModel<AppSettingModel> LoadSetting(ILogger logger)
        {
            var dir = VariableConstants.GetSettingDirectory();
            var filePath = Path.Combine(dir.FullName, Constants.SettingFileName);
            var existsFile = File.Exists(filePath);

            var setting = SerializeUtility.LoadSetting<AppSettingModel>(filePath, SerializeFileType.Json, logger);

            return new CheckResultModel<AppSettingModel>(existsFile, setting, null, null);
        }

        public static bool CheckAccept(RunningInformationSettingModel model, ILogger logger)
        {
            if(!model.Accept) {
                // 完全に初回
                logger.Debug("first");
                return false;
            }

            if(model.LastExecuteVersion == null) {
                // 何らかの理由で前回実行時のバージョン格納されていない
                logger.Debug("last version == null");
                return false;
            }

            if(model.LastExecuteVersion < Constants.AcceptVersion) {
                // 前回バージョンから強制定期に使用許諾が必要
                logger.Debug("last version < accept version");
                return false;
            }

            return true;
        }

        void SetLanguage(string cultureName)
        {
            CultureInfo usingCultureInfo = null;
            CultureInfo usingUICultureInfo = null;

            if(!string.IsNullOrWhiteSpace(cultureName)) {
                usingUICultureInfo = usingCultureInfo = CultureInfo.GetCultures(CultureTypes.AllCultures)
                    .FirstOrDefault(c => c.Name == cultureName)
                ;
            }
            if(usingCultureInfo == null) {
                usingCultureInfo = CultureInfo.CurrentCulture;
                usingUICultureInfo = CultureInfo.CurrentUICulture;
            }

            CultureInfo.DefaultThreadCurrentCulture = usingCultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = usingUICultureInfo;

            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(
                    XmlLanguage.GetLanguage(usingCultureInfo.IetfLanguageTag)
                )
            );
        }

        #endregion

        #region Application

        protected async override void OnStartup(StartupEventArgs e)
        {
            var logger = new Pe.PeMain.Logic.AppLogger();
            logger.IsStock = true;

            // ログオプションのあれこれ
            if(VariableConstants.HasOptionLogDirectoryPath) {
                var logFilePath = Path.Combine(Environment.ExpandEnvironmentVariables(VariableConstants.OptionLogDirectoryPath), PathUtility.AppendExtension($"{Constants.GetNowTimestampFileName()}-{Constants.BuildType}", "log"));
                FileUtility.MakeFileParentDirectory(logFilePath);
                var writer = new StreamWriter(new FileStream(logFilePath, FileMode.Create, FileAccess.Write, FileShare.Read)) {
                    AutoFlush = true,
                };
                logger.AttachmentStream(writer, true);
                logger.LoggerConfig.PutsStream = true;
            }

            logger.Information(Constants.ApplicationName, new AppInformationCollection().ToString());

            if(!Mutex.WaitOne(Constants.MutexWaitTime, false)) {
                Shutdown();
                return;
            }

            base.OnStartup(e);

#if DEBUG
            DoDebug();
#endif
            //var dir = VariableConstants.GetSettingDirectory();
            //var filePath = Path.Combine(dir.FullName, Constants.SettingFileName);
            //var setting = SerializeUtility.LoadSetting<AppSettingModel>(filePath, SerializeFileType.Json, logger);
            var settingResult = LoadSetting(logger);
            var setting = settingResult.Result;
            SetLanguage(setting.CultureName);

            Mediation = new Mediation(setting, logger);

            //var ieVersion = SystemEnvironmentUtility.GetInternetExplorerVersion();
            //logger.Information("IE version: " + ieVersion);
            //SystemEnvironmentUtility.SetUsingBrowserVersionForExecutingAssembly(ieVersion);

            AppManager = new AppManagerViewModel(Mediation, logger);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            WebNavigatorCore.Initialize(Mediation);
            AppUtility.SetTheme(setting.Theme);

            if(!CheckAccept(setting.RunningInformation, logger)) {
                var accept = new AcceptWindow(Mediation);
                var acceptResult = accept.ShowDialog();
                if(!acceptResult.GetValueOrDefault()) {
                    Shutdown();
                    return;
                }
                setting.RunningInformation.Accept = true;

                var isFirst = settingResult.IsSuccess;
                if(isFirst) {
                    setting.RunningInformation.FirstVersion = Constants.ApplicationVersionNumber;
                    setting.RunningInformation.FirstTimestamp = DateTime.Now;
                }
            }
            setting.RunningInformation.LastExecuteVersion = Constants.ApplicationVersionNumber;
            setting.RunningInformation.ExecuteCount = RangeUtility.Increment(setting.RunningInformation.ExecuteCount);

            SplashWindow.Show();

            await AppManager.InitializeAsync();
            View = new MainWindow() {
                DataContext = AppManager,
            };
            MainWindow = View;
            MainWindow.Closed += MainWindow_Closed;
            AppManager.InitializeView(View);
            MainWindow.Show();
            SplashWindow.Close();

        }

        protected override void OnExit(ExitEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException -= CurrentDomain_UnhandledException;

            base.OnExit(e);
        }

        #endregion

        /// <summary>
        /// UIスレッド
        /// </summary>
        void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            CatchUnhandleException(e.Exception, true);
            //e.Handled = true;
        }

        /// <summary>
        /// 非UIスレッド
        /// </summary>
        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            CatchUnhandleException((Exception)e.ExceptionObject, false);

            Shutdown();
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            WebNavigatorCore.Uninitialize();

            AppManager.UninitializeView(View);

            Mutex.Dispose();
        }
    }
}
