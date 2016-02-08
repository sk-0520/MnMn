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
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Feed.Rss2;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw.Feed.RankingRss2;
using ContentTypeTextNet.MnMn.MnMn.View.Controls.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video
{
    public class SmileVideoRankingCategoryItemViewModel: ViewModelBase
    {
        #region variable

        SmileVideoElementModel _selectedPeriod;
        SmileVideoElementModel _selectedTarget;

        SmileVideoRankingLoad _rankingLoad;

        SmileVideoInformationViewModel _selectedVideoInformation;

        bool _nowLoading;

        #endregion

        public SmileVideoRankingCategoryItemViewModel(Mediation mediation, SmileVideoRankingModel rankingModel, SmileVideoElementModel period, SmileVideoElementModel target, SmileVideoElementModel category)
        {
            Mediation = mediation;

            PeriodItems = new CollectionModel<SmileVideoElementModel>(rankingModel.Periods.Items.Select(i => (SmileVideoElementModel)i.DeepClone()));
            TargetItems = new CollectionModel<SmileVideoElementModel>(rankingModel.Targets.Items.Select(i => (SmileVideoElementModel)i.DeepClone()));

            SetContextElements(period, target);
            Category = category;

            VideoInformationList = new CollectionModel<SmileVideoInformationViewModel>();
            VideoInformationItems = CollectionViewSource.GetDefaultView(VideoInformationList);
        }

        #region property

        Mediation Mediation { get; set; }

        CancellationTokenSource CancelLoading { get; set;  }

        public IEnumerable<SmileVideoElementModel> PeriodItems { get; private set; }
        public IEnumerable<SmileVideoElementModel> TargetItems { get; private set; }

        public SmileVideoElementModel SelectedPeriod
        {
            get { return this._selectedPeriod; }
            set { SetVariableValue(ref this._selectedPeriod, value); }
        }
        public SmileVideoElementModel SelectedTarget
        {
            get { return this._selectedTarget; }
            set { SetVariableValue(ref this._selectedTarget, value); }
        }

        public SmileVideoElementModel Category { get; private set; }

        public SmileVideoRankingLoad RankingLoad
        {
            get { return this._rankingLoad; }
            set
            {
                if(SetVariableValue(ref this._rankingLoad, value)) {
                    var propertyNames = new[] {
                        nameof(CanLoad),
                        nameof(NowLoading),
                    };
                    CallOnPropertyChange(propertyNames);
                }
            }
        }

        CollectionModel<SmileVideoInformationViewModel> VideoInformationList { get; set; }

        ICollectionView _VideoInformationItems;
        public ICollectionView VideoInformationItems
        {
            get { return this._VideoInformationItems; }
            private set { SetVariableValue(ref this._VideoInformationItems, value); }
        }

        public string CategoryName => Category.CurrentWord;

        public bool CanLoad
        {
            get
            {
                var loadSkips = new[] { SmileVideoRankingLoad.RankingListLoading, SmileVideoRankingLoad.RankingListChecking };
                return !loadSkips.Any(l => l == RankingLoad);
            }
        }

        public bool NowLoading
        {
            get { return this._nowLoading; }
            set { SetVariableValue(ref this._nowLoading, value); }
        }

        public SmileVideoInformationViewModel SelectedVideoInformation
        {
            get { return this._selectedVideoInformation; }
            set { SetVariableValue(ref this._selectedVideoInformation, value); }
        }

        public async void OpenPlayer(SmileVideoInformationViewModel videoInformation)
        {
            var vm = new SmileVideoPlayerViewModel(Mediation);
            var window = new SmileVideoPlayerWindow() {
                DataContext = vm,
            };
            vm.SetView(window);
            window.Show();

            await vm.LoadAsync(videoInformation, Constants.ServiceSmileVideoThumbCacheSpan, Constants.ServiceSmileVideoImageCacheSpan);
        }

        #endregion

        #region command

        public ICommand ReloadCategoryCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        LoadRankingAsync(Constants.ServiceSmileVideoThumbCacheSpan, Constants.ServiceSmileVideoImageCacheSpan).ConfigureAwait(true);
                    }
                );
            }
        }

        public ICommand OpenVideoCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        var item = SelectedVideoInformation;
                        if(item != null) {
                            OpenPlayer(item);
                        }
                    }
                );
            }
        }

        #endregion

        #region function

        async Task LoadRankingAsync_Impl(CacheSpan thumbCacheSpan, CacheSpan imageCacheSpan)
        {
            RankingLoad = SmileVideoRankingLoad.RankingListLoading;
            NowLoading = true;
            var rankingFeedModel = await RestrictUtility.Block(async () => {
                using(var host = new HttpUserAgentHost())
                using(var page = new PageLoader(Mediation, host, SmileVideoMediationKey.ranking, ServiceType.SmileVideo)) {
                    page.ReplaceUriParameters["target"] = SelectedTarget.Key;
                    page.ReplaceUriParameters["period"] = SelectedPeriod.Key;
                    page.ReplaceUriParameters["category"] = Category.Key;
                    page.ReplaceUriParameters["lang"] = Constants.CurrentLanguageCode;

                    var rankingXmlResult = await page.GetResponseTextAsync(PageLoaderMethod.Get);
                    if(!rankingXmlResult.IsSuccess) {
                        RankingLoad = SmileVideoRankingLoad.Failure;
                        return null;
                    }

                    RankingLoad = SmileVideoRankingLoad.RankingListChecking;

                    return RestrictUtility.Block(() => {
                        using(var stream = new MemoryStream(Encoding.UTF8.GetBytes(rankingXmlResult.Result))) {
                            return SerializeUtility.LoadXmlSerializeFromStream<FeedSmileVideoRankingModel>(stream);
                        }
                    });
                }
            });
            if(rankingFeedModel == null) {
                NowLoading = false;
                RankingLoad = SmileVideoRankingLoad.Failure;
                return;
            }


            await Task.Run(() => {
                return rankingFeedModel.Channel.Items
                    .AsParallel()
                    .Select((item, index) => new SmileVideoInformationViewModel(Mediation, item, index + 1))
                ;
            }).ContinueWith(task => {
                var cancel = CancelLoading = new CancellationTokenSource();


                VideoInformationList.InitializeRange(task.Result);
                VideoInformationItems.Refresh();

                Task.Run(() => {
                    RankingLoad = SmileVideoRankingLoad.ImageLoading;

                    Parallel.ForEach(VideoInformationList.ToArray(), item => {
                        var count = 0;
                        var max = 3;
                        var wait = TimeSpan.FromSeconds(1);
                        while(count++ <= max) {
                            cancel.Token.ThrowIfCancellationRequested();
                            try {
                                var t = item.LoadImageAsync(imageCacheSpan);
                                t.Wait();
                                break;
                            } catch(Exception ex) {
                                Debug.WriteLine($"{item}: {ex}");
                                if(cancel.IsCancellationRequested) {
                                    break;
                                } else {
                                    Thread.Sleep(wait);
                                    continue;
                                }
                            }
                        }
                    });
                }).ContinueWith(t => {
                    //VideoInformationItems.Refresh();
                    RankingLoad = SmileVideoRankingLoad.Completed;
                    NowLoading = true;
                   // return Task.CompletedTask;
                }, cancel.Token, TaskContinuationOptions.LongRunning, TaskScheduler.FromCurrentSynchronizationContext());
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public Task LoadRankingAsync(CacheSpan thumbCacheSpan, CacheSpan imageCacheSpan)
        {
            if(CanLoad) {
                if(NowLoading) {
                    CancelLoading.Cancel(true);
                }

                return LoadRankingAsync_Impl(thumbCacheSpan, imageCacheSpan);
            } else {
                return Task.CompletedTask;
            }
        }

        SmileVideoElementModel GetContextElemetFromChangeElement(IEnumerable<SmileVideoElementModel> items, SmileVideoElementModel element)
        {
            if(items.Any(i => i == element)) {
                return element;
            } else {
                return items.FirstOrDefault(i => i.Key == element.Key);
            }
        }

        public void SetContextElements(SmileVideoElementModel period, SmileVideoElementModel target)
        {
            SelectedPeriod = GetContextElemetFromChangeElement(PeriodItems, period);
            SelectedTarget = GetContextElemetFromChangeElement(TargetItems, target);
        }

        void DisposeCancelLoading()
        {
            if(CancelLoading != null) {
                CancelLoading.Dispose();
                CancelLoading = null;
            }
        }

        #endregion

        #region ViewModelBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                DisposeCancelLoading();
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}
