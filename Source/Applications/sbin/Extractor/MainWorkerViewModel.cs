using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using Microsoft.CSharp;
using Microsoft.Win32;

namespace ContentTypeTextNet.MnMn.SystemApplications.Extractor
{
    public class MainWorkerViewModel : ViewModelBase
    {
        #region define

        delegate bool TryParse<TValue>(string input, out TValue result);
        const string scriptFileName = "UpdaterScript.cs";
        const int expandRertyMaxCount = 5;
        readonly TimeSpan expandRetryWaitTime = TimeSpan.FromSeconds(3);
        readonly TimeSpan closedWaitTime = TimeSpan.FromSeconds(5);

        #endregion

        #region variable

        bool _canInput = true;

        bool _autoExecute;
        bool _isEnabledAutoExecute = true;

        string _archiveFilePath;
        string _expandDirectoryPath;

        string _platform;

        bool _canExecute;

        #endregion

        public MainWorkerViewModel()
        { }

        #region property

        MainWindow View { get; set; }
        ListBox ListLog { get; set; }

        TextWriter Writer { get; set; }
        string LogFilePath { get; set; }

        bool IsRenamed { get; set; }

        public CollectionModel<LogItemViewModel> LogItems { get; } = new CollectionModel<LogItemViewModel>();

        public bool CanInput
        {
            get { return this._canInput; }
            set { SetVariableValue(ref this._canInput, value); }
        }

        public string ArchiveFilePath
        {
            get { return this._archiveFilePath; }
            set { SetVariableValue(ref this._archiveFilePath, value); }
        }

        public string ExpandDirectoryPath
        {
            get { return this._expandDirectoryPath; }
            set { SetVariableValue(ref this._expandDirectoryPath, value); }
        }

        public bool AutoExecute
        {
            get { return this._autoExecute; }
            set { SetVariableValue(ref this._autoExecute, value); }
        }

        public bool IsEnabledAutoExecute
        {
            get { return this._isEnabledAutoExecute; }
            set { SetVariableValue(ref this._isEnabledAutoExecute, value); }
        }

        string EventName { get; set; }
        int ProcessId { get; set; }

        string RebootApplicationPath { get; set; }
        string RebootApplicationCommandLine { get; set; }

        string ScriptFilePath { get; set; }

        public string Platform
        {
            get { return this._platform; }
            set { SetVariableValue(ref this._platform, value); }
        }

        public bool CanExecute
        {
            get { return this._canExecute; }
            set { SetVariableValue(ref this._canExecute, value); }
        }

        #endregion

        #region command

        public ICommand CloseCommand
        {
            get
            {
                return CreateCommand(
                    o => View.Close(),
                    o => CanInput
                );
            }
        }

        public ICommand ExecuteCommand
        {
            get
            {
                return CreateCommand(
                    o => ExecuteAsync(),
                    o => CanInput && !string.IsNullOrWhiteSpace(ArchiveFilePath) && !string.IsNullOrWhiteSpace(ExpandDirectoryPath)
                );
            }
        }

        public ICommand OpenLogFileCommand
        {
            get
            {
                return CreateCommand(
                    o => OpenLogFile(),
                    o => LogItems.Any()
                );
            }
        }

        public ICommand CopyLogsCommand
        {
            get
            {
                return CreateCommand(
                    o => CopyLogs(),
                    o => LogItems.Any()
                );
            }
        }

        public ICommand ExecuteApplicationCommand
        {
            get
            {
                return CreateCommand(
                    o => ExecuteApplication(),
                    o => CanExecute && File.Exists(RebootApplicationPath)
                );
            }
        }

        #endregion

        #region function

        public void SetView(MainWindow view)
        {
            View = view;
            ListLog = View.listLog;

            View.ContentRendered += View_ContentRendered;
            View.Closed += View_Closed;
        }

        string GetCommandValue(CommandLine commandLine, string option)
        {
            if(commandLine.HasValue(option)) {
                return commandLine.GetValue(option);
            }

            return string.Empty;
        }

        TValue GetCommandValue<TValue>(CommandLine commandLine, string option, TryParse<TValue> tryParse, TValue defaultResult = default(TValue))
        {
            var value = GetCommandValue(commandLine, option);

            TValue result;
            if(tryParse(value, out result)) {
                return result;
            }

            return defaultResult;
        }

