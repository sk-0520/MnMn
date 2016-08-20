using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.View.Controls;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.App
{
    public class AppAboutManagerViewModel: ManagerViewModelBase
    {
        #region define

        static string separator = "____________";

        #endregion

        public AppAboutManagerViewModel(Mediation mediation)
            : base(mediation)
        { }

        #region command

        public ICommand OpenLinkCommand
        {
            get
            {
                return CreateCommand(o => {
                    var s = (string)o;
                    if(s.Any(c => c == '@')) {
                        var mail = "mailto:" + s;
                        s = mail;
                    }
                    Execute(s);
                });
            }
        }

        public ICommand CopyShortInformationCommand
        {
            get
            {
                return CreateCommand(o => {
                    var list = new List<string>();
                    list.Add("Software: " + Constants.ApplicationName);
                    list.Add("Version: " + Constants.ApplicationVersion);
                    list.Add("BuildType: " + Constants.BuildType);
                    //list.Add("Process: " + Constants.BuildProcess);
                    list.Add("Platform: " + (Environment.Is64BitOperatingSystem ? "64" : "32"));
                    list.Add("OS: " + System.Environment.OSVersion);
                    list.Add("CLR: " + System.Runtime.InteropServices.RuntimeEnvironment.GetSystemVersion());
                    var text = Environment.NewLine + separator + Environment.NewLine + string.Join(Environment.NewLine, list.Select(s => "    " + s)) + Environment.NewLine + Environment.NewLine;
                    Clipboard.SetText(text);
                });
            }
        }

        public ICommand CopyLongInformationCommand
        {
            get
            {
                return CreateCommand(o => {
                    var appInfo = new AppInformationCollection();
                    var text
                        = Environment.NewLine
                        + separator
                        + Environment.NewLine
                        + string.Join(
                            Environment.NewLine,
                            appInfo.ToString()
                                .SplitLines()
                                .Select(s => "    " + s)
                        )
                        + Environment.NewLine
                        + Environment.NewLine
                    ;
                    Clipboard.SetText(text);
                });
            }
        }

        public ICommand OpenAppDirectoryCommand
        {
            get
            {
                return CreateCommand(o => {
                    Execute(Constants.AssemblyRootDirectoryPath);
                });
            }
        }

        public ICommand OpenSettingDirectoryCommand
        {
            get
            {
                return CreateCommand(o => {
                    Execute(VariableConstants.GetSettingDirectory().FullName);
                });
            }
        }

        public ICommand OpenCacheDirectoryCommand
        {
            get
            {
                return CreateCommand(o => {
                    var dir = Mediation.GetResultFromRequest<DirectoryInfo>(new RequestModel(RequestKind.CacheDirectory, ServiceType.Application));
                    Execute(dir.FullName);
                });
            }
        }

        #endregion

        #region function

        void Execute(string command)
        {
            try {
                Process.Start(command);
            } catch(Exception ex) {
                Mediation.Logger.Error(ex);
            }
        }

        #endregion

        #region ManagerViewModelBase

        public override Task GarbageCollectionAsync()
        {
            return Task.CompletedTask;
        }

        public override Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public override void InitializeView(MainWindow view)
        {
        }

        public override void UninitializeView(MainWindow view)
        {
        }

        #endregion
    }
}
