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
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Exceptions.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Video.Parameter;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.View.Controls;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.CheckItLater
{
    public class SmileVideoCheckItLaterManagerViewModel: SmileVideoCustomManagerViewModelBase
    {
        public SmileVideoCheckItLaterManagerViewModel(Mediation mediation)
            : base(mediation)
        {
            CheckItLaterFinder = new SmileVideoCheckItLaterFinderViewModel(Mediation);
        }

        #region property

        public SmileVideoCheckItLaterFinderViewModel CheckItLaterFinder { get; }

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
        public SmileVideoCheckItLaterModel AddLater(SmileVideoVideoItemModel videoItem, bool isForce)
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
                };
            } else {
                // 既存だがチェックされていない場合は上位に運ぶため一旦リストから破棄
                Setting.CheckItLater.Remove(item);
                item.CheckTimestamp = DateTime.Now;
                item.IsChecked = false;
            }
            Setting.CheckItLater.Insert(0, item);

            CallOnPropertyChangeDisplayItem();

            return item;
        }

        #endregion

        #region CheckItLaterManagerViewModel

        protected override IEnumerable<ManagerViewModelBase> GetManagerChildren()
        {
            return Enumerable.Empty<ManagerViewModelBase>();
        }

        protected override void ShowViewCore()
        {
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

        public override void InitializeView(MainWindow view)
        { }

        public override void UninitializeView(MainWindow view)
        { }

        public override Task<long> GarbageCollectionAsync(GarbageCollectionLevel garbageCollectionLevel, CacheSpan cacheSpan, bool force)
        {
            var span = Constants.ServiceSmileVideoCheckItLaterCacheSpan;
            var gcItems = Setting.CheckItLater
                .Where(c => !span.IsCacheTime(c.CheckTimestamp))
                .ToArray()
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
