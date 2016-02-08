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
    public class SmileVideoRankingCategoryGroupItemViewModel: SmileVideoFinderViewModelBase
    {
        #region variable

        SmileVideoElementModel _selectedPeriod;
        SmileVideoElementModel _selectedTarget;


        #endregion

        public SmileVideoRankingCategoryGroupItemViewModel(Mediation mediation, SmileVideoRankingModel rankingModel, SmileVideoElementModel period, SmileVideoElementModel target, SmileVideoElementModel category)
            :base(mediation)
        {
            PeriodItems = new CollectionModel<SmileVideoElementModel>(rankingModel.Periods.Items.Select(i => (SmileVideoElementModel)i.DeepClone()));
            TargetItems = new CollectionModel<SmileVideoElementModel>(rankingModel.Targets.Items.Select(i => (SmileVideoElementModel)i.DeepClone()));

            SetContextElements(period, target);
            Category = category;
        }

        #region property

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

        public string CategoryName => Category.CurrentWord;

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

        #endregion

        #region function

        async Task LoadRankingAsync_Impl(CacheSpan thumbCacheSpan, CacheSpan imageCacheSpan)
        {
            FinderLoadState = SmileVideoFinderLoadState.VideoSourceLoading;
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
                        FinderLoadState = SmileVideoFinderLoadState.Failure;
                        return null;
                    }

                    FinderLoadState = SmileVideoFinderLoadState.VideoSourceChecking;

                    return RestrictUtility.Block(() => {
                        using(var stream = new MemoryStream(Encoding.UTF8.GetBytes(rankingXmlResult.Result))) {
                            return SerializeUtility.LoadXmlSerializeFromStream<FeedSmileVideoRankingModel>(stream);
                        }
                    });
                }
            });
            if(rankingFeedModel == null) {
                NowLoading = false;
                FinderLoadState = SmileVideoFinderLoadState.Failure;
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
                    FinderLoadState = SmileVideoFinderLoadState.ImageLoading;

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
                    FinderLoadState = SmileVideoFinderLoadState.Completed;
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

        #endregion

        #region ViewModelBase

        public override bool CanLoad
        {
            get
            {
                var loadSkips = new[] { SmileVideoFinderLoadState.VideoSourceLoading, SmileVideoFinderLoadState.VideoSourceChecking };
                return !loadSkips.Any(l => l == FinderLoadState);
            }
        }

        #endregion
    }
}
