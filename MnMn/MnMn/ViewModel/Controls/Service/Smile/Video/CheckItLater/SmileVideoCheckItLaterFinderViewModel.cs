using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw.Feed;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.CheckItLater
{
    public class SmileVideoCheckItLaterFinderViewModel: SmileVideoFeedFinderViewModelBase
    {
        public SmileVideoCheckItLaterFinderViewModel(Mediation mediation)
            : base(mediation)
        { }

        #region command

        public ICommand RemoveCheckedVideosCommand
        {
            get { return CreateCommand(o => RemoveCheckedVideos()); }
        }

        #endregion

        #region function


        void RemoveCheckedVideos()
        {
            var items = VideoInformationItems
                .Cast<SmileVideoInformationViewModel>()
                .Where(v => v.IsChecked.GetValueOrDefault())
                .ToArray();
            ;

            if(items.Any()) {
                foreach(var item in items) {
                    var model = Setting.CheckItLater.FirstOrDefault(i => i.VideoId == item.VideoId);
                    if(model != null) {
                        model.CheckTimestamp = DateTime.Now;
                        model.IsChecked = true;
                    }
                }
            }
        }

        #endregion

        #region SmileVideoFeedFinderViewModelBase

        protected override SmileVideoInformationFlags InformationFlags => SmileVideoInformationFlags.Length;

        protected override Task<FeedSmileVideoModel> LoadFeedAsync()
        {
            var result = new FeedSmileVideoModel();
            foreach(var model in Setting.CheckItLater.Where(c => !c.IsChecked)) {
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
