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
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Exceptions.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.MultiCommandParameter.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Order;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Video.Parameter;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.View.Controls.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Bookmark;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Laboratory;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Player;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video
{
    public abstract class SmileVideoFinderViewModelBase : TFinderViewModelBase<SmileVideoInformationViewModel, SmileVideoFinderItemViewModel>
    {
        #region variable

        //bool _nowLoading;

        //SmileVideoFinderItemViewModel _selectedFinderItem;
        //SourceLoadState _finderLoadState;

        SmileVideoSortType _selectedSortType;
        bool _showContinuousPlaybackMenu;

        bool _showDownloadMenu;
        bool _showOpenCacheDirectoryMenu;

        #endregion

        public SmileVideoFinderViewModelBase(Mediator mediator, int baseNumber)
            : base(mediator, baseNumber)
        {
            var settingResult = Mediator.Request(new RequestModel(RequestKind.Setting, ServiceType.SmileVideo));
            Setting = (SmileVideoSettingModel)settingResult.Result;

            SortTypeItems = new CollectionModel<SmileVideoSortType>(EnumUtility.GetMembers<SmileVideoSortType>());

            var filteringResult = Mediator.GetResultFromRequest<SmileVideoFilteringResultModel>(new SmileVideoCustomSettingRequestModel(SmileVideoCustomSettingKind.CommentFiltering));
            FinderFiltering = filteringResult.Filtering;

            FinderItems.Filter = FilterItems;

            DragAndDrop = new DelegateDragAndDrop() {
                CanDragStartFunc = CanDragStartFromFinder,
                GetDragParameterFunc = GetDragParameterFromFinder,
                DragEnterAction = DragEnterFromFinder,
                DragOverAction = DragOverFromFinder,
                DragLeaveAction = DragLeaveFromFinder,
                DropAction = DropFromFinder
            };
        }

        #region property

        protected SmileVideoSettingModel Setting { get; }

        public CollectionModel<SmileVideoSortType> SortTypeItems { get; }

        /// <summary>
        /// フィルタリング。
        /// </summary>
        public SmileVideoFilteringViweModel FinderFiltering { get; }

        /// <summary>
        /// ソート方法。
        /// </summary>
        public virtual SmileVideoSortType SelectedSortType
        {
            get { return this._selectedSortType; }
            set
            {
                if(SetVariableValue(ref this._selectedSortType, value)) {
                    ChangeSortItems();
                }
            }
        }

        [Obsolete]
        protected virtual bool IsLoadVideoInformation { get { return Setting.Search.LoadInformation; } }

        public bool ShowContinuousPlaybackMenu
        {
            get { return this._showContinuousPlaybackMenu; }
            set
            {
                if(SetVariableValue(ref this._showContinuousPlaybackMenu, value)) {
                    if(ShowContinuousPlaybackMenu) {
                        CallOnPropertyChange(nameof(BookmarkItems));
                    }
                }
            }
        }

        /// <summary>
        /// 「あとで見る」メニューを有効にするか。
        /// </summary>
        public virtual bool IsEnabledCheckItLaterMenu { get; } = true;
        /// <summary>
        /// ダウンローダーへ設定可能か
        /// </summary>
        public virtual bool IsEnabledDownloadMenu => IsEnabledCheckItLaterMenu;
        /// <summary>
        /// 「未整理のブックマーク」メニューを有効にするか。
        /// </summary>
        public virtual bool IsEnabledUnorganizedBookmarkMenu { get; } = true;

        /// <summary>
        /// チェックメニューからブックマークメニューを使用可能にするか。
        /// </summary>
        public virtual bool IsEnabledBookmarkMenu => IsEnabledUnorganizedBookmarkMenu;

        /// <summary>
        /// 動画再生方法。
        /// </summary>
        public virtual ExecuteOrOpenMode OpenMode => Setting.Execute.OpenMode;
        /// <summary>
        /// プレイヤー表示方法。
        /// </summary>
        public virtual bool OpenPlayerInNewWindow => Setting.Execute.OpenPlayerInNewWindow;

        public SearchType FinderSearchType
        {
            get { return Setting.Search.FinderSearchType; }
            set { SetPropertyValue(Setting.Search, value, nameof(Setting.Search.FinderSearchType)); }
        }

        public IEnumerable<SmileVideoBookmarkNodeViewModel> BookmarkItems
        {
            get
            {
                if(!IsEnabledBookmarkMenu) {
                    return Enumerable.Empty<SmileVideoBookmarkNodeViewModel>();
                }
                var result = Mediator.GetResultFromRequest<SmileVideoBookmarkResultModel>(new SmileVideoCustomSettingRequestModel(SmileVideoCustomSettingKind.Bookmark));
                var bookmarkItems = SmileVideoBookmarkUtility.ConvertFlatBookmarkItems(result.UserNodes);
                return bookmarkItems;
            }
        }

        public bool ShowDownloadMenu
        {
            get { return this._showDownloadMenu; }
            set { SetVariableValue(ref this._showDownloadMenu, value); }
        }
        public bool ShowOpenCacheDirectoryMenu
        {
            get { return this._showOpenCacheDirectoryMenu; }
            set { SetVariableValue(ref this._showOpenCacheDirectoryMenu, value); }
        }

        #endregion

        #region command

        public ICommand ContinuousPlaybackCommand
        {
            get { return CreateCommand(o => ContinuousPlaybackAsync(false, false)); }
        }

        public ICommand ContinuousPlaybackLastItemIsStopCommand
        {
            get { return CreateCommand(o => ContinuousPlaybackAsync(false, true)); }
        }

        public ICommand RandomContinuousPlaybackCommand
        {
            get { return CreateCommand(o => ContinuousPlaybackAsync(true, false)); }
        }
        public ICommand RandomContinuousPlaybackLastItemIsStopCommand
        {
            get { return CreateCommand(o => ContinuousPlaybackAsync(true, true)); }
        }

        public ICommand ClearCheckedCacheCommand
        {
            get
            {
                return CreateCommand(
                    o => ClearCheckedCache(),
                    o => GetCheckedItems().Any()
                );
            }
        }

        public ICommand ChangedFilteringCommand
        {
            get { return CreateCommand(o => ChangedFiltering()); }
        }

        public ICommand AddFinderFilteringCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        var commandParameter = (SmileVideoFinderFilterCommandParameterModel)o;
                        AddFinderFiltering(commandParameter.FilteringTarget, commandParameter.Source);
                    },
                    o => SelectedFinderItem != null && !string.IsNullOrEmpty((o as SmileVideoFinderFilterCommandParameterModel)?.Source)
                );
            }
        }

        public ICommand AddCheckItLaterCommand
        {
            get
            {
                return CreateCommand(
                    o => AddCheckItLater(SelectedFinderItem),
                    o => IsEnabledCheckItLaterMenu && SelectedFinderItem != null
                );
            }
        }

        public ICommand AddUnorganizedBookmarkCommand
        {
            get
            {
                return CreateCommand(
                    o => AddUnorganizedBookmark(SelectedFinderItem),
                    o => IsEnabledUnorganizedBookmarkMenu && SelectedFinderItem != null
                );
            }
        }

        public ICommand AddPlayListCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        var player = Mediator.GetResultFromRequest<IEnumerable<SmileVideoPlayerViewModel>>(new RequestModel(RequestKind.WindowViewModels, ServiceType.SmileVideo))
                            .Where(p => !(p is SmileVideoLaboratoryPlayerViewModel))
                            .FirstOrDefault(p => p.IsWorkingPlayer.Value)
                        ;
                        if(player != null) {
                            player.AddPlayList(SelectedFinderItem.Information);
                        }
                    },
                    o => {
                        if(SelectedFinderItem != null) {
                            var players = Mediator.GetResultFromRequest<IEnumerable<SmileVideoPlayerViewModel>>(new RequestModel(RequestKind.WindowViewModels, ServiceType.SmileVideo))
                                .Where(p => !(p is SmileVideoLaboratoryPlayerViewModel))
                            ;
                            return players.Any();
                        }
                        return false;
                    }
                );
            }
        }

        public ICommand AddDownloadManagerCommand
        {
            get
            {
                return CreateCommand(
                    o => AddDownloadManager(SelectedFinderItem),
                    o => IsEnabledDownloadMenu && SelectedFinderItem != null && SmileVideoInformationUtility.CheckCanPlay(SelectedFinderItem.Information, Mediator.Logger)
                );
            }
        }

        public ICommand CopyCustomInformationCommand
        {
            get
            {
                return CreateCommand(
                    o => CopyCustomInformation(SelectedFinderItem.Information),
                    o => SelectedFinderItem != null && !string.IsNullOrWhiteSpace(Setting.Common.CustomCopyFormat)
                );
            }
        }

        public ICommand CopyInformationTextCommand
        {
            get
            {
                return CreateCommand(
                    o => CopyInformationText((string)o),
                    o => SelectedFinderItem != null && !string.IsNullOrEmpty((string)o)
                );
            }
        }

        public ICommand CopyInformationPosterTextCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        var isId = (bool)o;

                        if(SelectedFinderItem.Information.IsChannelVideo) {
                            var text = isId
                                ? SelectedFinderItem.Information.ChannelId
                                : SelectedFinderItem.Information.ChannelName
                            ;
                            CopyInformationText(text);
                        } else {
                            var text = isId
                                ? SelectedFinderItem.Information.UserId
                                : SelectedFinderItem.Information.UserName
                            ;
                            CopyInformationText(text);
                        }
                    },
                    o => SelectedFinderItem != null
                );
            }
        }

        public ICommand SearchInformationTextCommand
        {
            get
            {
                return CreateCommand(
                    o => SearchInformationText((string)o),
                    o => SelectedFinderItem != null && !string.IsNullOrEmpty((string)o)
                );
            }
        }

        public ICommand AddBookmarkCheckedItemCommand
        {
            get
            {
                return CreateCommand(
                    o => AddBookmarkCheckedItem((SmileVideoBookmarkNodeViewModel)o)
                );
            }
        }

        public ICommand ShowPosterCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        if(SelectedFinderItem.Information.IsChannelVideo) {
                            var channelId = SelectedFinderItem.Information.ChannelId;
                            SmileDescriptionUtility.OpenChannelId(channelId, Mediator, Mediator);
                        } else {
                            var userId = SelectedFinderItem.Information.UserId;
                            SmileDescriptionUtility.OpenUserId(userId, Mediator);
                        }
                    },
                    o => SelectedFinderItem != null //&& !SelectedFinderItem.Information.IsChannelVideo
                );
            }
        }

        #endregion

        #region function

        protected bool IsShowFilteringItem(object obj)
        {
            var finderItem = (SmileVideoFinderItemViewModel)obj;
            finderItem.Approval = true;

            if(!IsUsingFinderFilter) {
                return true;
            }

            if(!FinderFiltering.FinderFilterList.Any()) {
                return true;
            }

            var information = finderItem.Information;
            var filters = FinderFiltering.FinderFilterList.Select(i => new SmileVideoFinderFiltering(i.Model, Mediator));
            foreach(var filter in filters.AsParallel()) {
                var param = new SmileVideoFinderFilteringParameterModel() {
                    VideoId = information.VideoId,
                    Title = information.Title,
                    Tags = information.TagList,
                };
                if(information.InformationSource == SmileVideoInformationSource.Getthumbinfo) {
                    param.UserId = information.UserId;
                    param.UserName = information.UserName;
                    param.ChannelId = information.ChannelId;
                    param.ChannelName = information.ChannelName;
                    param.Description = information.Description;
                }

                var isHit = filter.Check(param);
                if(isHit) {
                    finderItem.Approval = false;
                    return false;
                }
            }

            return true;
        }

        protected virtual bool FilterItems(object obj)
        {
            var isHitFinder = IsShowFilteringItem(obj);

            if(IsEnabledFinderFiltering && !isHitFinder) {
                return false;
            }

            var filter = InputTitleFilter;
            if(string.IsNullOrEmpty(filter)) {
                return true;
            }

            var finderItem = (SmileVideoFinderItemViewModel)obj;
            var viewModel = finderItem.Information;
            var isHit = viewModel.Title.IndexOf(filter, StringComparison.OrdinalIgnoreCase) != -1;
            if(IsBlacklist) {
                return !isHit;
            } else {
                return isHit;
            }
        }

        internal virtual Task ContinuousPlaybackAsync(bool isRandom, bool lastItemIsStop, Action<SmileVideoPlayerViewModel> playerPreparationAction = null)
        {
            ShowContinuousPlaybackMenu = false;

            var items = GetCheckedItems().ToEvaluatedSequence();

            if(!items.Any()) {
                return Task.CompletedTask;
            }

            var playList = items
                .Select(i => i.Information)
                .ToEvaluatedSequence()
            ;

            foreach(var item in items) {
                item.IsChecked = false;
            }

            var vm = new SmileVideoPlayerViewModel(Mediator);
            vm.IsRandomPlay = isRandom;
            vm.LastPlayListItemIsStop = lastItemIsStop;

            try {
                var task = vm.LoadAsync(playList, Constants.ServiceSmileVideoThumbCacheSpan, Constants.ServiceSmileVideoImageCacheSpan);
                if(playerPreparationAction != null) {
                    playerPreparationAction(vm);
                }
                Mediator.Request(new ShowViewRequestModel(RequestKind.ShowView, ServiceType.SmileVideo, vm, ShowViewState.Foreground));
                return task;
            } catch(SmileVideoCanNotPlayItemInPlayListException ex) {
                Mediator.Logger.Warning(ex);
                vm.Dispose();
            }

            return Task.CompletedTask;
        }

        void ChangedFiltering()
        {
            FinderItems.Refresh();
        }

        void AddFinderFiltering(SmileVideoFinderFilteringTarget target, string source)
        {
            Mediator.Logger.Debug($"{target}: {source}");

            // 同一っぽいデータがある場合は無視する
            if(FinderFiltering.FinderFilterList.Any(i => i.Model.Target == target && i.Model.Source == source)) {
                return;
            }

            var model = new SmileVideoFinderFilteringItemSettingModel() {
                Source = source,
                Target = target,
                Type = FilteringType.PerfectMatch,
                IgnoreCase = true,
                IsEnabled = true,
            };
            FinderFiltering.AddFinderFilter(model);
        }

        void AddCheckItLater(SmileVideoFinderItemViewModel finderItem)
        {
            var information = finderItem.Information;
            var item = information.ToVideoItemModel();
            Mediator.Request(new SmileVideoProcessRequestModel(new SmileVideoProcessCheckItLaterParameterModel(item, SmileVideoCheckItLaterFrom.ManualOperation, true)));
        }

        void AddBookmarkCheckedItem(SmileVideoBookmarkNodeViewModel bookmark)
        {
            ShowContinuousPlaybackMenu = false;

            var checkedItems = GetCheckedItems();
            if(!checkedItems.Any()) {
                return;
            }

            var items = checkedItems
                .Select(i => i.Information.ToVideoItemModel())
                .ToEvaluatedSequence()
            ;
            Mediator.Request(new SmileVideoProcessRequestModel(new SmileVideoProcessBookmarkParameterModel(bookmark, items, true)));
        }

        void AddUnorganizedBookmark(SmileVideoFinderItemViewModel finderItem)
        {
            var information = finderItem.Information;
            var item = information.ToVideoItemModel();
            Mediator.Request(new SmileVideoProcessRequestModel(new SmileVideoProcessUnorganizedBookmarkParameterModel(item)));
        }

        void AddDownloadManager(SmileVideoFinderItemViewModel finderItem)
        {
            var information = finderItem.Information;
            if(!SmileVideoInformationUtility.CheckCanPlay(information, Mediator.Logger)) {
                return;
            }
            // あとで見る用キャッシュなのであとで見るに追加するが、「追加可能であれば」という条件付き
            if(IsEnabledCheckItLaterMenu) {
                AddCheckItLater(finderItem);
            }
            var download = new SmileVideoDownloadViewModel(Mediator) {
                Information = information,
                DownloadState = DownloadState.Waiting,
            };
            Mediator.Order(new DownloadOrderModel(download, false, ServiceType.SmileVideo));
        }


        protected virtual bool CanDragStartFromFinder(UIElement sender, MouseEventArgs e)
        {
            return false;
        }

        protected virtual IReadOnlyCheckResult<DragParameterModel> GetDragParameterFromFinder(UIElement sender, MouseEventArgs e)
        {
            return CheckResultModel.Failure<DragParameterModel>();
        }
        protected virtual void DragEnterFromFinder(UIElement sender, DragEventArgs e)
        { }
        protected virtual void DragOverFromFinder(UIElement sender, DragEventArgs e)
        { }
        protected virtual void DragLeaveFromFinder(UIElement sender, DragEventArgs e)
        { }
        protected virtual void DropFromFinder(UIElement sender, DragEventArgs e)
        { }

        void CopyCustomInformation(SmileVideoInformationViewModel information)
        {
            if(string.IsNullOrEmpty(Setting.Common.CustomCopyFormat)) {
                Mediator.Logger.Information($"{nameof(Setting.Common.CustomCopyFormat)} is empty");
                return;
            }

            var keyword = Mediator.GetResultFromRequest<IReadOnlySmileVideoKeyword>(new SmileVideoOtherDefineRequestModel(SmileVideoOtherDefineKind.Keyword));

            var text = SmileVideoInformationUtility.GetCustomFormatedText(information, keyword, Setting.Common.CustomCopyFormat);
            CopyInformationText(text);
        }

        void CopyInformationText(string text)
        {
            ShellUtility.SetClipboard(text, Mediator.Logger);
        }

        void SearchInformationText(string text)
        {
            var parameter = new SmileVideoSearchParameterModel() {
                SearchType = FinderSearchType,
                Query = text,
            };

            Mediator.Request(new ShowViewRequestModel(RequestKind.ShowView, ServiceType.SmileVideo, parameter, ShowViewState.Foreground));
        }

        void ClearCheckedCache()
        {
            ShowContinuousPlaybackMenu = false;

            var items = GetCheckedItems().ToEvaluatedSequence();
            long gcSize = 0;
            foreach(var item in items) {
                try {
                    var checkResult = item.Information.GarbageCollection(GarbageCollectionLevel.All, CacheSpan.NoCache, true);
                    if(checkResult.IsSuccess) {
                        item.IsChecked = false;
                        gcSize += checkResult.Result;
                    } else {
                        Mediator.Logger.Warning($"{item.Information.VideoId}: GC fail", checkResult.Detail);
                    }
                } catch(Exception ex) {
                    Mediator.Logger.Error(ex);
                }
            }

            if(0 < gcSize) {
                Mediator.Logger.Information($"GC: {RawValueUtility.ConvertHumanLikeByte(gcSize)}", gcSize);
            }
        }

        #endregion

        #region TFinderViewModelBase

        public override bool IsOpenContextMenu
        {
            get { return base.IsOpenContextMenu; }
            set
            {
                if(value) {
                    var moreOptionsShowable = AppUtility.MoreOptionsShowable;
                    ShowDownloadMenu = moreOptionsShowable;
                    ShowOpenCacheDirectoryMenu = moreOptionsShowable;
                }

                base.IsOpenContextMenu = value;
            }
        }

        public override CacheSpan DefaultInformationCacheSpan => Constants.ServiceSmileVideoThumbCacheSpan;
        public override CacheSpan DefaultImageCacheSpan => Constants.ServiceSmileVideoImageCacheSpan;
        public override object DefaultExtends { get; } = null;

        public override CheckedProcessType SelectedCheckedProcess
        {
            get { return base.SelectedCheckedProcess; }
            set
            {
                if(base.SelectedCheckedProcess != value) {
                    base.SelectedCheckedProcess = value;
                    CheckedProcessCommand.TryExecute(SelectedCheckedProcess);
                }
            }
        }

        internal override void ChangeSortItems()
        {
            var map = new[] {
                new { Type = SmileVideoSortType.Number, Parent = string.Empty, Property = nameof(SmileVideoFinderItemViewModel.Number) },
                new { Type = SmileVideoSortType.Title, Parent = nameof(SmileVideoFinderItemViewModel.Information), Property = nameof(SmileVideoInformationViewModel.Title) },
                new { Type = SmileVideoSortType.Length, Parent = nameof(SmileVideoFinderItemViewModel.Information), Property = nameof(SmileVideoInformationViewModel.Length) },
                new { Type = SmileVideoSortType.FirstRetrieve, Parent = nameof(SmileVideoFinderItemViewModel.Information), Property = nameof(SmileVideoInformationViewModel.FirstRetrieve) },
                new { Type = SmileVideoSortType.ViewCount, Parent = nameof(SmileVideoFinderItemViewModel.Information),Property = nameof(SmileVideoInformationViewModel.ViewCounter) },
                new { Type = SmileVideoSortType.CommentCount, Parent = nameof(SmileVideoFinderItemViewModel.Information),Property = nameof(SmileVideoInformationViewModel.CommentCounter) },
                new { Type = SmileVideoSortType.MyListCount, Parent = nameof(SmileVideoFinderItemViewModel.Information),Property = nameof(SmileVideoInformationViewModel.MylistCounter) },
            }.ToDictionary(
                ak => ak.Type,
                av => $"{av.Parent}.{av.Property}"
            );
            var propertyName = map[SelectedSortType];
            var direction = IsAscending ? ListSortDirection.Ascending : ListSortDirection.Descending;
            var sort = new SortDescription(propertyName, direction);

            FinderItems.SortDescriptions.Clear();
            FinderItems.SortDescriptions.Add(sort);
            FinderItems.Refresh();
        }

        protected override SmileVideoFinderItemViewModel CreateFinderItem(SmileVideoInformationViewModel information, int number)
        {
            return new SmileVideoFinderItemViewModel(information, number);
        }

        protected override Task LoadInformationAsync(IEnumerable<SmileVideoInformationViewModel> informationItems, CacheSpan informationCacheSpan)
        {
            var loader = new SmileVideoInformationLoader(informationItems, NetworkSetting, Mediator.Logger);
            return loader.LoadInformationAsync(informationCacheSpan);
        }

        protected override Task LoadImageAsync(IEnumerable<SmileVideoInformationViewModel> informationItems, CacheSpan imageCacheSpan)
        {
            var loader = new SmileVideoInformationLoader(informationItems, NetworkSetting, Mediator.Logger);
            return loader.LoadThumbnaiImageAsync(imageCacheSpan);
        }

        protected override Task CheckedProcessAsync(CheckedProcessType checkedProcessType)
        {
            switch(checkedProcessType) {
                case CheckedProcessType.SequencePlay:
                    return ContinuousPlaybackAsync(false, false);

                case CheckedProcessType.RandomPlay:
                    return ContinuousPlaybackAsync(true, false);

                default:
                    throw new NotImplementedException();
            }
        }

        #endregion
    }
}
