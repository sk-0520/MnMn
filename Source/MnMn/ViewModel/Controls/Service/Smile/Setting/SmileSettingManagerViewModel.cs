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
using System.Windows.Input;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.View.Controls;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Service.Smile;
using Microsoft.Win32;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Setting
{
    public class SmileSettingManagerViewModel: ManagerViewModelBase
    {
        #region variable

        string _editingAccountName;
        string _editingAccountPassword;

        #endregion

        public SmileSettingManagerViewModel(Mediation mediation)
            : base(mediation)
        {
            Setting = Mediation.GetResultFromRequest<SmileSettingModel>(new RequestModel(RequestKind.Setting, ServiceType.Smile));
            Session = Mediation.GetResultFromRequest<SmileSessionViewModel>(new RequestModel(RequestKind.Session, ServiceType.Smile));
        }

        #region property

        SmileSettingModel Setting { get; }
        public SmileSessionViewModel Session { get; }

        /// <summary>
        /// 編集用アカウント名。
        /// </summary>
        public string EditingAccountName
        {
            get { return this._editingAccountName; }
            set { SetVariableValue(ref this._editingAccountName, value); }
        }
        /// <summary>
        /// 編集用アカウントパスワード。
        /// </summary>
        public string EditingAccountPassword
        {
            get { return this._editingAccountPassword; }
            set { SetVariableValue(ref this._editingAccountPassword, value); }
        }

        /// <summary>
        /// 動画再生方法。
        /// </summary>
        public ExecuteOrOpenMode OpenMode
        {
            get { return Setting.Video.Execute.OpenMode; }
            set { SetPropertyValue(Setting.Video.Execute, value); }
        }
        /// <summary>
        /// 外部プログラムパス。
        /// </summary>
        public string LauncherPath
        {
            get { return Setting.Video.Execute.LauncherPath; }
            set { SetPropertyValue(Setting.Video.Execute, value); }
        }
        /// <summary>
        /// 外部プログラムパラメータ。
        /// </summary>
        public string LauncherParameter
        {
            get { return Setting.Video.Execute.LauncherParameter; }
            set { SetPropertyValue(Setting.Video.Execute, value); }
        }

        public string LauncherParameterList
        {
            get
            {
                var list = new[] {
                    SmileVideoInformationUtility.launcherParameterVideoId,
                    SmileVideoInformationUtility.launcherParameterVideoTitle,
                    SmileVideoInformationUtility.launcherParameterVideoPage,
                };
                return string.Join(", ", list.Select(s => "${" + s + "}"));
            }
        }

        #endregion

        #region command

        public ICommand LoginCommand
        {
            get
            {
                return CreateCommand(o => LoginAsync().ConfigureAwait(false));
            }
        }

        public ICommand OpenUriCommand
        {
            get
            {
                return CreateCommand(o => {
                    var uri = (string)o;
                    try {
                        Process.Start(uri);
                    } catch(Exception ex) {
                        Mediation.Logger.Error(ex);
                    }
                });
            }
        }

        public ICommand OpenDialogLauncherPathCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        LauncherPath = OpenDialog(LauncherPath);
                    }
                );
            }
        }

        #endregion

        #region function

        async Task LoginAsync()
        {
            Setting.Account.Name = EditingAccountName;
            Setting.Account.Password = EditingAccountPassword;

            await Session.ChangeUserAccountAsync(Setting.Account);

            await Session.LoginAsync();

            var tasks = new[] {
                Mediation.Smile.ManagerPack.UsersManager.InitializeAsync(),
                Mediation.Smile.ManagerPack.VideoManager.InitializeAsync(),
            };
            await Task.WhenAll(tasks);
        }

        string OpenDialog(string path)
        {
            var existFile = File.Exists(path);

            var dialog = new OpenFileDialog() {
                FileName = existFile ? path : string.Empty,
                InitialDirectory = existFile ? Path.GetDirectoryName(path) : Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                CheckFileExists = true,
            };
            if(dialog.ShowDialog().GetValueOrDefault()) {
                return dialog.FileName;
            }

            return path;
        }

        #endregion

        #region SmileVideoCustomManagerViewModelBase

        protected override IEnumerable<ManagerViewModelBase> GetManagerChildren()
        {
            return Enumerable.Empty<ManagerViewModelBase>();
        }

        protected override void ShowViewCore()
        {
            EditingAccountName = Setting.Account.Name;
            EditingAccountPassword = Setting.Account.Password;
        }

        protected override void HideViewCore()
        { }

        public override Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public override void InitializeView(MainWindow view)
        { }

        public override void UninitializeView(MainWindow view)
        { }

        public override Task<long> GarbageCollectionAsync(GarbageCollectionLevel garbageCollectionLevel, CacheSpan cacheSpan)
        {
            return GarbageCollectionDummyResult;
        }

        #endregion
    }
}
