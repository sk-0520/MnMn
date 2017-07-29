using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ContentTypeTextNet.Library.PInvoke.Windows;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using Microsoft.Win32.SafeHandles;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.App.Update
{
    public class AppUpdateDownloadItemViewModel : DownloadItemViewModel
    {
        public AppUpdateDownloadItemViewModel(Mediator mediator, Uri uri, FileInfo downloadFile, IHttpUserAgentCreator userAgentCreator)
            : base(mediator, uri, downloadFile, userAgentCreator)
        { }

        #region function

        void SetStartInfoEnvironment(ProcessStartInfo startInfo)
        {
            // #552@自身の Path は除外する
            var myDirPath = Constants.AssemblyRootDirectoryPath;
            if(myDirPath.Last() == Path.DirectorySeparatorChar) {
                myDirPath = myDirPath.Substring(0, myDirPath.Length - 1);
            }
            var pathValue = startInfo.Environment["PATH"];
            Mediator.Logger.Information("default path", pathValue);
            var pathItems = pathValue.Split(';')
                .Where(s => !s.StartsWith(myDirPath, StringComparison.OrdinalIgnoreCase))
            ;
            var newPathValue = string.Join(";", pathItems);
            Mediator.Logger.Information("update path", newPathValue);
            startInfo.Environment["PATH"] = newPathValue;
        }

        void ExceuteUpdate()
        {
            var eventName = "mnmn-event";
            var waitEvent = new EventWaitHandle(false, EventResetMode.AutoReset, eventName);

            var process = new Process();
            var startInfo = process.StartInfo;
            startInfo.FileName = Constants.ExtractorExecuteFilePath;

            var argsMap = new Dictionary<string, string>() {
                { "archive",    DownloadFile.FullName },
                { "expand",     Constants.AssemblyRootDirectoryPath },
                { "auto",       "true" },
                { "pid",        string.Format("{0}", Process.GetCurrentProcess().Id) },
                { "event",      eventName },
                { "reboot",     Constants.AssemblyPath },
                { "reboot-arg", AppUtility.GetRawCommandLine() },
                { "script",     Path.Combine(Constants.ApplicationEtcDirectoryPath, Constants.ScriptDirectoryName, "Updater", "UpdaterScript.cs") },
                { "platform",   Environment.Is64BitProcess ? "x64": "x86" },
            };
            startInfo.Arguments = string.Join(" ", argsMap.Select(p => $"/{p.Key}=\"{p.Value}\""));

            SetStartInfoEnvironment(startInfo);

            startInfo.WorkingDirectory = Path.GetDirectoryName(startInfo.FileName);

            startInfo.UseShellExecute = false;

            Mediator.Logger.Information("update exec", process.StartInfo.Arguments);

            // 光り輝く #552!
            process.StartWithDllReset();

            Task.Run(() => {
                var processEvent = new EventWaitHandle(false, EventResetMode.AutoReset) {
                    SafeWaitHandle = new SafeWaitHandle(process.Handle, false),
                };
                var handles = new[] { waitEvent, processEvent };
                var waitResult = WaitHandle.WaitAny(handles, Constants.UpdateAppExitWaitTime);

                Mediator.Logger.Debug("WaitHandle.WaitAny", waitResult);
                if(0 <= waitResult && waitResult < handles.Length) {
                    if(handles[waitResult] == waitEvent) {
                        // イベントが立てられたので終了
                        Mediator.Logger.Information("exit", process.StartInfo.Arguments);
                        Application.Current.Dispatcher.Invoke(() => {
                            Mediator.Order(new OrderModel(OrderKind.Exit, ServiceType.Application));
                        });
                    } else if(handles[waitResult] == processEvent) {
                        // Updaterがイベント立てる前に死んだ
                        Mediator.Logger.Information("error-process", process.ExitCode);
                    }
                } else {
                    // タイムアウト
                    if(!process.HasExited) {
                        // まだ生きてるなら強制的に殺す
                        process.Kill();
                    }
                    Mediator.Logger.Information("error-timeout", process.ExitCode);
                }
            });
        }

        #endregion

        #region DownloadItemViewModel

        public override ImageSource Image
        {
            get
            {
                var image = new BitmapImage();
                using(Initializer.BeginInitialize(image)) {
                    image.UriSource = SharedConstants.GetEntryUri("Resources/MnMn-Update_App.png");
                }

                return image;
            }
        }

        public override ICommand OpenDirectoryCommand => CreateCommand(o => { }, o => false);

        public override ICommand ExecuteTargetCommand => CreateCommand(o => { }, o => false);

        public override ICommand AutoExecuteTargetCommand
        {
            get
            {
                return CreateCommand(o => ExceuteUpdate(), o => true);
            }
        }

        #endregion
    }
}
