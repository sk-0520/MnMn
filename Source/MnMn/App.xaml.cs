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
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Order.AppProcessLink;
using ContentTypeTextNet.MnMn.MnMn.Model.ProcessLink;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Parameter;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting;
using ContentTypeTextNet.MnMn.MnMn.View.Controls;
using ContentTypeTextNet.MnMn.MnMn.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Service.Smile;
using ContentTypeTextNet.Pe.PeMain.Logic.Utility;
using Gecko;
using Trinet.Core.IO.Ntfs;

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

        /// <summary>
        /// 起動時間。
        /// </summary>
        DateTime WakeUpTimestamp { get; } = DateTime.Now;

        /// <summary>
        /// 二重起動抑制 Mutex。
        /// </summary>
        Mutex Mutex { get; set; } = new Mutex(false, Constants.ApplicationUsingName);

        /// <summary>
        /// スプラッシュスクリーン。
        /// </summary>
        SplashWindow SplashWindow { get; set; }

        /// <summary>
        /// 橋渡し。
        /// </summary>
        Mediation Mediation { get; set; }

        /// <summary>
        /// メインウィンドウ。
        /// </summary>
        MainWindow View { get; set; }

        /// <summary>
        /// マネージャ。
        /// </summary>
        AppManagerViewModel AppManager { get; set; }

        #endregion

        #region function

        /// <summary>
        /// 非ハンドリング例外の取得。
        /// </summary>
        /// <param name="exception">例外。</param>
        /// <param name="callerUiThread">UI スレッドで発生したか。</param>
        void CatchUnhandleException(Exception exception, bool callerUiThread)
        {
            Debug.WriteLine($"{nameof(callerUiThread)} = {callerUiThread}");
            if(Mediation != null && Mediation.Logger != null) {
                Mediation.Logger.Fatal(exception);
            } else {
                Debug.WriteLine(exception);
            }

            var reportPath = CreateCrashReport(exception, callerUiThread);
            var sendCrash = Constants.AppSendCrashReport;
#if KILL
            sendCrash = true;
#endif
            if(sendCrash) {
                var args = $"/crash /report=\"{reportPath}\"";
                args += $" /message=\"{exception.GetType().FullName}: {exception.Message}\"";
                if(Constants.AppCrashReportIsDebug) {
                    args += " /debug";
                }
                args += $" /reboot=\"{Constants.AssemblyPath}\"";
                if(VariableConstants.CommandLine.Length != 0) {
                    var arg = AppUtility.GetEscapedCommandLine();
                    args += $" /reboot-arg=\"{arg}\"";
                }
                Process.Start(Constants.CrashReporterApplicationPath, args);
            }

            Shutdown();
        }

        /// <summary>
        /// 設定ファイルの読み込み。
        /// </summary>
        /// <param name="logger">ロガー。</param>
        /// <returns>真: 読み込めた, 偽: 読み込めなかった。</returns>
        IReadOnlyCheckResult<AppSettingModel> LoadSetting(ILogger logger)
        {
            var dir = VariableConstants.GetSettingDirectory();
            var fileName = VariableConstants.SettingFileName;

            var filePath = Path.Combine(dir.FullName, fileName);
            var existsFile = File.Exists(filePath);

            if(VariableConstants.IsSafeModeExecute && existsFile) {
                // セーフモードはファイル初期化状態で起動させる
                File.Delete(filePath);
                existsFile = false;
            }

            var setting = SerializeUtility.LoadSetting<AppSettingModel>(filePath, SerializeFileType.Json, logger);

            return new CheckResultModel<AppSettingModel>(existsFile, setting, null, null);
        }

        /// <summary>
        /// 使用許諾状況のチェック。
        /// </summary>
        /// <param name="model">実行情報データ。</param>
        /// <param name="logger">ロガー。</param>
        /// <returns>許諾されている場合は真。偽の場合は非許諾状態か前回バージョン等々の都合で許諾されていない。</returns>
        public static bool CheckAccept(RunningInformationSettingModel model, IReadOnlyAcceptVersion acceptVersion, ILogger logger)
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

            if(model.LastExecuteVersion < acceptVersion.ForceAcceptVersion) {
                // 前回バージョンから強制定期に使用許諾が必要
                logger.Debug("last version < accept version");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 言語設定。
        /// </summary>
        /// <param name="cultureName">設定するカルチャ。</param>
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

        /// <summary>
        /// クラッシュレポートに設定データから抜粋情報を設定する。
        /// </summary>
        /// <param name="target"></param>
        void SetCrashReportSetting(CrashReportSettingModel target)
        {
            target.WakeUpTimestamp = WakeUpTimestamp;
            target.RunningTime = (DateTime.Now - WakeUpTimestamp).ToString();

            var setting = Mediation.GetResultFromRequest<AppSettingModel>(new RequestModel(RequestKind.Setting, ServiceType.Application));
            //var cacheDirectory = Mediation.GetResultFromRequest<DirectoryInfo>(new RequestModel(RequestKind.CacheDirectory, ServiceType.Application));
            target.UserId = setting.RunningInformation.UserId;

            target.CacheDirectoryPath = setting.CacheDirectoryPath;
            //target.UsingCacheDirectoryPath = cacheDirectory.FullName;
            target.CacheLifeTime = setting.CacheLifeTime.ToString();

            target.FirstVersion = setting.RunningInformation.FirstVersion?.ToString();
            target.FirstTimestamp = setting.RunningInformation.FirstTimestamp;
            target.LightweightUpdateTimestamp = setting.RunningInformation.LightweightUpdateTimestamp;

            target.GeckoFxScanPlugin = setting.WebNavigator.GeckoFxScanPlugin;

            // セッション
            var smileSession = Mediation.GetResultFromRequest<SmileSessionViewModel>(new RequestModel(RequestKind.Session, ServiceType.Smile));
            target.SmileSession.LoginState = smileSession.LoginState;
            if(smileSession.IsLoggedIn) {
                target.SmileSession.Extension.Text = $"{nameof(smileSession.IsPremium)} = {smileSession.IsPremium}, {nameof(smileSession.IsOver18)} = {smileSession.IsOver18}";
            }
            //SmileSession
        }

        IEnumerable<string> GetFilePathAndAds(string filePath)
        {
            yield return filePath;

            var streams = FileSystem.ListAlternateDataStreams(filePath);
            foreach(var stream in streams) {
                yield return stream.FullPath;
                yield return $"\t{nameof(stream.Size)}: {stream.Size}";
                yield return $"\t{nameof(stream.StreamType)}: {stream.StreamType}";
                yield return $"\t{nameof(stream.Attributes)}: {stream.Attributes}";
            }
        }

        /// <summary>
        /// クラッシュレポートに例外から拡張情報を設定する。
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="information"></param>
        void SetCrashReportInformation(CDataModel information, Exception ex)
        {
            // #465
            if(ex is DllNotFoundException) {
                var appDir = Constants.AssemblyRootDirectoryPath;
                var files = Directory.GetFiles(appDir, "*", SearchOption.AllDirectories)
                    .Select(f => GetFilePathAndAds(f))
                    .SelectMany(fs => fs)
                    .Select(f => appDir.Length <= f.Length ? f.Substring(appDir.Length): f)
                ;
                information.Text = string.Join(Environment.NewLine, files);
            }
        }

        /// <summary>
        /// クラッシュレポート作成。
        /// </summary>
        /// <param name="exception">例外。</param>
        /// <param name="callerUiThread">UI スレッドで発生したか。</param>
        /// <returns>生成したクラッシュレポートファイルのパス。</returns>
        string CreateCrashReport(Exception exception, bool callerUiThread)
        {
            var logParam = new AppLoggingParameterModel() {
                GetClone = true,
            };

            var logs = Mediation.GetResultFromRequest<IEnumerable<LogItemModel>>(new AppLogingProcessRequestModel(logParam))
                .Select(i => $"[{i.Timestamp:yyyy-MM-ddTHH:mm:ss.fff}] {i.Message}:  {i.CallerMember} ({i.CallerLine})" + (i.HasDetail ? (Environment.NewLine + i.DetailText): string.Empty) )
            ;

            var crashReport = new CrashReportModel(exception, callerUiThread);
            crashReport.Logs.Text = string.Join(Environment.NewLine, logs);
            SetCrashReportInformation(crashReport.Information, exception);
            SetCrashReportSetting(crashReport.Setting);

            var dir = VariableConstants.GetCrashReportDirectory();
            var path = Path.Combine(dir.FullName, PathUtility.AppendExtension(Constants.GetNowTimestampFileName(), Constants.CrashReportFileExtension));
            SerializeUtility.SaveXmlSerializeToFile(path, crashReport);

            return path;
        }

        /// <summary>
        /// 自身がβ版の際にβ版として明示的に実行されていない場合にユーザーから使用許可を取る。
        /// </summary>
        /// <returns>使用許可を得られたか。</returns>
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

        void ShowSafeModeFlag()
        {
            if(VariableConstants.IsSafeModeExecute) {
                var dialogResult = MessageBox.Show(
                    MnMn.Properties.Resources.String_App_ExecuteSafeMode_Information,
                    $"{Constants.ApplicationName}:{Constants.ApplicationVersion}",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information,
                    MessageBoxResult.OK,
                    MessageBoxOptions.DefaultDesktopOnly
                );
            }
        }

        async Task CheckAndRenewalUserIdAsync(AppSettingModel setting)
        {
            if(!AppUtility.ValidateUserId(setting.RunningInformation.UserId)) {
                var userId = await Task.Run(() => {
                    return AppUtility.CreateUserId(Mediation.Logger);
                });
                setting.RunningInformation.UserId = userId;
            }
        }

        /// <summary>
        /// 現在バージョンが前回バージョンより大きい場合通常アップデートが行われたと判断して簡易アップデート日時をリセットする。
        /// </summary>
        /// <param name="runningInformation"></param>
        void CheckAndResetLightweightUpdateVersion(RunningInformationSettingModel runningInformation)
        {
            if(runningInformation.LastExecuteVersion == null) {
                return;
            }

            var lastVersion = new Version(runningInformation.LastExecuteVersion.Major, runningInformation.LastExecuteVersion.Minor, runningInformation.LastExecuteVersion.Build);
            var nowVersion = new Version(Constants.ApplicationVersionNumber.Major, Constants.ApplicationVersionNumber.Minor, Constants.ApplicationVersionNumber.Build);
            if(lastVersion < nowVersion) {
                runningInformation.LightweightUpdateTimestamp = Constants.LightweightUpdateNone;
            }
        }

        static SystemParameterModel GetSystemParameter(ILogger logger)
        {
            var systemParameter = new SystemParameterModel();

            //var screenSaverFlag = 0;
            //NativeMethods.SystemParametersInfo(SPI.SPI_GETSCREENSAVEACTIVE, 0, ref screenSaverFlag, SPIF.None);
            //var isEnabledScreenSaver = screenSaverFlag != 0 ? true : false;

            //logger.Information($"SPI_GETSCREENSAVEACTIVE: {isEnabledScreenSaver}");
            //systemParameter.IsEnabledScreenSaver = isEnabledScreenSaver;

            return systemParameter;
        }

        static void RestoreSystemParameter(SystemParameterModel systemParameter, ILogger logger)
        {
            //if(systemParameter.IsEnabledScreenSaver.HasValue) {
            //    int tempIsEnabledScreenSaver = 0;
            //    logger.Information($"SPI_SETSCREENSAVEACTIVE: {systemParameter.IsEnabledScreenSaver}");
            //    NativeMethods.SystemParametersInfo(SPI.SPI_SETSCREENSAVEACTIVE, systemParameter.IsEnabledScreenSaver.Value ? 1u: 0u, ref tempIsEnabledScreenSaver, SPIF.SPIF_SENDCHANGE);
            //}
        }

        void InitializeEnvironmentVariables()
        {
            Environment.SetEnvironmentVariable("MNMN_DESKTOP", Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory));
        }

        void SendProccessLinkIfEnabledCommandLine(bool useWCF)
        {
            var isEnabledCommandLine = VariableConstants.HasOptionProcessLinkService && VariableConstants.HasOptionProcessLinkKey && VariableConstants.HasOptionProcessLinkKey;
            if(!isEnabledCommandLine) {
                Mediation.Logger.Trace("process link skip");
                return;
            }

            var rawService = VariableConstants.OptionValueProcessLinkService;
            ServiceType serviceType;
            try {
                var enumType = typeof(ServiceType);
                var enumAttr = EnumUtility.GetMembers<ServiceType>()
                    .Select(m => new { Member = m, Attributes = enumType.GetField(m.ToString()).GetCustomAttributes(false) })
                    .Select(i => new { Member = i.Member, Attributes = i.Attributes, XmlEnum = i.Attributes.OfType<System.Xml.Serialization.XmlEnumAttribute>().First() })
                ;
                var xmlEnumValue = enumAttr.FirstOrDefault(i => string.Equals(i.XmlEnum.Name, rawService, StringComparison.OrdinalIgnoreCase));
                if(xmlEnumValue != null) {
                    serviceType = xmlEnumValue.Member;
                } else {
                    serviceType = EnumUtility.Parse<ServiceType>(rawService);
                }
            } catch(Exception ex) {
                Mediation.Logger.Error(ex);
                return;
            }

            var key = VariableConstants.OptionValueProcessLinkKey;
            var value = VariableConstants.OptionValueProcessLinkValue;

            if(useWCF) {
                var client = new ProcessLinkClient(Mediation);
                client.Execute(serviceType, key, value);
            } else {
                var parameter = new ProcessLinkExecuteParameterModel() {
                    ServiceType = serviceType,
                    Key = key,
                    Value = value,
                };

                Mediation.Order(new AppProcessLinkParameterOrderModel(parameter));
            }
        }

        #endregion

        #region Application

        protected async override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
