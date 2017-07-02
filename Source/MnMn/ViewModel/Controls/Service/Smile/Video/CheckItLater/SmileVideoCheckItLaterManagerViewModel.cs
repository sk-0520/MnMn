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
            CheckItLaterFinder = new SmileVideoCheckItLaterFinderViewModel(Mediation);

            AllItemsFinder = new SmileVideoCheckItLaterFinderViewModel2(Mediation);
            ManualOperationFinder = new SmileVideoCheckItLaterFinderViewModel2(Mediation);
        }

        #region property

        public SmileVideoCheckItLaterFinderViewModel CheckItLaterFinder { get; }

        IReadOnlyList<SmileVideoCheckItLaterModel> CheckItLaterItem { get; set; }

        public SmileVideoCheckItLaterFinderViewModel2 AllItemsFinder { get; }
        public SmileVideoCheckItLaterFinderViewModel2 ManualOperationFinder { get; }
        public CollectionModel<SmileVideoCheckItLaterFinderViewModel2> MylistBookmarkFinderItems { get; } = new CollectionModel<SmileVideoCheckItLaterFinderViewModel2>();
        public CollectionModel<SmileVideoCheckItLaterFinderViewModel2> UserBookmarkFinderItems { get; } = new CollectionModel<SmileVideoCheckItLaterFinderViewModel2>();
        public CollectionModel<SmileVideoCheckItLaterFinderViewModel2> WordBookmarkFinderItems { get; } = new CollectionModel<SmileVideoCheckItLaterFinderViewModel2>();

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

        void BuildFinders()
        {
            CheckItLaterItem = Setting.CheckItLater
                .Where(i => !i.IsChecked)
                .ToEvaluatedSequence()
            ;

            AllItemsFinder.SetVideoItems(new SmileVideoCheckItLaterFromModel() { }, CheckItLaterItem);
            ManualOperationFinder.SetVideoItems(new SmileVideoCheckItLaterFromModel() { }, CheckItLaterItem.Where(i => i.CheckItLaterFrom == SmileVideoCheckItLaterFrom.ManualOperation || i.CheckItLaterFrom == SmileVideoCheckItLaterFrom.Unknown));
        }

        #endregion

        #region CheckItLaterManagerViewModel

        protected override IEnumerable<ManagerViewModelBase> GetManagerChildren()
        {
            return Enumerable.Empty<ManagerViewModelBase>();
        }

        protected override void ShowViewCore()
        {
            BuildFinders();
            CheckItLaterFinder.LoadDefaultCacheAsync().ContinueWith(_ => {
                CallOnPropertyChangeDisplayItem();
            }).ConfigureAwait(false);
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
