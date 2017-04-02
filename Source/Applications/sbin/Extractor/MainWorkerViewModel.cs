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
using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using Microsoft.CSharp;

namespace ContentTypeTextNet.MnMn.SystemApplications.Extractor
{
    public class MainWorkerViewModel:ViewModelBase
    {
        #region define

        delegate bool TryParse<TValue>(string input, out TValue result);
        const string scriptFileName = "UpdaterScript.cs";

        #endregion

        #region variable

        bool _canInput=true;

        string _archiveFilePath;
        string _expandDirectoryPath;

        #endregion

        #region property

        MainWindow View { get; set; }

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

        bool AutoExecute { get; set; }
        string EventName { get; set; }
        int ProcessId { get; set; }

        string RebootApplicationPath { get; set; }
        string RebootApplicationCommandLine { get; set; }

        string ScriptFilePath { get; set; }
        string Platform { get; set; }

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
                    o => CanInput
                );
            }
        }

        #endregion

        #region function

        public void SetView(MainWindow view)
        {
            View = view;

            View.ContentRendered += View_ContentRendered;
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

        void InitializeFromCommandLIne()
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
        }

        void InitializeCore()
        {
            InitializeFromCommandLIne();
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
                Debug.WriteLine("event set");
                eventHandle.Set();
            } else {
                Debug.WriteLine("process kill");
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
                KillProcess(process ,true);

                var isRestart = process.WaitForExit((int)(TimeSpan.FromMinutes(1).TotalMilliseconds));
                if(isRestart && !process.HasExited) {
                    KillProcess(process, false);
                }
                Debug.WriteLine("Kill -> {0}, Time = {1}", isRestart, killStopwatch.Elapsed);
            }
        }

        void ExpandArchive()
        {
            // 自身の名前を切り替え
            var myPath = Assembly.GetEntryAssembly().Location;
            var myDir = Path.GetDirectoryName(myPath);

            var renamePath = Path.ChangeExtension(myPath, "expand-old");
            if(File.Exists(renamePath)) {
                File.Delete(renamePath);
            }
            Debug.WriteLine("Rename -> {0} => {1}", myPath, renamePath);
            File.Move(myPath, renamePath);

            // 置き換え開始
            using(var archive = ZipFile.OpenRead(ArchiveFilePath)) {
                foreach(var entry in archive.Entries.Where(e => e.Name.Length > 0)) {
                    var expandPath = Path.Combine(ExpandDirectoryPath, entry.FullName);
                    var dirPath = Path.GetDirectoryName(expandPath);
                    if(!Directory.Exists(dirPath)) {
                        Directory.CreateDirectory(dirPath);
                    }
                    Debug.WriteLine($"Expand -> {expandPath}");
                    entry.ExtractToFile(expandPath, true);
                }
            }
        }

        void AppendAssembly(CompilerParameters parameters, string dllName)
        {
            if(!parameters.ReferencedAssemblies.Contains(dllName)) {
                parameters.ReferencedAssemblies.Add(dllName);
            } else {
                Console.WriteLine("Overlap: {0}", dllName);
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
                    Console.WriteLine("Assembly = {0}", asm);
                }

                var cr = compiler.CompileAssemblyFromSource(parameters, scriptText);

#if DEBUG
                if(cr.Output.Count > 0) {
                    foreach(var output in cr.Output) {
                        Debug.Write(output);
                    }
                }
#endif
                if(cr.Errors.Count > 0) {
                    Debug.Write("error:");
                    foreach(var error in cr.Errors) {
                        Debug.Write(error);
                    }
                    throw new Exception("compile");
                }

                var assembly = cr.CompiledAssembly;

                var us = assembly.CreateInstance("UpdaterScript");
                us.GetType().GetMethod("ExpandMain").Invoke(us, new object[] { new [] {
                    ScriptFilePath,
                    ExpandDirectoryPath
                }});
            }
        }

        Task ExecuteAsync()
        {
            CanInput = false;

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
                } else {
                    throw t.Exception;
                }
            }).ContinueWith(t => {
                if(!t.IsFaulted) {
                    Process.Start(RebootApplicationPath, RebootApplicationCommandLine);

                    if(AutoExecute) {
                        //Application.Current.Shutdown();
                    }
                }
                CanInput = true;
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        #endregion

        private void View_ContentRendered(object sender, EventArgs e)
        {
            View.ContentRendered -= View_ContentRendered;

            if(AutoExecute) {
                ExecuteAsync();
            }
        }


    }
}
