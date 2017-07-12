﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw.Feed;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Channel
{
    public class SmileChannelVideoFinderViewModel : SmileVideoFeedFinderViewModelBase
    {
        public SmileChannelVideoFinderViewModel(Mediation mediation, string channelId)
            : base(mediation, 0)
        {
            ChannelId = channelId;
        }

        #region property

        public string ChannelId { get; }

        #endregion

        #region SmileVideoFeedFinderViewModelBase

        protected override SmileVideoInformationFlags InformationFlags => SmileVideoInformationFlags.None;

        protected override Task<FeedSmileVideoModel> LoadFeedAsync()
        {
            return SmileChannelUtility.LoadAllVideoFeedAsync(Mediation, ChannelId).ContinueWith(t => {
                var channelFeeds = t.Result;

                var videoFeed = new FeedSmileVideoModel();

                if(!channelFeeds.Any(i => i.Channel.Items.Any())) {
                    return videoFeed;
                }

                // リフレクションはやめとけって石器時代の先祖が言ってる

                var channelFeed = channelFeeds.First();

                videoFeed.Version = channelFeed.Version;
                videoFeed.Channel.Description = channelFeed.Channel.Description;
                videoFeed.Channel.Copyright = channelFeed.Channel.Copyright;
                videoFeed.Channel.Language = channelFeed.Channel.Language;
                videoFeed.Channel.Link = channelFeed.Channel.Link;

                foreach(var item in channelFeeds.SelectMany(c => c.Channel.Items)) {
                    var videoItem = new FeedSmileVideoItemModel() {
                        Category = item.Category,
                        Guid = item.Guid,
                        Description = item.Description,
                        Link = item.Link,
                        PubDate = item.PubDate,
                        Title = item.Title,
                    };
                    videoFeed.Channel.Items.Add(videoItem);
                }

                return videoFeed;
            });
        }

        #endregion
    }
}
