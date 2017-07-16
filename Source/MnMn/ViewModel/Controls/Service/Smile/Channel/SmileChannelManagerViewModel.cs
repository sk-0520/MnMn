using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility.UI;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Data;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Parameter;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Channel;
using ContentTypeTextNet.MnMn.MnMn.View.Controls;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Channel
{
    public class SmileChannelManagerViewModel : ManagerViewModelBase
    {
        #region variable

        SmileChannelInformationViewModel _selectedChannel;
        SmileChannelHistoryItemViewModel _selectedChannelHistory;
        SmileChannelBookmarkItemViewModel _selectedChannelBookmark;

        #endregion


        public SmileChannelManagerViewModel(Mediation mediation)
            : base(mediation)
        {
            Setting = Mediation.GetResultFromRequest<SmileSettingModel>(new RequestModel(RequestKind.Setting, ServiceType.Smile));

            ChannelBookmarkCollection = new MVMPairCreateDelegationCollection<SmileChannelBookmarkItemModel, SmileChannelBookmarkItemViewModel>(Setting.Channel.Bookmark, default(object), CreateBookmarkItem);
            ChannelBookmarkItems = CollectionViewSource.GetDefaultView(ChannelBookmarkCollection.ViewModelList);

            ChannelHistoryCollection = new MVMPairCreateDelegationCollection<SmileChannelItemModel, SmileChannelHistoryItemViewModel>(Setting.Channel.History, default(object), CreateHistoryItem);
            ChannelHistoryItems = CollectionViewSource.GetDefaultView(ChannelHistoryCollection.ViewModelList);
        }

        #region property

        SmileSettingModel Setting { get; }
        TabControl ChannelTab { get; set; }

        DispatcherTimer CheckItLaterCheckTimer { get; } = new DispatcherTimer() {
            Interval = Constants.ServiceSmileChannelCheckItLaterCheckTime,
        };

        public SmileChannelInformationViewModel SelectedChannel
        {
            get { return this._selectedChannel; }
            set
            {
                if(SetVariableValue(ref this._selectedChannel, value)) {
                    if(SelectedChannel != null) {
                        RefreshWebPage();
                    }
                }
            }
        }

        public GridLength GroupWidth
        {
            get { return new GridLength(Setting.Channel.GroupWidth, GridUnitType.Star); }
            set { SetPropertyValue(Setting.Channel, value.Value, nameof(Setting.Channel.GroupWidth)); }
        }
        public GridLength ItemsWidth
        {
            get { return new GridLength(Setting.Channel.ItemsWidth, GridUnitType.Star); }
            set { SetPropertyValue(Setting.Channel, value.Value, nameof(Setting.Channel.ItemsWidth)); }
        }

        public CollectionModel<SmileChannelInformationViewModel> ChannelItems { get; } = new CollectionModel<SmileChannelInformationViewModel>();

        MVMPairCreateDelegationCollection<SmileChannelBookmarkItemModel, SmileChannelBookmarkItemViewModel> ChannelBookmarkCollection { get; }
        public ICollectionView ChannelBookmarkItems { get; }

        MVMPairCreateDelegationCollection<SmileChannelItemModel, SmileChannelHistoryItemViewModel> ChannelHistoryCollection { get; }
        public ICollectionView ChannelHistoryItems { get; }

        public SmileChannelHistoryItemViewModel SelectedChannelHistory
        {
            get { return this._selectedChannelHistory; }
            set
            {
                if(SetVariableValue(ref this._selectedChannelHistory, value)) {
                    if(SelectedChannelHistory != null) {
                        LoadAsync(SelectedChannelHistory.ChannelId, false, false).ConfigureAwait(false);
                    }
                }
            }
        }

        public SmileChannelBookmarkItemViewModel SelectedChannelBookmark
        {
            get { return this._selectedChannelBookmark; }
            set
            {
                if(SetVariableValue(ref this._selectedChannelBookmark, value)) {
                    if(SelectedChannelBookmark != null) {
                        LoadAsync(SelectedChannelBookmark.ChannelId, false, true).ConfigureAwait(false);
                    }
                }
            }
        }

        #endregion

        #region command

        public ICommand NewWindowCommand
        {
            get
            {
                return CreateCommand(o => {
                    var data = (WebNavigatorEventDataBase)o;
                    WebNavigatorUtility.OpenNewWindowWrapper(data, Mediation.Logger);
                });
            }
        }

        public ICommand CloseTabCommand
        {
            get
            {
                return CreateCommand(o => CloseTab((SmileChannelInformationViewModel)o));
            }
        }

        public ICommand AddBookmarkCommand
        {
            get { return CreateCommand(o => AddBookmarkAsync((SmileChannelInformationViewModel)o).ConfigureAwait(false)); }
        }

        public ICommand RemoveBookmarkCommand
        {
            get { return CreateCommand(o => RemoveBookmark((SmileChannelInformationViewModel)o)); }
        }

        public ICommand MoveUpBookmarkSelectedItemCommand
        {
            get
            {
                return CreateCommand(
                    o => ItemsControlUtility.MoveItem(ChannelBookmarkCollection, SelectedChannelBookmark, true),
                    o => SelectedChannelBookmark != null && ItemsControlUtility.CanMoveNext(ChannelBookmarkCollection, SelectedChannelBookmark, true)
                );
            }
        }

        public ICommand MoveDownBookmarkSelectedItemCommand
        {
            get
            {
                return CreateCommand(
                    o => ItemsControlUtility.MoveItem(ChannelBookmarkCollection, SelectedChannelBookmark, false),
                    o => SelectedChannelBookmark != null && ItemsControlUtility.CanMoveNext(ChannelBookmarkCollection, SelectedChannelBookmark, false)
                );
            }
        }

        #endregion

        #region function

        public Task LoadFromParameterAsync(SmileOpenChannelIdParameterModel parameter)
        {
            return LoadAsync(parameter.ChannelId, parameter.IsLoginUser, parameter.AddHistory);
        }

        public Task LoadAsync(string channelId, bool isLoginUser, bool addHistory)
        {
            var existChannel = ChannelItems.FirstOrDefault(i => i.ChannelId == channelId);
            if(existChannel != null) {
                SelectedChannel = existChannel;
                return Task.CompletedTask;
            } else {
                var channel = new SmileChannelInformationViewModel(Mediation, channelId);
                ChannelItems.Add(channel);
                SelectedChannel = channel;
                channel.SetView(ChannelTab);

                return channel.LoadDefaultAsync().ContinueWith(t => {
                    if(!isLoginUser && addHistory) {
                        AddHistory(channel);
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        void RefreshWebPage()
        {
            var tabItem = ChannelTab.ItemContainerGenerator.ContainerFromItem(SelectedChannel) as TabItem;
            var web = UIUtility.FindChildren<WebNavigator>(ChannelTab).FirstOrDefault();
            if(web != null) {
                web.HomeSource = SelectedChannel.Uri;
                web.Navigate(web.HomeSource);
                web.CrearHistory();
            }
        }

        void CloseTab(SmileChannelInformationViewModel finder)
        {
            ChannelItems.Remove(finder);
        }

        static SmileChannelBookmarkItemViewModel CreateBookmarkItem(SmileChannelBookmarkItemModel model, object data)
        {
            var result = new SmileChannelBookmarkItemViewModel(model);

            return result;
        }

        static SmileChannelHistoryItemViewModel CreateHistoryItem(SmileChannelItemModel model, object data)
        {
            var result = new SmileChannelHistoryItemViewModel(model);

            return result;
        }

        void AddBookmarkVideos(SmileChannelBookmarkItemModel bookmark, SmileChannelInformationViewModel information)
        {
            var videoIds = information.VideoFinder.FinderItemsViewer.Select(i => i.Information.VideoId);
            bookmark.Videos.InitializeRange(videoIds);
        }

        Task AddBookmarkAsync(SmileChannelInformationViewModel information)
        {
            var item = new SmileChannelBookmarkItemModel() {
                ChannelId = information.ChannelId,
                ChannelName = information.ChannelName,
                UpdateTimestamp = DateTime.Now,
            };
            var existItem = ChannelBookmarkCollection.ModelList.FirstOrDefault(i => i.ChannelId == item.ChannelId);
            if(existItem != null) {
                return Task.CompletedTask;
            }
            //UserBookmarkCollection.Insert(0, item, null);
            AppUtility.PlusItem(ChannelBookmarkCollection, item, null);

            RefreshChannel();

            if(information.HasPostVideo) {
                if(information.VideoFinder.FinderLoadState == SourceLoadState.None) {
                    return information.VideoFinder.LoadDefaultCacheAsync().ContinueWith(t => {
                        AddBookmarkVideos(item, information);
                    });
                } else if(information.VideoFinder.FinderLoadState == SourceLoadState.InformationLoading || information.VideoFinder.FinderLoadState == SourceLoadState.Completed) {
                    AddBookmarkVideos(item, information);
                } else {
                    // 弱参照はむりぽ
                    PropertyChangedEventHandler propertyChanged = null;
                    propertyChanged = (object sender, PropertyChangedEventArgs e) => {
                        if(e.PropertyName == nameof(information.VideoFinder.FinderLoadState)) {
                            if(information.VideoFinder.FinderLoadState == SourceLoadState.InformationLoading || information.VideoFinder.FinderLoadState == SourceLoadState.Completed) {
                                information.VideoFinder.PropertyChanged -= propertyChanged;
                                AddBookmarkVideos(item, information);
                            }
                        }
                    };
                    information.VideoFinder.PropertyChanged += propertyChanged;
                }
            }

            return Task.CompletedTask;
        }

        void RemoveBookmark(SmileChannelInformationViewModel information)
        {
            var viewModel = ChannelBookmarkCollection.ViewModelList.FirstOrDefault(i => i.ChannelId == information.ChannelId);
            if(viewModel != null) {
                ChannelBookmarkCollection.Remove(viewModel);
                RefreshChannel();
            }
        }

        void AddHistory(SmileChannelInformationViewModel information)
        {
            var item = new SmileChannelItemModel() {
                ChannelId = information.ChannelId,
                ChannelName = information.ChannelName,
                UpdateTimestamp = DateTime.Now,
            };
            var existItem = ChannelHistoryCollection.ModelList.FirstOrDefault(i => i.ChannelId == item.ChannelId);
            if(existItem != null) {
                ChannelHistoryCollection.Remove(existItem);
                item = existItem;
            }
            //UserHistoryCollection.Insert(0, item, null);
            AppUtility.AddHistoryItem(ChannelHistoryCollection, item, null);
        }

        void RefreshChannel()
        {
            //var selectedChannel = SelectedChannel;
            //SelectedChannel = null;
            //SelectedChannel = selectedChannel;
        }

        #endregion

        #region SmileChannelManagerViewModel

        public override Task<long> GarbageCollectionAsync(GarbageCollectionLevel garbageCollectionLevel, CacheSpan cacheSpan, bool force)
        {
            return GarbageCollectionDummyResult;
        }

        public override Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public override Task UninitializeAsync()
        {
            return Task.CompletedTask;
        }

        public override void InitializeView(MainWindow view)
        {
            ChannelTab = view.smile.channel.channelTab;
        }

        public override void UninitializeView(MainWindow view)
        {
            ChannelTab = null;
        }

        protected override IEnumerable<ManagerViewModelBase> GetManagerChildren()
        {
            return Enumerable.Empty<ManagerViewModelBase>();
        }

        protected override void HideViewCore()
        { }

        protected override void ShowViewCore()
        { }

        #endregion
    }
}
