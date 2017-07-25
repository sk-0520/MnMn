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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Exceptions.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Api;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video.Api;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Video.Parameter;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.View.Controls;
using ContentTypeTextNet.MnMn.MnMn.View.Controls.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Setting;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Bookmark;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.CheckItLater;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.History;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Local;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.MyList;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.NewArrivals;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Ranking;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Search;


namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video
{
    public class SmileVideoManagerViewModel: ManagerViewModelBase
    {
        public SmileVideoManagerViewModel(Mediator mediator)
            : base(mediator)
        {
            var settingResponse = Mediation.Request(new RequestModel(RequestKind.Setting, ServiceType.SmileVideo));
            Setting = (SmileVideoSettingModel)settingResponse.Result;

            Session = Mediation.GetResultFromRequest<SessionViewModelBase>(new RequestModel(RequestKind.Session, ServiceType.Smile));

            CheckItLaterCheckTimer.Tick += CheckItLaterCheckTimer_Tick;

            var rankingDefine = Mediation.GetResultFromRequest<IReadOnlySmileVideoRanking>(new RequestModel(RequestKind.RankingDefine, ServiceType.SmileVideo));
            RankingManager = new SmileVideoRankingManagerViewModel(Mediation, rankingDefine);

            var searchResponse = Mediation.Request(new RequestModel(RequestKind.SearchDefine, ServiceType.SmileVideo));
            var searchModel = (SmileVideoSearchModel)searchResponse.Result;
            SearchManager = new SmileVideoSearchManagerViewModel(Mediation, searchModel);

            NewArrivalsManager = new SmileVideoNewArrivalsManagerViewModel(Mediation);

            MyListManager = new SmileVideoMyListManagerViewModel(Mediation);

            HistoryManager = new SmileVideoHistoryManagerViewModel(Mediation);

            BookmarkManager = new SmileVideoBookmarkManagerViewModel(Mediation);

            CheckItLaterManager = new SmileVideoCheckItLaterManagerViewModel(Mediation);

            LaboratoryManager = new SmileVideoLaboratoryManagerViewModel(Mediation);

            Mediation.SetManager(
                ServiceType.SmileVideo,
                new SmileVideoManagerPackModel(
                    SearchManager,
                    RankingManager,
                    NewArrivalsManager,
                    MyListManager,
                    HistoryManager,
                    BookmarkManager,
                    CheckItLaterManager,
                    LaboratoryManager
                )
            );
        }

        #region property

        SmileVideoSettingModel Setting { get; }

        DispatcherTimer CheckItLaterCheckTimer { get; } = new DispatcherTimer() {
            Interval = Constants.ServiceSmileVideoCheckItLaterCheckTime,
        };

        public SessionViewModelBase Session { get; }

        public SmileVideoRankingManagerViewModel RankingManager { get; }
        public SmileVideoSearchManagerViewModel SearchManager { get; }
        public SmileVideoNewArrivalsManagerViewModel NewArrivalsManager { get; }
        public SmileVideoMyListManagerViewModel MyListManager { get; }
        public SmileVideoHistoryManagerViewModel HistoryManager { get; }
        public SmileVideoBookmarkManagerViewModel BookmarkManager { get; }
        public SmileVideoCheckItLaterManagerViewModel CheckItLaterManager { get; }
        public SmileVideoLaboratoryManagerViewModel LaboratoryManager { get; }

        #endregion

        #region command

        #endregion

        #region function

        Task CheckUpdateAsync()
        {
            var mylistTask = MyListManager.CheckMyListAsync();
            var tagBookmarkTask = SearchManager.UpdateBookmarkAsync();

            return Task.WhenAll(mylistTask, tagBookmarkTask).ContinueWith(t => {
                var addItemList = new List<SmileVideoVideoItemModel>();

                var myListItems = mylistTask.Result;
                foreach(var item in myListItems) {
                    Mediation.Request(new SmileVideoProcessRequestModel(new SmileVideoProcessCheckItLaterParameterModel(item, SmileVideoCheckItLaterFrom.MylistBookmark)));
                }

                var tagBookmarkItems = tagBookmarkTask.Result;
                foreach(var item in tagBookmarkItems) {
                    Mediation.Request(new SmileVideoProcessRequestModel(new SmileVideoProcessCheckItLaterParameterModel(item, SmileVideoCheckItLaterFrom.WordBookmark)));
                }
            });
        }

        long RemoveDirectory(DirectoryInfo directory)
        {
            if(directory.Exists) {
                var files = directory.GetFiles("*", SearchOption.AllDirectories);
                if(files.Length > 0) {
                    var totalSize = files.Sum(f => f.Length);
                    directory.Delete(true);
                    return totalSize;
                }
            }

            return 0;
        }

        #endregion

        #region ManagerViewModelBase

        protected override IEnumerable<ManagerViewModelBase> GetManagerChildren()
        {
            return new ManagerViewModelBase[] {
                SearchManager,
                RankingManager,
                NewArrivalsManager,
                MyListManager,
                HistoryManager,
                BookmarkManager,
                CheckItLaterManager,
            };
        }

        protected override void ShowViewCore()
        { }

        protected override void HideViewCore()
        { }

        Task<long> GarbageCollectionCahceVideoAsync(string videoId, DirectoryInfo baseDirectory, GarbageCollectionLevel garbageCollectionLevel, CacheSpan cacheSpan, bool force)
        {
            CheckUtility.EnforceNotNullAndNotWhiteSpace(videoId);

            var targetFile = SmileVideoInformationUtility.GetGetthumbinfoFile(Mediation, videoId);
            try {
                targetFile.Refresh();
            } catch(IOException ex) {
                Mediation.Logger.Warning(ex);
            }
            if(!targetFile.Exists && baseDirectory.Exists) {
                try {
                    // thumbなけりゃディレクトリごとポイする
                    var size = RemoveDirectory(baseDirectory);
                    return Task.FromResult(size);
                } catch(Exception ex) {
                    Mediation.Logger.Error($"{baseDirectory}: {ex}");
                }
                return GarbageCollectionDummyResult;
            }

            var request = new SmileVideoInformationCacheRequestModel(new SmileVideoInformationCacheParameterModel(videoId, CacheSpan.InfinityCache));
            var viewModelTask = Mediation.GetResultFromRequest<Task<SmileVideoInformationViewModel>>(request);
            return viewModelTask.ContinueWith(t => {
                if(t.IsFaulted) {
                    var exception = t.Exception.InnerException as SmileVideoGetthumbinfoFailureException;
                    Mediation.Logger.Information($"load fail: {exception}");
                    var dir = SmileVideoInformationUtility.GetCacheDirectory(Mediation, videoId);
                    if(dir.Exists) {
                        try {
                            return RemoveDirectory(dir);
                        } catch(Exception ex) {
                            Mediation.Logger.Error(ex);
                        }
                    }
                    return GarbageCollectionDummyResult.Result;
                } else {
                    var viewModel = t.Result;
                    var checkResult = viewModel.GarbageCollection(garbageCollectionLevel, cacheSpan, force);
                    return checkResult.Result;
                }
            });
        }

        Task<long> GarbageCollectionCahceVideosAsync(GarbageCollectionLevel garbageCollectionLevel, CacheSpan cacheSpan, bool force)
        {
            return Task.Run(async () => {
                var baseDirInfo = Mediation.GetResultFromRequest<DirectoryInfo>(new RequestModel(RequestKind.CacheDirectory, ServiceType.SmileVideo));
                var cacheDirInfos = baseDirInfo.GetDirectories("*", SearchOption.TopDirectoryOnly).ToEvaluatedSequence();
                long totalSize = 0;
                foreach(var dir in cacheDirInfos) {
                    var result = await GarbageCollectionCahceVideoAsync(dir.Name, dir, garbageCollectionLevel, cacheSpan, force);
                    totalSize += result;
                }
                return totalSize;
            });
        }

        public override Task InitializeAsync()
        {
            return Task.WhenAll(ManagerChildren.Select(m => m.InitializeAsync())).ContinueWith(_ => {
                // 裏で走らせとく
                if(NetworkUtility.IsNetworkAvailable) {
                    CheckUpdateAsync().ContinueWith(t => {
                        CheckItLaterCheckTimer.Stop();
                        CheckItLaterCheckTimer.Start();
                    });
                } else {
                    CheckItLaterCheckTimer.Stop();
                    CheckItLaterCheckTimer.Start();
                }
            });
        }

        public override Task UninitializeAsync()
        {
            return Task.WhenAll(ManagerChildren.Select(m => m.UninitializeAsync()));
        }

        public override void InitializeView(MainWindow view)
        {
            foreach(var item in ManagerChildren) {
                item.InitializeView(view);
            }
        }

        public override void UninitializeView(MainWindow view)
        {
            CheckItLaterCheckTimer.Stop();
            CheckItLaterCheckTimer.Tick -= CheckItLaterCheckTimer_Tick;

            foreach(var item in ManagerChildren) {
                item.UninitializeView(view);
            }
        }

        public override Task<long> GarbageCollectionAsync(GarbageCollectionLevel garbageCollectionLevel, CacheSpan cacheSpan, bool force)
        {
            var items = ManagerChildren
                .Select(m => m.GarbageCollectionAsync(garbageCollectionLevel, cacheSpan, force))
                .ToEvaluatedSequence()
            ;
            items.Add(GarbageCollectionCahceVideosAsync(garbageCollectionLevel, cacheSpan, force));

            return Task.WhenAll(items).ContinueWith(t => {
                return t.Result.Sum();
            });
        }

        #endregion

        async void CheckItLaterCheckTimer_Tick(object sender, EventArgs e)
        {
            CheckItLaterCheckTimer.Stop();
            try {
                await CheckUpdateAsync();
            } finally {
                CheckItLaterCheckTimer.Start();
            }
        }

    }
}
