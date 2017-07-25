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
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using ContentTypeTextNet.Library.SharedLibrary.Data;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Define.UI.Player;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.View.Controls;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Service.Smile;
using Microsoft.Win32;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Setting
{
    public class SmileSettingManagerViewModel : ManagerViewModelBase
    {
        #region variable

        string _editingAccountName;
        string _editingAccountPassword;

        #endregion

        public SmileSettingManagerViewModel(Mediator mediator)
            : base(mediator)
        {
            Setting = Mediation.GetResultFromRequest<SmileSettingModel>(new RequestModel(RequestKind.Setting, ServiceType.Smile));
            Session = Mediation.GetResultFromRequest<SmileSessionViewModel>(new RequestModel(RequestKind.Session, ServiceType.Smile));

            PropertyChangedListener = new PropertyChangedWeakEventListener(RankingSelectItem_PropertyChanged);

            var rankingDefine = Mediation.GetResultFromRequest<SmileVideoRankingModel>(new RequestModel(RequestKind.RankingDefine, ServiceType.SmileVideo));
            RankingCategoryItems = rankingDefine.Items
                .Select(i => new SmileVideoRankingGroupViewModel(i))
                .ToEvaluatedSequence()
            ;
            foreach(var item in RankingCategoryItems.SelectMany(i => i.Items)) {
                item.IsChecked = !Setting.Video.Ranking.IgnoreCategoryItems.Any(s => s == item.Key);
                PropertyChangedListener.Add(item);
            }
        }

        #region property

        SmileSettingModel Setting { get; }
        public SmileSessionViewModel Session { get; }

        PropertyChangedWeakEventListener PropertyChangedListener { get; }

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
        /// 編集用アカウントパスワード。
        /// </summary>
        public bool EnabledStartupAutoLogin
        {
            get { return Setting.Account.EnabledStartupAutoLogin; }
            set { SetPropertyValue(Setting.Account, value, nameof(Setting.Account.EnabledStartupAutoLogin)); }
        }

        public string CommonCustomCopyFormat
        {
            get { return Setting.Video.Common.CustomCopyFormat; }
            set { SetPropertyValue(Setting.Video.Common, value, nameof(Setting.Video.Common.CustomCopyFormat)); }
        }

        public IEnumerable<IReadOnlyKeywordTextItem> CommonCustomCopyList
        {
            get
            {
                var keyword = Mediation.GetResultFromRequest<IReadOnlySmileVideoKeyword>(new SmileVideoOtherDefineRequestModel(SmileVideoOtherDefineKind.Keyword));
                var result = keyword.Items
                    .Where(i => SmileVideoInformationUtility.IsCustomCopyElement(i))
                    .Select(i => new KeywordTextItemDefinedViewModel(i))
                .ToEvaluatedSequence();

                return result;
            }
        }

        public IReadOnlyList<SmileVideoRankingGroupViewModel> RankingCategoryItems { get; }


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

        public IEnumerable<IReadOnlyKeywordTextItem> LauncherParameterList
        {
            get
            {
                var keyword = Mediation.GetResultFromRequest<IReadOnlySmileVideoKeyword>(new SmileVideoOtherDefineRequestModel(SmileVideoOtherDefineKind.Keyword));
                var result = keyword.Items
                    .Where(i => SmileVideoInformationUtility.IsLauncherParameterElement(i))
                    .Select(i => new KeywordTextItemDefinedViewModel(i))
                .ToEvaluatedSequence();

                return result;
            }
        }

        /// <summary>
        /// 新規ウィンドウでプレイヤーを表示するか。
        /// </summary>
        public bool OpenPlayerInNewWindow
        {
            get { return Setting.Video.Execute.OpenPlayerInNewWindow; }
            set { SetPropertyValue(Setting.Video.Execute, value); }
        }

        /// <summary>
        /// 自動再生を行うか。
        /// </summary>
        public bool IsAutoPlay
        {
            get { return Setting.Video.Player.IsAutoPlay; }
            set { SetPropertyValue(Setting.Video.Player, value, nameof(Setting.Video.Player.IsAutoPlay)); }
        }
        /// <summary>
        /// 自動再生のファイルサイズ閾値。
        /// </summary>
        public long AutoPlayLowestSize
        {
            get { return Setting.Video.Player.AutoPlayLowestSize; }
            set { SetPropertyValue(Setting.Video.Player, value); }
        }

        public bool DisbaledAutoScrollCommentListOverCursor
        {
            get {return Setting.Video.Player.DisbaledAutoScrollCommentListOverCursor;}
            set { SetPropertyValue(Setting.Video.Player, value); }
        }

        public bool CanChangeCommentEnabledArea
        {
            get { return Setting.Video.Player.CanChangeCommentEnabledArea; }
            set { SetPropertyValue(Setting.Video.Player, value); }
        }

        public bool ShowNavigatorFullScreen
        {
            get { return Setting.Video.Player.ShowNavigatorFullScreen; }
            set { SetPropertyValue(Setting.Video.Player, value); }
        }

        public bool KeySpaceToPause
        {
            get { return Setting.Video.Player.KeySpaceToPause; }
            set { SetPropertyValue(Setting.Video.Player, value); }
        }

        public bool MoseClickToPause
        {
            get { return Setting.Video.Player.MoseClickToPause; }
            set { SetPropertyValue(Setting.Video.Player, value); }
        }

        public WheelOperation WheelOperation
        {
            get { return Setting.Video.Player.WheelOperation; }
            set { SetPropertyValue(Setting.Video.Player, value); }
        }

        public int VolumeOperationStep
        {
            get { return Setting.Video.Player.VolumeOperationStep; }
            set { SetPropertyValue(Setting.Video.Player, value); }
        }

        public bool SeekOperationIsPercent
        {
            get { return Setting.Video.Player.SeekOperationIsPercent; }
            set { SetPropertyValue(Setting.Video.Player, value); }
        }

        public int SeekOperationAbsoluteStep
        {
            get { return Setting.Video.Player.SeekOperationAbsoluteStep; }
            set { SetPropertyValue(Setting.Video.Player, value); }
        }
        public int SeekOperationPercentStep
        {
            get { return Setting.Video.Player.SeekOperationPercentStep; }
            set { SetPropertyValue(Setting.Video.Player, value); }
        }

        public bool InactiveIsFullScreenRestore
        {
            get { return Setting.Video.Player.InactiveIsFullScreenRestore; }
            set { SetPropertyValue(Setting.Video.Player, value); }
        }

        public bool InactiveIsFullScreenRestorePrimaryDisplayOnly
        {
            get { return Setting.Video.Player.InactiveIsFullScreenRestorePrimaryDisplayOnly; }
            set { SetPropertyValue(Setting.Video.Player, value); }
        }

        public bool StopFullScreenRestore
        {
            get { return Setting.Video.Player.StopFullScreenRestore; }
            set { SetPropertyValue(Setting.Video.Player, value); }
        }

        public bool StopFullScreenRestorePrimaryDisplayOnly
        {
            get { return Setting.Video.Player.StopFullScreenRestorePrimaryDisplayOnly; }
            set { SetPropertyValue(Setting.Video.Player, value); }
        }

        public bool UsingDmc
        {
            get { return Setting.Video.Download.UsingDmc; }
            set { SetPropertyValue(Setting.Video.Download, value, nameof(Setting.Video.Download.UsingDmc)); }
        }

        public int DmcVideoWeight
        {
            get { return Setting.Video.Download.VideoWeight; }
            set { SetPropertyValue(Setting.Video.Download, value, nameof(Setting.Video.Download.VideoWeight)); }
        }

        public int DmcAudioWeight
        {
            get { return Setting.Video.Download.AudioWeight; }
            set { SetPropertyValue(Setting.Video.Download, value, nameof(Setting.Video.Download.AudioWeight)); }
        }

        public BackgroundKind BackgroundKind
        {
            get { return Setting.Video.Player.BackgroundKind; }
            set { SetPropertyValue(Setting.Video.Player, value); }
        }
        public Color BackgroundColor
        {
            get { return Setting.Video.Player.BackgroundColor; }
            set { SetPropertyValue(Setting.Video.Player, value); }
        }

        public IEnumerable<Color> BackgroundColors
        {
            get
            {
                return new[] {
                    Colors.Transparent,
                    Colors.White,
                    Colors.Gray,
                    Colors.Black,
                    Colors.Red,
                    Colors.Blue,
                    Colors.Lime,
                    Colors.Yellow,
                };
            }
        }

        public bool AutoInputVideoTitle
        {
            get { return Setting.IdleTalkMutter.AutoInputVideoTitle; }
            set { SetPropertyValue(Setting.IdleTalkMutter, value); }
        }
        public bool AutoInputWatchPageUri
        {
            get { return Setting.IdleTalkMutter.AutoInputWatchPageUri; }
            set { SetPropertyValue(Setting.IdleTalkMutter, value); }
        }
        public string AutoInputHashTags
        {
            get { return Setting.IdleTalkMutter.AutoInputHashTags; }
            set { SetPropertyValue(Setting.IdleTalkMutter, value); }
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
                    var rawUri = (string)o;
                    var uri = new Uri(rawUri);
                    ShellUtility.OpenUriInDefaultBrowser(uri, Mediation.Logger);
                });
            }
        }

        public ICommand OpenDialogLauncherPathCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        LauncherPath = OpenDialogLauncherPath(LauncherPath);
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
                Mediation.Smile.ManagerPack.WebSiteManager.InitializeAsync(),
            };
            await Task.WhenAll(tasks);
        }

        string OpenDialogLauncherPath(string path)
        {
            var existFile = File.Exists(path);

            var filters = new DialogFilterList();
            filters.Add(new DialogFilterItem(Properties.Resources.String_Service_Smile_SmileVide_Launcher_Item_Program, "*.exe"));
            filters.Add(new DialogFilterItem(Properties.Resources.String_Service_Smile_SmileVide_Launcher_Item_Any, "*.*"));

            var dialog = new OpenFileDialog() {
                FileName = existFile ? path : string.Empty,
                InitialDirectory = existFile ? Path.GetDirectoryName(path) : Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                Filter = filters.FilterText,
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

        public override Task UninitializeAsync()
        {
            return Task.CompletedTask;
        }

        public override void InitializeView(MainWindow view)
        { }

        public override void UninitializeView(MainWindow view)
        { }

        public override Task<long> GarbageCollectionAsync(GarbageCollectionLevel garbageCollectionLevel, CacheSpan cacheSpan, bool force)
        {
            return GarbageCollectionDummyResult;
        }

        #endregion

        private void RankingSelectItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(SmileVideoRankingSelectItemViewModel.IsChecked)) {
                var item = (SmileVideoRankingSelectItemViewModel)sender;
                Setting.Video.Ranking.IgnoreCategoryItems.Remove(item.Key);
                if(!item.IsChecked) {
                    Setting.Video.Ranking.IgnoreCategoryItems.Add(item.Key);

                    // カテゴリのチェックが外されたなら下位は全部チェック外す(レイアウトの問題)
                    var category = RankingCategoryItems.FirstOrDefault(i => i.RootItem == item);
                    if(category != null) {
                        foreach(var child in category.Children) {
                            child.IsChecked = false;
                        }
                    }
                }
            }
        }

    }
}
