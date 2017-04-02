﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using Microsoft.Win32.SafeHandles;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.App.Update
{
    public class AppUpdateDownloadItemViewModel : DownloadItemViewModel
    {
        public AppUpdateDownloadItemViewModel(Mediation mediation, Uri uri, FileInfo downloadFile, ICreateHttpUserAgent userAgentCreator)
            : base(mediation, uri, downloadFile, userAgentCreator)
        { }

        #region function

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

            Mediation.Logger.Information("update exec", process.StartInfo.Arguments);

            process.Start();

            Task.Run(() => {
                var processEvent = new EventWaitHandle(false, EventResetMode.AutoReset) {
                    SafeWaitHandle = new SafeWaitHandle(process.Handle, false),
                };
                var handles = new[] { waitEvent, processEvent };
                var waitResult = WaitHandle.WaitAny(handles, Constants.UpdateAppExitWaitTime);

                Mediation.Logger.Debug("WaitHandle.WaitAny", waitResult);
                if(0 <= waitResult && waitResult < handles.Length) {
                    if(handles[waitResult] == waitEvent) {
                        // イベントが立てられたので終了
                        Mediation.Logger.Information("exit", process.StartInfo.Arguments);
                        Application.Current.Dispatcher.Invoke(() => {
                            Mediation.Order(new OrderModel(OrderKind.Exit, ServiceType.Application));
                        });
                    } else if(handles[waitResult] == processEvent) {
                        // Updaterがイベント立てる前に死んだ
                        Mediation.Logger.Information("error-process", process.ExitCode);
                    }
                } else {
                    // タイムアウト
                    if(!process.HasExited) {
                        // まだ生きてるなら強制的に殺す
                        process.Kill();
                    }
                    Mediation.Logger.Information("error-timeout", process.ExitCode);
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
