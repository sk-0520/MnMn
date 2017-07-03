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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Exceptions.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Video.Parameter;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.View.Controls;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.CheckItLater
{
    public class SmileVideoCheckItLaterManagerViewModel : SmileVideoCustomManagerViewModelBase
    {
        #region variable
        /*
        bool _isSelectedAllItemsFinder;
        bool _isSelectedManualOperationFinder;
        bool _isSelectedMylistBookmarkFinderItems;
        bool _isSelectedUserBookmarkFinderItems;
        bool _isSelectedWordBookmarkFinderItems;
        */
        #endregion

        public SmileVideoCheckItLaterManagerViewModel(Mediation mediation)
            : base(mediation)
        {
            AllItemsFinder = new SmileVideoCheckItLaterFinderViewModel(Mediation);
            ManualOperationFinder = new SmileVideoCheckItLaterFinderViewModel(Mediation);
        }

        #region property

        IReadOnlyList<SmileVideoCheckItLaterModel> CheckItLaterItem { get; set; }

        public SmileVideoCheckItLaterFinderViewModel AllItemsFinder { get; }
        public SmileVideoCheckItLaterFinderViewModel ManualOperationFinder { get; }
        public CollectionModel<SmileVideoCheckItLaterFinderViewModel> MylistBookmarkFinderItems { get; } = new CollectionModel<SmileVideoCheckItLaterFinderViewModel>();
        public CollectionModel<SmileVideoCheckItLaterFinderViewModel> UserBookmarkFinderItems { get; } = new CollectionModel<SmileVideoCheckItLaterFinderViewModel>();
        public CollectionModel<SmileVideoCheckItLaterFinderViewModel> WordBookmarkFinderItems { get; } = new CollectionModel<SmileVideoCheckItLaterFinderViewModel>();

        /*
        public bool IsSelectedAllItemsFinder
        {
            get { return this._isSelectedAllItemsFinder; }
            set
            {
                if(SetVariableValue(ref this._isSelectedAllItemsFinder, value)) {
                    if(IsSelectedAllItemsFinder) {
                        AllItemsFinder.LoadDefaultCacheAsync().ConfigureAwait(false);
                    }
                }
            }
        }

        public bool IsSelectedManualOperationFinder
        {
            get { return this._isSelectedManualOperationFinder; }
            set
            {
                if(SetVariableValue(ref this._isSelectedManualOperationFinder, value)) {
                    if(IsSelectedManualOperationFinder) {
                        ManualOperationFinder.LoadDefaultCacheAsync().ConfigureAwait(false);
                    }
                }
            }
        }
        public bool IsSelectedMylistBookmarkFinderItems
        {
            get { return this._isSelectedMylistBookmarkFinderItems; }
            set
            {
                if(SetVariableValue(ref this._isSelectedMylistBookmarkFinderItems, value)) {
                }
            }
        }
        public bool IsSelectedUserBookmarkFinderItems
        {
            get { return this._isSelectedUserBookmarkFinderItems; }
            set
            {
                if(SetVariableValue(ref this._isSelectedUserBookmarkFinderItems, value)) {
                }
            }
        }
        public bool IsSelectedWordBookmarkFinderItems
        {
            get { return this._isSelectedWordBookmarkFinderItems; }
            set
            {
                if(SetVariableValue(ref this._isSelectedWordBookmarkFinderItems, value)) {
                }
            }
        }
        */

        public bool HasItem
        {
            get { return ItemCount > 0; }
        }

        public int ItemCount
        {
            get { return Setting.CheckItLater.Count(i => !i.IsChecked); }
        }

        #endregion

        #region command

        public ICommand RemoveCheckedVideosCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        RemoveCheckedVideos();
                    }
                );
            }
        }

        #endregion

        #region function

        /// <summary>
        /// 後で見るに追加。
        /// </summary>
        /// <param name="videoItem"></param>
        /// <param name="isForce"></param>
        /// <returns>追加できなかった場合は null。</returns>
        public SmileVideoCheckItLaterModel AddLater(SmileVideoVideoItemModel videoItem, SmileVideoCheckItLaterFrom checkItLaterFrom, bool isForce)
        {
            var item = Setting.CheckItLater.FirstOrDefault(c => c.VideoId == videoItem.VideoId);
            // 強制でなければ既に存在してチェック済みのものは追加しない
            if(!isForce && item != null && !item.IsChecked) {
                return null;
            }

            if(item == null) {
                item = new SmileVideoCheckItLaterModel() {
                    VideoId = videoItem.VideoId,
                    VideoTitle = videoItem.VideoTitle,
                    FirstRetrieve = videoItem.FirstRetrieve,
                    Length = videoItem.Length,
                    WatchUrl = videoItem.WatchUrl,
                    CheckTimestamp = DateTime.Now,
                    IsChecked = false,
                    CheckItLaterFrom = checkItLaterFrom,
                };
            } else {
                // 既存だがチェックされていない場合は上位に運ぶため一旦リストから破棄
                Setting.CheckItLater.Remove(item);
                item.CheckTimestamp = DateTime.Now;
                item.IsChecked = false;
                // 下位互換の Unknown を指定値で上書き
                item.CheckItLaterFrom = checkItLaterFrom;
            }

            var checkItLaterFromTag = videoItem.VolatileTag as IReadOnlySmileVideoCheckItLaterFrom;
            if(checkItLaterFromTag != null) {
                item.FromId = checkItLaterFromTag.FromId;
                item.FromName = checkItLaterFromTag.FromName;
            }

            //Setting.CheckItLater.Insert(0, item);
            AppUtility.PlusItem(Setting.CheckItLater, item);

            CallOnPropertyChangeDisplayItem();

            return item;
        }

        void RemoveCheckedVideos()
        {
            var items = AllItemsFinder.GetCheckedItems()
                .Select(i => i.Information)
                .ToEvaluatedSequence();
            ;

            if(items.Any()) {
                foreach(var item in items) {
                    var model = Setting.CheckItLater.FirstOrDefault(i => i.VideoId == item.VideoId || i?.WatchUrl.OriginalString == item?.WatchUrl.OriginalString);
                    //var model = Setting.CheckItLater.FirstOrDefault(i => i.VideoId == item.VideoId);
                    if(model != null) {
                        model.CheckTimestamp = DateTime.Now;
                        model.IsChecked = true;
                    }
                }
                AllItemsFinder.FinderItems.Refresh();
            }
            BuildFinders(false);
        }

        IEnumerable<SmileVideoCheckItLaterFinderViewModel> BuildGroupFinders(SmileVideoCheckItLaterFrom checkItLaterFrom, IEnumerable<SmileVideoCheckItLaterModel> items)
        {
            //TODO: ローカライズ
            var allFinder = new SmileVideoCheckItLaterFinderViewModel(Mediation);
            allFinder.SetVideoItems(new SmileVideoCheckItLaterFromModel() { FromName = Properties.Resources.String_Service_Smile_SmileVideo_CheckItLater_Group_AllItems }, items);
            yield return allFinder;

            var groups = items
                .IfElse(
                    checkItLaterFrom != SmileVideoCheckItLaterFrom.WordBookmark,
                    seq => seq.GroupBy(i => i.FromId),
                    seq => seq.GroupBy(i => i.FromName)
                )
                .OrderBy(g => g.Key)
            ;
            foreach(var group in groups) {
                var finder = new SmileVideoCheckItLaterFinderViewModel(Mediation);
                finder.SetVideoItems(group.First(), group);
                yield return finder;
            }
        }

        void BuildFinders(bool refreshAllItemsFinder)
        {
            CheckItLaterItem = Setting.CheckItLater
                .Where(i => !i.IsChecked)
                .ToEvaluatedSequence()
            ;

            // 削除時は全アイテムを再設定する必要なし
            if(refreshAllItemsFinder) {
                AllItemsFinder.SetVideoItems(new SmileVideoCheckItLaterFromModel() { FromName = Properties.Resources.String_Service_Smile_SmileVideo_CheckItLater_AllItems }, CheckItLaterItem);
            }

            var manualOperationItems = CheckItLaterItem
                .Where(i => i.CheckItLaterFrom == SmileVideoCheckItLaterFrom.ManualOperation || i.CheckItLaterFrom == SmileVideoCheckItLaterFrom.Unknown)
            ;
            ManualOperationFinder.SetVideoItems(new SmileVideoCheckItLaterFromModel() { FromName = Properties.Resources.String_Service_Smile_SmileVideo_CheckItLater_ManualOperation }, manualOperationItems);

            var groups = CheckItLaterItem
                .Where(i => i.CheckItLaterFrom != SmileVideoCheckItLaterFrom.Unknown)
                .Where(i => i.CheckItLaterFrom != SmileVideoCheckItLaterFrom.ManualOperation)
                .GroupBy(i => i.CheckItLaterFrom)
                .Select(g => new { GroupKey = g.Key, Items = BuildGroupFinders(g.Key, g) })
            ;
            var finderMap = new Dictionary<SmileVideoCheckItLaterFrom, CollectionModel<SmileVideoCheckItLaterFinderViewModel>>() {
                [SmileVideoCheckItLaterFrom.MylistBookmark] = MylistBookmarkFinderItems,
                [SmileVideoCheckItLaterFrom.UserBookmark] = UserBookmarkFinderItems,
                [SmileVideoCheckItLaterFrom.WordBookmark] = WordBookmarkFinderItems,
            };
            foreach(var group in groups) {
                finderMap[group.GroupKey].InitializeRange(group.Items);
            }
        }

        #endregion

        #region CheckItLaterManagerViewModel

        protected override IEnumerable<ManagerViewModelBase> GetManagerChildren()
        {
            return Enumerable.Empty<ManagerViewModelBase>();
        }

        protected override void ShowViewCore()
        {
            BuildFinders(true);
        }

        protected override void HideViewCore()
        {
            CallOnPropertyChangeDisplayItem();
        }

        public override async Task InitializeAsync()
        {
            if(!NetworkUtility.IsNetworkAvailable) {
                Mediation.Logger.Information("skip check it later");
                return;
            }
            // 動画IDの補正処理
            foreach(var item in Setting.CheckItLater) {
                if(SmileIdUtility.NeedCorrectionVideoId(item.VideoId)) {
                    var request = new SmileVideoInformationCacheRequestModel(new SmileVideoInformationCacheParameterModel(item.VideoId, Constants.ServiceSmileVideoThumbCacheSpan));
                    try {
                        var info = await Mediation.GetResultFromRequest<Task<SmileVideoInformationViewModel>>(request);
                        item.VideoId = info.VideoId;
                    } catch(SmileVideoGetthumbinfoFailureException ex) {
                        // やっばいことになったら破棄
                        Mediation.Logger.Warning(ex);
                        item.IsChecked = false;
                    }
                }
            }

            //return Task.CompletedTask;
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
            var span = Constants.ServiceSmileVideoCheckItLaterCacheSpan;
            var gcItems = Setting.CheckItLater
                .Where(c => !span.IsCacheTime(c.CheckTimestamp))
                .ToEvaluatedSequence()
            ;
            foreach(var gcItem in gcItems) {
                Setting.CheckItLater.Remove(gcItem);
            }
            CallOnPropertyChangeDisplayItem();

            return GarbageCollectionDummyResult;
        }

        protected override void CallOnPropertyChangeDisplayItem()
        {
            base.CallOnPropertyChangeDisplayItem();

            var propertyName = new[] {
                nameof(ItemCount),
                nameof(HasItem),
            };
            CallOnPropertyChange(propertyName);
        }

        #endregion

    }
}