        void InitializeFromCommandLine()
        {
            var commandLine = new CommandLine();

            ArchiveFilePath = GetCommandValue(commandLine, "archive");
            ExpandDirectoryPath = GetCommandValue(commandLine, "expand");

            AutoExecute = GetCommandValue(commandLine, "auto", bool.TryParse, false);

            EventName = GetCommandValue(commandLine, "event");
            ProcessId = GetCommandValue(commandLine, "pid", int.TryParse, 0);

            RebootApplicationPath = GetCommandValue(commandLine, "reboot");
            RebootApplicationCommandLine = GetCommandValue(commandLine, "reboot-arg");

            ScriptFilePath = GetCommandValue(commandLine, "script");
            Platform = GetCommandValue(commandLine, "platform");
        }

        void InitializeCore()
        {
            InitializeFromCommandLine();
        }

        public void Initialize()
        {
            if(View == null) {
                throw new InvalidOperationException(nameof(View));
            }

            InitializeCore();
        }

        void KillProcess(Process process, bool closeEventName)
        {
            EventWaitHandle eventHandle = null;
            if(closeEventName) {
                eventHandle = EventWaitHandle.OpenExisting(EventName);
            }
            if(eventHandle != null && closeEventName) {
                AddInformationLog("event set");
                eventHandle.Set();
            } else {
                AddInformationLog("process kill");
                process.Kill();
            }
        }

        void CloseProcess()
        {
            if(ProcessId == 0) {
                return;
            }

            var killStopwatch = new Stopwatch();
            killStopwatch.Start();

            var process = Process.GetProcessById(ProcessId);
            if(process != null) {
                process.Exited += (object sender, EventArgs e) => {
                    killStopwatch.Stop();
                };
                KillProcess(process, true);

                AddInformationLog("Wait: process exit...");
                var isRestart = !process.WaitForExit((int)(TimeSpan.FromMinutes(3).TotalMilliseconds));
                AddInformationLog($"Exit: {!isRestart}");
                if(isRestart && !process.HasExited) {
                    AddInformationLog($"Kill: process");
                    KillProcess(process, false);
                }
                AddInformationLog($"Close: HasExited = {process.HasExited}, time = {killStopwatch.Elapsed}");
                AddInformationLog($"Wait: sleep({closedWaitTime})");
                Thread.Sleep((int)this.closedWaitTime.TotalMilliseconds);
            }
        }

        void ExpandEntry(ZipArchiveEntry entry, string expandPath)
        {
            AddInformationLog($"Expand: {expandPath}", $"name = {entry.Name}, length = {entry.Length}");
            Exception lastException;
            var i = 0;
            do {
                try {
                    entry.ExtractToFile(expandPath, true);
                    return;
                } catch(Exception ex) {
                    AddWarningLog($"{i + 1}/{expandRertyMaxCount}{ex.Message}", ex.ToString());
                    lastException = ex;
                }
                if(i + 1 < expandRertyMaxCount) {
                    AddInformationLog($"{i + 1}/{expandRertyMaxCount}, wait...");
                    Thread.Sleep((int)this.expandRetryWaitTime.TotalMilliseconds);
                }
            } while(++i < expandRertyMaxCount);

            throw lastException;
        }

        void ExpandArchive()
        {
            // 自身の名前を切り替え
            var myPath = Assembly.GetEntryAssembly().Location;
            var myDir = Path.GetDirectoryName(myPath);

            var renamePath = Path.ChangeExtension(myPath, "expand-old");
            if(!IsRenamed) {
                if(File.Exists(renamePath)) {
                    File.Delete(renamePath);
                }
                AddInformationLog($"Rename: {myPath} => {renamePath}");
                File.Move(myPath, renamePath);
                IsRenamed = true;
            }

            // 置き換え開始
            using(var archive = new ZipArchive(new FileStream(ArchiveFilePath, FileMode.Open, FileAccess.Read, FileShare.None), ZipArchiveMode.Read)) {
                foreach(var entry in archive.Entries.Where(e => e.Name.Length > 0)) {
                    var expandPath = Path.Combine(ExpandDirectoryPath, entry.FullName);
                    var dirPath = Path.GetDirectoryName(expandPath);
                    if(!Directory.Exists(dirPath)) {
                        Directory.CreateDirectory(dirPath);
                    }
                    ExpandEntry(entry, expandPath);
                }
            }

            // Updater使用バージョンの場合は展開プログラムがないので下位互換として現在処理中展開プログラムを実行可能な状態にしておく
            if(!File.Exists(myPath)) {
                AddInformationLog($"Restore: {renamePath} => {myPath}");
                File.Copy(renamePath, myPath, true);
            }

        }

