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
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Api;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video.Api;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.View.Controls.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Setting;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Bookmark;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.CheckItLater;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.History;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.MyList;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.NewArrivals;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Ranking;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Search;
using MnMn.View.Controls;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video
{
    public class SmileVideoManagerViewModel: ManagerViewModelBase
    {
        public SmileVideoManagerViewModel(Mediation mediation)
            : base(mediation)
        {
            var settingResponse = Mediation.Request(new RequestModel(RequestKind.Setting, ServiceType.SmileVideo));
            Setting = (SmileVideoSettingModel)settingResponse.Result;

            Session = Mediation.GetResultFromRequest<SessionViewModelBase>(new RequestModel(RequestKind.Session, ServiceType.Smile));

            var rankingResponse = Mediation.Request(new RequestModel(RequestKind.RankingDefine, ServiceType.SmileVideo));
            var rankingModel = (SmileVideoRankingModel)rankingResponse.Result;
            RankingManager = new SmileVideoRankingManagerViewModel(Mediation, rankingModel);

            var searchResponse = Mediation.Request(new RequestModel(RequestKind.SearchDefine, ServiceType.SmileVideo));
            var searchModel = (SmileVideoSearchModel)searchResponse.Result;
            SearchManager = new SmileVideoSearchManagerViewModel(Mediation, searchModel);

            NewArrivalsManager = new SmileVideoNewArrivalsManagerViewModel(Mediation);

            MyListManager = new SmileVideoMyListManagerViewModel(Mediation);

            HistoryManager = new SmileVideoHistoryManagerViewModel(Mediation);

            BookmarkManager = new SmileVideoBookmarkManagerViewModel(Mediation);

            CheckItLaterManager = new SmileVideoCheckItLaterManagerViewModel(Mediation);

            Mediation.SetManager(
                ServiceType.SmileVideo,
                new SmileVideoManagerPackModel(
                    SearchManager,
                    RankingManager,
                    NewArrivalsManager,
                    MyListManager,
                    HistoryManager,
                    BookmarkManager,
                    CheckItLaterManager
                )
            );
        }

        #region property

        SmileVideoSettingModel Setting { get; }
        MainWindow View { get; set; }

        public SessionViewModelBase Session { get; }

        public SmileVideoRankingManagerViewModel RankingManager { get; }
        public SmileVideoSearchManagerViewModel SearchManager { get; }
        public SmileVideoNewArrivalsManagerViewModel NewArrivalsManager { get; }
        public SmileVideoMyListManagerViewModel MyListManager { get; }
        public SmileVideoHistoryManagerViewModel HistoryManager { get; }
        public SmileVideoBookmarkManagerViewModel BookmarkManager { get; }
        public SmileVideoCheckItLaterManagerViewModel CheckItLaterManager { get; }

        public IEnumerable<SmileVideoCustomManagerViewModelBase> ManagerItems => new SmileVideoCustomManagerViewModelBase[] {
            SearchManager,
            RankingManager,
            NewArrivalsManager,
            MyListManager,
            HistoryManager,
            BookmarkManager,
            CheckItLaterManager,
        };

        #endregion

        #region command

        #endregion

        #region function

        Task CheckUpdateAsync()
        {
            var mylistTask = MyListManager.CheckMyListAsync();

            return Task.WhenAll(mylistTask).ContinueWith(t => {
                var addItemList = new List<SmileVideoVideoItemModel>();

                var myListItems = mylistTask.Result;

                foreach(var item in myListItems) {
                    var addItem = CheckItLaterManager.AddLater(item);
                    addItemList.Add(addItem);
                }
            });
        }

        #endregion

        #region ManagerViewModelBase

        Task GarbageCollectionCahceVideosAsync()
        {
            var appSetting = Mediation.GetResultFromRequest<AppSettingModel>(new RequestModel(RequestKind.Setting, ServiceType.Application));
            if(appSetting.CacheLifeTime == TimeSpan.Zero) {
                return Task.CompletedTask;
            }

            return Task.Run(() => {
                var videoExts = new[] {
                SmileVideoMovieType.Mp4,
                SmileVideoMovieType.Swf,
                SmileVideoMovieType.Flv,
            }
                    .Select(t => SmileVideoGetthumbinfoUtility.GetFileExtension(t))
                    .Select(s => "." + s)
                    .ToArray()
                ;


                var cacheSpan = new CacheSpan(DateTime.Now, appSetting.CacheLifeTime);

                var dirInfo = Mediation.GetResultFromRequest<DirectoryInfo>(new RequestModel(RequestKind.CacheDirectory, ServiceType.SmileVideo));
                var gcFiles = dirInfo.EnumerateFileSystemInfos("*", SearchOption.AllDirectories)
                    .Where(f => videoExts.Any(v => string.Equals(f.Extension, v, StringComparison.OrdinalIgnoreCase)))
                    .Where(f => !cacheSpan.IsCacheTime(f.CreationTime))
                ;
                foreach(var item in gcFiles) {
                    try {
                        Mediation.Logger.Trace($"GC:SMILE:VIDEO: {item.FullName}");
                        File.Delete(item.FullName);
                    } catch(Exception ex) {
                        Mediation.Logger.Warning(ex);
                    }
                }
            });
        }

        public override Task InitializeAsync()
        {
            return Task.WhenAll(ManagerItems.Select(m => m.InitializeAsync()));
        }

        public override void InitializeView(MainWindow view)
        {
            View = view;
            View.Loaded += View_Loaded;

            foreach(var item in ManagerItems) {
                item.InitializeView(view);
            }
        }

        public override void UninitializeView(MainWindow view)
        {
            foreach(var item in ManagerItems) {
                item.UninitializeView(view);
            }
        }

        public override Task GarbageCollectionAsync()
        {
            var items = ManagerItems
                .Select(m => m.GarbageCollectionAsync())
                .ToList()
            ;
            items.Add(GarbageCollectionCahceVideosAsync());

            return Task.WhenAll(items);
        }

        #endregion

        private void View_Loaded(object sender, RoutedEventArgs e)
        {
            View.Loaded -= View_Loaded;

            CheckUpdateAsync();
        }


    }
}