#if BETA
            if(!ShowBetaMessageIfNoBetaFlag()) {
                Shutdown();
            }
#endif
            ShowSafeModeFlag();

            var logger = new Pe.PeMain.Logic.AppLogger();
            logger.IsStock = true;

            InitializeEnvironmentVariables();

            // ログオプションのあれこれ
            if(VariableConstants.HasOptionLogDirectoryPath) {
                var logFilePath = Path.Combine(Environment.ExpandEnvironmentVariables(VariableConstants.OptionLogDirectoryPath), PathUtility.AppendExtension($"{Constants.GetNowTimestampFileName()}-{Constants.BuildType}", "log"));
                FileUtility.MakeFileParentDirectory(logFilePath);
                var writer = new StreamWriter(new FileStream(logFilePath, FileMode.Create, FileAccess.Write, FileShare.Read)) {
                    AutoFlush = true,
                };
                logger.AttachStream(writer, true);
                logger.LoggerConfig.PutsStream = true;
            }

            var stream = GlobalManager.MemoryStream.GetStreamWidthAutoTag();
            using(var writer = new StreamWriter(stream)) {
                var appInfo = new AppInformationCollection(null);
                appInfo.WriteInformation(writer);
                var s = Encoding.UTF8.GetString(stream.GetBuffer(), 0, (int)stream.Length);
                logger.Information(Constants.ApplicationName, s);
            }

            logger.Information($"mutex check: {Constants.ApplicationUsingName}");
            if(!Mutex.WaitOne(Constants.MutexWaitTime, false)) {
                logger.Warning($"{Constants.ApplicationUsingName} is opened"); ;
                Mutex = null;

                SendProccessLinkIfEnabledCommandLine(true);

                Shutdown();
                return;
            }
            logger.Information("mutex ok");

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

            // セーフモードの場合許諾ウィンドウは表示しない
            if(!VariableConstants.IsSafeModeExecute) {
                var acceptVersion = SerializeUtility.LoadXmlSerializeFromFile<AcceptVersionModel>(Constants.ApplicationAcceptVersionPath);
                if(!CheckAccept(setting.RunningInformation, acceptVersion, logger)) {
                    var acceptViewModel = new AcceptViewModel(Mediation, acceptVersion);
                    var acceptWindow = new AcceptWindow() {
                        DataContext = acceptViewModel,
                    };
                    acceptViewModel.SetView(acceptWindow);
                    acceptViewModel.Initialize();

                    var acceptResult = acceptWindow.ShowDialog();
                    if(!acceptResult.GetValueOrDefault()) {
                        Shutdown();
                        return;
                    }

                    var isFirst = !settingResult.IsSuccess;
                    if(isFirst) {
                        setting.RunningInformation.FirstVersion = Constants.ApplicationVersionNumber;
                        setting.RunningInformation.FirstTimestamp = DateTime.Now;
                    }
                }
            }

            if(setting.RunningInformation.FirstVersion == null) {
                setting.RunningInformation.FirstVersion = Constants.ApplicationVersionNumber;
                setting.RunningInformation.FirstTimestamp = DateTime.Now;
            }

            CheckAndResetLightweightUpdateVersion(setting.RunningInformation);

            setting.RunningInformation.LastExecuteVersion = Constants.ApplicationVersionNumber;
            setting.RunningInformation.ExecuteCount = RangeUtility.Increment(setting.RunningInformation.ExecuteCount);

            SplashWindow.Show();