        void AppendAssembly(CompilerParameters parameters, string dllName)
        {
            if(!parameters.ReferencedAssemblies.Contains(dllName)) {
                parameters.ReferencedAssemblies.Add(dllName);
            } else {
                AddInformationLog($"Overlap: {dllName}");
            }
        }

        void RunScript()
        {
            if(!File.Exists(ScriptFilePath)) {
                return;
            }

            using(var compiler = new CSharpCodeProvider(new Dictionary<string, string>() {
                {"CompilerVersion", "v4.0" }
            })) {
                var scriptText = File.ReadAllText(ScriptFilePath);

                var parameters = new CompilerParameters();
                parameters.GenerateExecutable = false;
                parameters.GenerateInMemory = true;
                parameters.IncludeDebugInformation = true;
                parameters.TreatWarningsAsErrors = true;
                parameters.WarningLevel = 4;
                //parameters.CompilerOptions = string.Format("/platform:{0}", Platform);

                // 最低限のアセンブリは読み込ませる
                var asmList = new[] {
                    "mscorlib.dll",
                    "System.dll",
                    "System.Core.dll",
                    "System.Data.dll"
                };
                foreach(var dllName in asmList) {
                    AppendAssembly(parameters, dllName);
                }

                // //+DLL:*.dll読み込み
                var regTargetDll = new Regex(@"^//\+DLL\s*:\s*(?<DLL>.*\.dll)", RegexOptions.Multiline | RegexOptions.IgnoreCase);
                foreach(Match match in regTargetDll.Matches(scriptText)) {
                    var dllName = match.Groups["DLL"].Value;
                    AppendAssembly(parameters, dllName);
                }

                // /*-*/using xxx は読み込み無視
                var regUsingDll = new Regex(@"[^(/*-*/)]\s*using\s+(?<NAME>.+)\s*;", RegexOptions.Multiline);
                foreach(Match match in regUsingDll.Matches(scriptText)) {
                    var name = match.Groups["NAME"].Value;
                    if(name.Any(c => c == '=')) {
                        name = name.Split('=').Last().Trim();
                    }
                    var dllName = name + ".dll";
                    AppendAssembly(parameters, dllName);
                }
                foreach(var asm in parameters.ReferencedAssemblies) {
                    AddInformationLog($"Assembly = {asm}");
                }

                var cr = compiler.CompileAssemblyFromSource(parameters, scriptText);

#if DEBUG
                if(cr.Output.Count > 0) {
                    foreach(var output in cr.Output) {
                        AddInformationLog(output.ToString());
                    }
                }
#endif
                if(cr.Errors.Count > 0) {
                    AddErrorLog("error:");
                    foreach(var error in cr.Errors) {
                        AddErrorLog(error.ToString());
                    }
                    throw new Exception("compile");
                }

                var assembly = cr.CompiledAssembly;

                using(var writer = new ConsoleWriter()) {
                    writer.WriteLineAction = s => {
                        AddScriptLog(s);
                    };
                    Console.SetOut(writer);

                    var us = assembly.CreateInstance("UpdaterScript");
                    us.GetType().GetMethod("Main").Invoke(us, new object[] { new [] {
                        ScriptFilePath,
                        ExpandDirectoryPath,
                        Platform
                    }});
                }
            }
        }

