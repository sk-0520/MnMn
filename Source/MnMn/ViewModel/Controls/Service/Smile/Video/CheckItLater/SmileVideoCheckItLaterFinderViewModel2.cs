using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw.Feed;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.CheckItLater
{
    public class SmileVideoCheckItLaterFinderViewModel2 : SmileVideoFeedFinderViewModelBase
    {
        #region variable

        bool _isSelected;

        #endregion

        public SmileVideoCheckItLaterFinderViewModel2(Mediation mediation)
            : base(mediation, 0)
        { }

        #region property

        Dictionary<string, SmileVideoCheckItLaterModel> IdAndUrlLaterMap { get; } = new Dictionary<string, SmileVideoCheckItLaterModel>();

        IReadOnlySmileVideoCheckItLaterFrom CheckItLaterFrom { get; set; }
        IReadOnlyList<SmileVideoCheckItLaterModel> SettedVideoItems { get; set; }

        public bool IsSelected
        {
            get { return this._isSelected; }
            set
            {
                if(SetVariableValue(ref this._isSelected, value)) {
                    if(IsSelected) {
                        LoadDefaultCacheAsync().ConfigureAwait(false);
                    }
                }
            }
        }

        #endregion

        #region command

        public ICommand RemoveCheckedVideosCommand
        {
            get { return CreateCommand(o => RemoveCheckedVideos()); }
        }

        #endregion

        #region function

        public void SetVideoItems(IReadOnlySmileVideoCheckItLaterFrom checkItLaterFrom, IEnumerable<SmileVideoCheckItLaterModel> videoItems)
        {
            CheckItLaterFrom = checkItLaterFrom;
            SettedVideoItems = videoItems.ToEvaluatedSequence();
        }

        void RemoveCheckedVideos()
        {
            var items = GetCheckedItems()
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
                FinderItems.Refresh();
            }
        }

        #endregion

        #region SmileVideoFeedFinderViewModelBase

        protected override SmileVideoInformationFlags InformationFlags => SmileVideoInformationFlags.Length;

        public override bool IsUsingFinderFilter { get; } = false;

        public override bool IsEnabledCheckItLaterMenu { get; } = false;

        public override bool IsEnabledDownloadMenu => true;

        protected override bool FilterItems(object obj)
        {
            var baseResult = base.FilterItems(obj);
            if(!baseResult) {
                return baseResult;
            }

            var finderItem = (SmileVideoFinderItemViewModel)obj;
            var viewModel = finderItem.Information;

            SmileVideoCheckItLaterModel checkItLater;
            if(IdAndUrlLaterMap.TryGetValue(viewModel.VideoId, out checkItLater)) {
                return !checkItLater.IsChecked;
            }
            if(viewModel.WatchUrl != null && IdAndUrlLaterMap.TryGetValue(viewModel.WatchUrl.OriginalString, out checkItLater)) {
                return !checkItLater.IsChecked;
            }

            // 表示できないよりは表示できた方がいいんじゃないかと
            return true;
        }

        protected override Task<FeedSmileVideoModel> LoadFeedAsync()
        {
            IdAndUrlLaterMap.Clear();

            var result = new FeedSmileVideoModel();
            foreach(var model in SettedVideoItems) {
                if(IdAndUrlLaterMap.ContainsKey(model.VideoId)) {
                    continue;
                }
                IdAndUrlLaterMap.Add(model.VideoId, model);

                // 視聴ページが既に含まれている場合はもう何もしない
                if(model.WatchUrl != null) {
                    if(IdAndUrlLaterMap.ContainsKey(model.WatchUrl.OriginalString)) {
                        continue;
                    }

                    IdAndUrlLaterMap.Add(model.WatchUrl.OriginalString, model);
                }

                var item = new FeedSmileVideoItemModel();

                item.Title = model.VideoTitle;
                item.Link = model.WatchUrl.OriginalString;

                var detailModel = new RawSmileVideoFeedDetailModel();
                detailModel.Title = model.VideoTitle;
                detailModel.VideoId = model.VideoId;
                detailModel.FirstRetrieve = model.FirstRetrieve.ToString("s");
                detailModel.Length = SmileVideoFeedUtility.ConvertM3H2TimeFromTimeSpan(model.Length);

                item.Description = SmileVideoFeedUtility.ConvertDescriptionFromFeedDetailModel(detailModel);
                result.Channel.Items.Add(item);
            }

            return Task.FromResult(result);
        }

        #endregion
    }
}
