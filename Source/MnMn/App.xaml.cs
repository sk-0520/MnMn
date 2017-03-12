//#define FORCE_ACCEPT
//#define KILL

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Threading;
using ContentTypeTextNet.Library.PInvoke.Windows;
using ContentTypeTextNet.Library.SharedLibrary.CompatibleWindows.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Parameter;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting;
using ContentTypeTextNet.MnMn.MnMn.View.Controls;
using ContentTypeTextNet.MnMn.MnMn.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Service.Smile;
using ContentTypeTextNet.Pe.PeMain.Logic.Utility;
using Gecko;

#if !DEBUG && !BETA
#if FORCE_ACCEPT
#error FORCE_ACCEPT
#endif
#if KILL
#error KILL
#endif
#endif

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

#if DEBUG && KILL
            var t = new DispatcherTimer();
            t.Interval = TimeSpan.FromSeconds(10);
            t.Tick += (sender, e) => { throw new Exception("しんだ！"); };
            t.Start();
#endif
        }

        #region property

        DateTime WakeUpTimestamp { get; } = DateTime.Now;

        Mutex Mutex { get; } = new Mutex(false, Constants.ApplicationUsingName);

        SplashWindow SplashWindow { get; set; }

        Mediation Mediation { get; set; }
        MainWindow View { get; set; }
        AppManagerViewModel AppManager { get; set; }

        #endregion

        #region function

        void CatchUnhandleException(Exception ex, bool callerUiThread)
        {
            Debug.WriteLine($"{nameof(callerUiThread)} = {callerUiThread}");
            if(Mediation != null && Mediation.Logger != null) {
                Mediation.Logger.Fatal(ex);
            } else {
                Debug.WriteLine(ex);
            }

            var reportPath = CreateCrashReport(ex, callerUiThread);
            var sendCrash = Constants.AppSendCrashReport;
#if KILL
            sendCrash = true;
#endif
            if(sendCrash) {
                var args = $"/crash /report=\"{reportPath}\"";
                if(Constants.AppCrashReportIsDebug) {
                    args += " /debug";
                }
                Process.Start(Constants.CrashReporterApplicationPath, args);
            }

            Shutdown();
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
#if FORCE_ACCEPT
            var a = true; if(a) return false;
#endif
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

        static DateTime? GetCrashReportFileTimestamp(FileInfo file)
        {
            DateTime dateTime;
            if(DateTime.TryParseExact(Path.GetFileNameWithoutExtension(file.Name), Constants.FormatTimestampFileName, CultureInfo.InvariantCulture, DateTimeStyles.AllowLeadingWhite, out dateTime)) {
                return dateTime;
            }

            if(file.Exists) {
                return file.CreationTime;
            }

            return null;
        }

        /// <summary>
        /// 古い異常終了時のデータを破棄。
        /// </summary>
        /// <returns></returns>
        Task InitializeCrashReportAsync()
        {
            return Task.Run(() => {
                var dir = VariableConstants.GetCrashReportDirectory();
                dir.Refresh();
                if(dir.Exists) {
                    FileUtility.RotateFiles(dir.FullName, Constants.CrashReportSearchPattern, OrderBy.Descending, Constants.CrashReportCount, ex => {
                        Mediation.Logger.Error(ex);
                        return true;
                    });
                }
            });
        }

        void SetCrashReportSetting(CrashReportSettingModel target)
        {
            target.WakeUpTimestamp = WakeUpTimestamp;
            target.RunningTime = (DateTime.Now - WakeUpTimestamp).ToString();

            var setting = Mediation.GetResultFromRequest<AppSettingModel>(new RequestModel(RequestKind.Setting, ServiceType.Application));
            //var cacheDirectory = Mediation.GetResultFromRequest<DirectoryInfo>(new RequestModel(RequestKind.CacheDirectory, ServiceType.Application));

            target.CacheDirectoryPath = setting.CacheDirectoryPath;
            //target.UsingCacheDirectoryPath = cacheDirectory.FullName;
            target.CacheLifeTime = setting.CacheLifeTime.ToString();

            target.FirstVersion = setting.RunningInformation.FirstVersion?.ToString();
            target.FirstTimestamp = setting.RunningInformation.FirstTimestamp;

            target.GeckoFxScanPlugin = setting.WebNavigator.GeckoFxScanPlugin;

            // セッション
            var smileSession = Mediation.GetResultFromRequest<SmileSessionViewModel>(new RequestModel(RequestKind.Session, ServiceType.Smile));
            target.SmileSession.LoginState = smileSession.LoginState;
            if(smileSession.IsLoggedIn) {
                target.SmileSession.Extension.Text = $"{nameof(smileSession.IsPremium)} = {smileSession.IsPremium}, {nameof(smileSession.IsOver18)} = {smileSession.IsOver18}";
            }
            //SmileSession
        }

        void SetCrashReportInformation(CDataModel information, Exception ex)
        {
            // #465
            if(ex is DllNotFoundException) {
                var appDir = Constants.AssemblyRootDirectoryPath;
                var files = Directory.GetFiles(appDir, "*", SearchOption.AllDirectories)
                    .Select(f => f.Substring(appDir.Length))
                ;
                information.Text = string.Join(Environment.NewLine, files);
            }
        }

        string CreateCrashReport(Exception ex, bool callerUiThread)
        {
            var logParam = new AppLoggingParameterModel() {
                GetClone = true,
            };

            var logs = Mediation.GetResultFromRequest<IEnumerable<LogItemModel>>(new AppLogingProcessRequestModel(logParam))
                .Select(i => $"[{i.Timestamp:yyyy-MM-ddTHH:mm:ss.fff}] {i.Message}:  {i.CallerMember} ({i.CallerLine})" + (i.HasDetail ? (Environment.NewLine + i.DetailText): string.Empty) )
            ;

            var crashReport = new CrashReportModel(ex, callerUiThread);
            crashReport.Logs.Text = string.Join(Environment.NewLine, logs);
            SetCrashReportInformation(crashReport.Information, ex);
            SetCrashReportSetting(crashReport.Setting);

            var dir = VariableConstants.GetCrashReportDirectory();
            var path = Path.Combine(dir.FullName, PathUtility.AppendExtension(Constants.GetNowTimestampFileName(), Constants.CrashReportFileExtension));
            SerializeUtility.SaveXmlSerializeToFile(path, crashReport);

            return path;
        }

        bool ShowBetaMessageIfNoBetaFlag()
        {
            if(VariableConstants.HasOptionExecuteBetaVersion) {
                // フラグ指定があれば実行OK
                return true;
            }

            var dialogResult = MessageBox.Show(
                MnMn.Properties.Resources.String_App_ExecuteBetaVersion_Warning,
                $"{Constants.ApplicationName}:{Constants.ApplicationVersion}",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning,
                MessageBoxResult.No,
                MessageBoxOptions.DefaultDesktopOnly
            );

            return dialogResult == MessageBoxResult.OK;
        }

#endregion

#region Application

        protected async override void OnStartup(StartupEventArgs e)
        {
#if BETA
            if(!ShowBetaMessageIfNoBetaFlag()) {
                Shutdown();
            }
#endif
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

            var stream = GlobalManager.MemoryStream.GetStreamWidthAutoTag();
            using(var writer = new StreamWriter(stream)) {
                var appInfo = new AppInformationCollection();
                appInfo.WriteInformation(writer);
                var s = Encoding.UTF8.GetString(stream.GetBuffer(), 0, (int)stream.Length);
                logger.Information(Constants.ApplicationName, s);
            }

            if(!Mutex.WaitOne(Constants.MutexWaitTime, false)) {
                Shutdown();
                return;
            }

            base.OnStartup(e);

#if DEBUG
            DoDebug();
#endif

            var settingResult = LoadSetting(logger);
            var setting = settingResult.Result;
            SetLanguage(setting.CultureName);

            Mediation = new Mediation(setting, logger);

            AppManager = new AppManagerViewModel(Mediation, logger);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            WebNavigatorCore.Initialize(Mediation);
            AppUtility.InitializeTheme(Mediation.Logger);
            AppUtility.SetTheme(setting.Theme);

            if(!CheckAccept(setting.RunningInformation, logger)) {
                var accept = new AcceptWindow(Mediation);
                var acceptResult = accept.ShowDialog();
                if(!acceptResult.GetValueOrDefault()) {
                    Shutdown();
                    return;
                }
                setting.RunningInformation.Accept = true;

                var isFirst = !settingResult.IsSuccess;
                if(isFirst) {
                    setting.RunningInformation.FirstVersion = Constants.ApplicationVersionNumber;
                    setting.RunningInformation.FirstTimestamp = DateTime.Now;
                }
            }
            if(setting.RunningInformation.FirstVersion == null) {
                setting.RunningInformation.FirstVersion = Constants.ApplicationVersionNumber;
                setting.RunningInformation.FirstTimestamp = DateTime.Now;
            }

            setting.RunningInformation.LastExecuteVersion = Constants.ApplicationVersionNumber;
            setting.RunningInformation.ExecuteCount = RangeUtility.Increment(setting.RunningInformation.ExecuteCount);

            SplashWindow.Show();
#if DEBUG
            SplashWindow.Topmost = false;
            SplashWindow.ShowInTaskbar = true; // 非最前面化でどっかいっちゃう対策
#endif

            var initializeTasks = new[] {
                InitializeCrashReportAsync(),
                AppManager.InitializeAsync(),
            };
            var initializeTask = Task.WhenAll(initializeTasks);
            await initializeTask;

            initializeTask.Dispose();
            foreach(var task in initializeTasks) {
                task.Dispose();
            }

            View = new MainWindow() {
                DataContext = AppManager,
            };
            MainWindow = View;
            MainWindow.Closed += MainWindow_Closed;
            AppManager.InitializeView(View);
            Exit += App_Exit;
            MainWindow.Loaded += MainWindow_Loaded;
            SplashWindow.commandClose.Visibility = Visibility.Collapsed;
            MainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Mutex.Dispose();

            AppDomain.CurrentDomain.UnhandledException -= CurrentDomain_UnhandledException;

            base.OnExit(e);
        }

#endregion

        /// <summary>
        /// UIスレッド
        /// </summary>
        void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException -= CurrentDomain_UnhandledException;
            CatchUnhandleException(e.Exception, true);
        }

        /// <summary>
        /// 非UIスレッド
        /// </summary>
        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            CatchUnhandleException((Exception)e.ExceptionObject, false);
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            WebNavigatorCore.Uninitialize();

            AppManager.UninitializeView(View);
        }

        private async void App_Exit(object sender, ExitEventArgs e)
        {
            Exit -= App_Exit;

            await AppManager.UninitializeAsync();
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            MainWindow.Loaded -= MainWindow_Loaded;

            // スプラッシュスクリーンさよなら～
            var hSplashWnd = HandleUtility.GetWindowHandle(SplashWindow);
            var exStyle = WindowsUtility.GetWindowLong(hSplashWnd, (int)GWL.GWL_EXSTYLE);
            WindowsUtility.SetWindowLong(hSplashWnd, (int)GWL.GWL_EXSTYLE, new IntPtr((int)exStyle | (int)WS_EX.WS_EX_TRANSPARENT));
            SplashWindow.IsHitTestVisible = false;
            SplashWindow.Topmost = false;

            var splashWindowAnimation = new DoubleAnimation(0, Constants.AppSplashCloseTime);
            splashWindowAnimation.Completed += (splashSender, splashEvent) => {
                SplashWindow.Close();
                SplashWindow = null;
            };
            SplashWindow.BeginAnimation(UIElement.OpacityProperty, splashWindowAnimation);
        }

    }
}