#if DEBUG
            SplashWindow.Topmost = false;
            SplashWindow.ShowInTaskbar = true; // 非最前面化でどっかいっちゃう対策
#endif
            await CheckAndRenewalUserIdAsync(setting);

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
            MainWindow.Closing += MainWindow_Closing;
            MainWindow.Closed += MainWindow_Closed;
            AppManager.InitializeView(View);
            Exit += App_Exit;
            MainWindow.Loaded += MainWindow_Loaded;
            SplashWindow.commandClose.Visibility = Visibility.Collapsed;
            MainWindow.ShowActivated = SplashWindow.IsActive;
            MainWindow.Show();

            // ここで現在情報取得！
            GlobalManager.SystemParameter = GetSystemParameter(Mediation.Logger);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if(Mutex != null) {
                Mutex.ReleaseMutex();
                Mutex.Dispose();
                Mutex = null;
            }

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
            if(Constants.AppSendCrashReport) {
                e.Handled = Constants.AppUnhandledExceptionHandled;
            }
        }

        /// <summary>
        /// 非UIスレッド
        /// </summary>
        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            CatchUnhandleException((Exception)e.ExceptionObject, false);
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Mediation.Logger.Trace("start closing!");

            var uninitTask = AppManager.UninitializeAsync();
            if(!uninitTask.Wait(Constants.UninitializeAsyncWaitTime)) {
                Mediation.Logger.Fatal($"time out: {nameof(ManagerViewModelBase.UninitializeAsync)}");
            }

            Mediation.Logger.Trace("end closing -> start close!");
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            WebNavigatorCore.Uninitialize();

            Mediation.Order(new AppProcessLinkStateChangeOrderModel(ProcessLinkState.Shutdown));

            AppManager.UninitializeView(View);

            if(VariableConstants.IsSafeModeExecute) {
                var dir = VariableConstants.GetSettingDirectory();
                var fileName = VariableConstants.SettingFileName;

                var filePath = Path.Combine(dir.FullName, fileName);
                if(File.Exists(filePath)) {
                    ShellUtility.OpenFileInDirectory(new FileInfo(filePath), Mediation.Logger);
                }
            }

            RestoreSystemParameter(GlobalManager.SystemParameter, Mediation.Logger);
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            Exit -= App_Exit;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            MainWindow.Loaded -= MainWindow_Loaded;

            // ホストを有効にする
            Mediation.Order(new AppProcessLinkStateChangeOrderModel(ProcessLinkState.Listening));

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

                SendProccessLinkIfEnabledCommandLine(false);
            };
            SplashWindow.BeginAnimation(UIElement.OpacityProperty, splashWindowAnimation);
        }

    }
}
