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
using ContentTypeTextNet.MnMn.MnMn.Define.NicoNico.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Feed.Rss2;
using ContentTypeTextNet.MnMn.MnMn.Model.NicoNico.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.NicoNico.Video.Raw;
using ContentTypeTextNet.MnMn.MnMn.Model.NicoNico.Video.Raw.Feed.RankingRss2;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Control.NicoNico.Video
{
    public class RankingCategoryItemViewModel: ViewModelBase
    {
        #region variable

        ElementModel _selectedPeriod;
        ElementModel _selectedTarget;

        RankingLoad _rankingLoad;

        bool _nowLoading;

        #endregion

        public RankingCategoryItemViewModel(Mediation mediation, RankingModel rankingModel, ElementModel period, ElementModel target, ElementModel category)
        {
            Mediation = mediation;

            PeriodItems = new CollectionModel<ElementModel>(rankingModel.Periods.Items.Select(i => (ElementModel)i.DeepClone()));
            TargetItems = new CollectionModel<ElementModel>(rankingModel.Targets.Items.Select(i => (ElementModel)i.DeepClone()));

            SetContextElements(period, target);
            Category = category;

            VideoInformationList = new CollectionModel<VideoInformationViewModel>();
            VideoInformationItems = CollectionViewSource.GetDefaultView(VideoInformationList);
        }

        #region property

        Mediation Mediation { get; set; }

        CancellationTokenSource CancelLoading { get; set;  }

        public IEnumerable<ElementModel> PeriodItems { get; private set; }
        public IEnumerable<ElementModel> TargetItems { get; private set; }

        public ElementModel SelectedPeriod
        {
            get { return this._selectedPeriod; }
            set { SetVariableValue(ref this._selectedPeriod, value); }
        }
        public ElementModel SelectedTarget
        {
            get { return this._selectedTarget; }
            set { SetVariableValue(ref this._selectedTarget, value); }
        }

        public ElementModel Category { get; private set; }

        public RankingLoad RankingLoad
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

        CollectionModel<VideoInformationViewModel> VideoInformationList { get; set; }

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
                var loadSkips = new[] { RankingLoad.RankingListLoading, RankingLoad.RankingListChecking };
                return !loadSkips.Any(l => l == RankingLoad);
            }
        }

        public bool NowLoading
        {
            get { return this._nowLoading; }
            set { SetVariableValue(ref this._nowLoading, value); }
        }

        #endregion

        #region command

        public ICommand ReloadCategoryCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        LoadRankingAsync().ConfigureAwait(true);
                    }
                );
            }
        }

        #endregion

        #region function

        async Task LoadRankingAsync_Impl()
        {
            RankingLoad = RankingLoad.RankingListLoading;
            NowLoading = true;
            var rankingFeedModel = await RestrictUtility.Block(async () => {
                using(var host = new HttpUserAgentHost())
                using(var page = new PageScraping(Mediation, host, MediationNicoNicoVideoKey.ranking, ServiceType.NicoNicoVideo)) {
                    page.ReplaceUriParameters["target"] = SelectedTarget.Key;
                    page.ReplaceUriParameters["period"] = SelectedPeriod.Key;
                    page.ReplaceUriParameters["category"] = Category.Key;
                    page.ReplaceUriParameters["lang"] = Constants.CurrentLanguageCode;

                    var rankingXmlResult = await page.GetResponseTextAsync(HttpMethod.Get);
                    if(!rankingXmlResult.IsSuccess) {
                        RankingLoad = RankingLoad.Failure;
                        return null;
                    }

                    RankingLoad = RankingLoad.RankingListChecking;

                    return RestrictUtility.Block(() => {
                        using(var stream = new MemoryStream(Encoding.UTF8.GetBytes(rankingXmlResult.Result))) {
                            return SerializeUtility.LoadXmlSerializeFromStream<RankingFeedModel>(stream);
                        }
                    });
                }
            });
            if(rankingFeedModel == null) {
                NowLoading = false;
                RankingLoad = RankingLoad.Failure;
                return;
            }


            await Task.Run(() => {
                return rankingFeedModel.Channel.Items
                    .AsParallel()
                    .Select((item, index) => new VideoInformationViewModel(Mediation, item, index + 1))
                ;
            }).ContinueWith(task => {
                CancelLoading = new CancellationTokenSource();

                VideoInformationList.InitializeRange(task.Result);
                VideoInformationItems.Refresh();

                Task.Run(() => {
                    RankingLoad = RankingLoad.ImageLoading;

                    Parallel.ForEach(VideoInformationList.ToArray(), item => {
                        var count = 0;
                        var max = 3;
                        var wait = TimeSpan.FromSeconds(1);
                        while(count++ <= max) {
                            CancelLoading.Token.ThrowIfCancellationRequested();
                            try {
                                var t = item.LoadImageAsync();
                                t.Wait(CancelLoading.Token);
                                break;
                            } catch(WebSocketException ex) {
                                Debug.WriteLine($"{item}: {ex}");
                                Thread.Sleep(wait);
                            } catch(IOException ex) {
                                Debug.WriteLine($"{item}: {ex}");
                            }
                        }
                    });
                }).ContinueWith(t => {
                    //VideoInformationItems.Refresh();
                    RankingLoad = RankingLoad.Completed;
                    NowLoading = true;
                   // return Task.CompletedTask;
                }, CancelLoading.Token, TaskContinuationOptions.LongRunning, TaskScheduler.FromCurrentSynchronizationContext());
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public Task LoadRankingAsync()
        {
            if(CanLoad) {
                if(NowLoading) {
                    DisposeCancelLoading();
                }

                return LoadRankingAsync_Impl();
            } else {
                return Task.CompletedTask;
            }
        }

        ElementModel GetContextElemetFromChangeElement(IEnumerable<ElementModel> items, ElementModel element)
        {
            if(items.Any(i => i == element)) {
                return element;
            } else {
                return items.FirstOrDefault(i => i.Key == element.Key);
            }
        }

        public void SetContextElements(ElementModel period, ElementModel target)
        {
            SelectedPeriod = GetContextElemetFromChangeElement(PeriodItems, period);
            SelectedTarget = GetContextElemetFromChangeElement(TargetItems, target);
        }

        void DisposeCancelLoading()
        {
            if(CancelLoading != null) {
                CancelLoading.Cancel();
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