        Task ExecuteAsync()
        {
            CanInput = false;
            CanExecute = false;

            if(Writer == null) {
                var fileName = $"extractor_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.log";
                var dirPath = Path.Combine(Environment.ExpandEnvironmentVariables("%APPDATA%"), "MnMn", "extractor");
                Directory.CreateDirectory(dirPath);
                LogFilePath = Path.Combine(dirPath, fileName);
                var stream = new FileStream(LogFilePath, FileMode.Create, FileAccess.Write, FileShare.Read);
                Writer = new StreamWriter(stream) {
                    AutoFlush = true,
                };
            }

            try {
                if(!File.Exists(ArchiveFilePath)) {
                    throw new Exception($"not found {ArchiveFilePath}");
                }
                if(!Directory.Exists(ExpandDirectoryPath)) {
                    throw new Exception($"not found {ExpandDirectoryPath}");
                }
            } catch(Exception ex) {
                AddErrorLog(ex.ToString());
                CanInput = true;
                return Task.CompletedTask;
            }

            return Task.Run(() => {
                CloseProcess();
            }).ContinueWith(t => {
                if(!t.IsFaulted) {
                    ExpandArchive();
                } else {
                    throw t.Exception;
                }
            }).ContinueWith(t => {
                if(!t.IsFaulted) {
                    RunScript();
                    AddInformationLog(";-)");
                } else {
                    throw t.Exception;
                }
            }).ContinueWith(async t => {
                if(!t.IsFaulted) {
                    if(AutoExecute) {
                        IsEnabledAutoExecute = false;
                        AutoExecute = false;

                        ExecuteApplication();

                        await Task.Delay(TimeSpan.FromSeconds(5));

                        Application.Current.Shutdown();
                    } else {
                        CanExecute = true;
                    }
                } else {
                    AddErrorLog(t.Exception.ToString());
                }

                CanInput = true;
                CommandManager.InvalidateRequerySuggested();

            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        void ExecuteApplication()
        {
            try {
                Process.Start(RebootApplicationPath, RebootApplicationCommandLine);
            } catch(Exception ex) {
                AddErrorLog(ex.Message, ex.ToString());
            }
        }

        void WriteStream(Stream stream)
        {
            using(var writer = new StreamWriter(stream, Encoding.UTF8, 4096, true)) {
                foreach(var item in LogItems) {
                    writer.WriteLine($"[{item.Timestamp:yyyy-MM-ddThh:mm:ss}] {item.Kind}: {item.Message}");
                    if(item.HasDetail) {
                        writer.WriteLine(item.Detail);
                    }
                }
            }
        }

        void OutputLog(string outputFilePath)
        {
            var dirPath = Path.GetDirectoryName(outputFilePath);
            Directory.CreateDirectory(dirPath);
            using(var stream = new FileStream(outputFilePath, FileMode.Create, FileAccess.ReadWrite, FileShare.Read)) {
                WriteStream(stream);
            }
        }

        [Obsolete]
        void OutputLogsFromDialog()
        {
            var dialog = new SaveFileDialog() {
                Filter = "log|*.log",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                FileName = $"{DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss")}_expand.log",
                CheckPathExists = true,
            };
            if(dialog.ShowDialog().GetValueOrDefault()) {
                OutputLog(dialog.FileName);
            }
        }

        void OpenLogFile()
        {
            try {
                Process.Start("explorer", $"/select,\"{LogFilePath}\"");
            } catch(Exception ex) {
                AddErrorLog(ex.Message, ex.ToString());
                try {
                    var dirPath = Path.GetDirectoryName(LogFilePath);
                    Process.Start(dirPath);
                } catch(Exception ex2) {
                    AddErrorLog(ex2.Message, ex2.ToString());
                }
            }
        }

        void CopyLogs()
        {
            using(var stream = new MemoryStream()) {
                WriteStream(stream);
                try {
                    var text = Encoding.UTF8.GetString(stream.ToArray());
                    Clipboard.SetText(text);
                } catch(Exception ex) {
                    AddErrorLog(ex.Message, ex.ToString());
                }
            }
        }

        #region function

        void AddInformationLog(string message, string detail = null)
        {
            var log = new LogItemViewModel(LogKind.Information, message, detail);
            AddLog(log);
        }

        void AddWarningLog(string message, string detail = null)
        {
            var log = new LogItemViewModel(LogKind.Warning, message, detail);
            AddLog(log);
        }

        void AddErrorLog(string message, string detail = null)
        {
            var log = new LogItemViewModel(LogKind.Error, message, detail);
            AddLog(log);
        }

        void AddScriptLog(string message)
        {
            var log = new LogItemViewModel(LogKind.Script, message, null);
            AddLog(log);
        }

        void AddLog(LogItemViewModel log)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() => {
                LogItems.Add(log);
                ListLog.ScrollIntoView(log);
                var header = $"[{log.Timestamp:yyyy-MM-dd_HH-mm-ss}] {log.Kind}";
                var splitter = ": ";
                Writer.WriteLine($"{header}{splitter}{log.Message}");
                if(log.HasDetail) {
                    Writer.Write(new String(' ', header.Length + splitter.Length));
                    Writer.WriteLine(log.Detail);
                }
            }));
        }

        #endregion

        #endregion

        private void View_ContentRendered(object sender, EventArgs e)
        {
            View.ContentRendered -= View_ContentRendered;

            if(AutoExecute) {
                ExecuteAsync();
            }
        }

        private void View_Closed(object sender, EventArgs e)
        {
            Writer?.Dispose();
        }

    }
}
