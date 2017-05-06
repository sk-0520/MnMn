using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Parameter;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Raw;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw.Feed;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Video.Parameter
{
    public class SmileVideoInformationCacheParameterModel: CacheDataParameterModel
    {
        public SmileVideoInformationCacheParameterModel(string videoId, CacheSpan thumbCacheSpan)
        {
            InformationSource = SmileVideoInformationSource.VideoId;

            VideoId = videoId;
            ThumbCacheSpan = thumbCacheSpan;
        }

        public SmileVideoInformationCacheParameterModel(RawSmileVideoThumbModel thumb)
        {
            InformationSource = SmileVideoInformationSource.Getthumbinfo;

            Thumb = thumb;
        }

        public SmileVideoInformationCacheParameterModel(RawSmileContentsSearchItemModel contentsSearch)
        {
            InformationSource = SmileVideoInformationSource.ContentsSearch;

            ContentsSearch = contentsSearch;
        }

        public SmileVideoInformationCacheParameterModel(RawSmileVideoSearchItemModel officialSearch)
        {
            InformationSource = SmileVideoInformationSource.OfficialSearch;

            OfficialSearch = officialSearch;
        }

        public SmileVideoInformationCacheParameterModel(FeedSmileVideoItemModel feed, SmileVideoInformationFlags informationFlags)
        {
            InformationSource = SmileVideoInformationSource.Feed;

            Feed = feed;
            InformationFlags = informationFlags;
        }

        #region property

        public SmileVideoInformationSource InformationSource { get; }

        public string VideoId { get; set; }
        public CacheSpan ThumbCacheSpan { get; set; }

        public RawSmileVideoThumbModel Thumb { get; set; }

        public RawSmileContentsSearchItemModel ContentsSearch { get; set; }
        public RawSmileVideoSearchItemModel OfficialSearch { get; set; }

        public FeedSmileVideoItemModel Feed { get; set; }
        public SmileVideoInformationFlags InformationFlags { get; set; }

        #endregion
    }
}
