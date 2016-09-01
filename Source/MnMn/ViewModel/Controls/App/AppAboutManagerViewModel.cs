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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.View.Controls;
using ContentTypeTextNet.Pe.Library.PeData.Item;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.App
{
    public class AppAboutManagerViewModel: ManagerViewModelBase
    {
        #region define

        static string separator = "____________";

        #endregion

        #region variable

        ComponentItemCollectionModel _componentCollection;

        #endregion

        public AppAboutManagerViewModel(Mediation mediation)
            : base(mediation)
        { }

        #region property

        public ComponentItemCollectionModel ComponentCollection
        {
            get
            {
                if(this._componentCollection == null) {
                    this._componentCollection = SerializeUtility.LoadXmlSerializeFromFile<ComponentItemCollectionModel>(Constants.ComponentListFileName);
                }

                return this._componentCollection;
            }
        }

        #endregion

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

        protected override IEnumerable<ManagerViewModelBase> GetManagerChildren()
        {
            return Enumerable.Empty<ManagerViewModelBase>();
        }

        protected override void ShowViewCore()
        { }

        protected override void HideViewCore()
        { }

        public override Task<long> GarbageCollectionAsync(GarbageCollectionLevel garbageCollectionLevel, CacheSpan cacheSpan)
        {
            return GarbageCollectionDummyResult;
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
