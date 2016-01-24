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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Define.NicoNico.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.NicoNico.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.NicoNico.Video.Raw;
using ContentTypeTextNet.MnMn.MnMn.Model.NicoNico.Video.Raw.Feed.RankingRss2;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Control.NicoNico.Video
{
    public class VideoInformationViewModel: ViewModelBase
    {
        #region variable

        VideoThumbnailLoad _videoThumbnailLoad;
        ImageSource _thumbnailImage;

        #endregion

        VideoInformationViewModel(Mediation mediation, int number)
        {
            Mediation = mediation;
            Number = number;
            VideoThumbnailLoad = VideoThumbnailLoad.None;
        }

        public VideoInformationViewModel(Mediation mediation, RawVideoThumbModel thumb, int number)
            : this(mediation, number)
        {
            Thumb = thumb;

            VideoInformationSource = VideoInformationSource.Getthumbinfo;
        }

        public VideoInformationViewModel(Mediation mediation, RankingFeedItemModel ranking, int number)
            : this(mediation, number)
        {
            Ranking = ranking;
            RankingDetail = RankingUtility.ConvertRawDescription(Ranking.Description);
            RankingDetail.VideoId = RankingUtility.GetVideoId(Ranking);
            RankingDetail.Title = RankingUtility.GetTitle(Ranking.Title);

            VideoInformationSource = VideoInformationSource.Ranking;
        }

        #region property

        Mediation Mediation { get; set; }

        RawVideoThumbModel Thumb { get; set; }
        RankingFeedItemModel Ranking { get; set; }

        RawVideoRankingDetailModel RankingDetail { get; set; }

        public int Number { get; private set; }

        public VideoInformationSource VideoInformationSource { get; private set; }

        public VideoThumbnailLoad VideoThumbnailLoad
        {
            get { return this._videoThumbnailLoad; }
            set
            {
                if(SetVariableValue(ref this._videoThumbnailLoad, value)) {
                    CallOnPropertyChange(nameof(ThumbnailImage));
                }
            }
        }

        /// <summary>
        /// 見たことがある。
        /// </summary>
        public bool Visited { get;set;}
        /// <summary>
        /// キャッシュ済み。
        /// </summary>
        public bool Cached { get; set; }

        #region 生データから取得

        public string VideoId
        {
            get
            {
                switch(VideoInformationSource) {
                    case VideoInformationSource.Getthumbinfo:
                        return Thumb.VideoId;

                    case VideoInformationSource.Ranking:
                        return RankingDetail.VideoId;

                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public string Title
        {
            get
            {
                switch(VideoInformationSource) {
                    case VideoInformationSource.Getthumbinfo:
                        return Thumb.Title;

                    case VideoInformationSource.Ranking:
                        return RankingDetail.Title ?? Ranking.Title;

                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public Uri ThumbnailUri {
            get
            {
                switch(VideoInformationSource) {
                    case VideoInformationSource.Getthumbinfo:
                        return RawValueUtility.ConvertUri(Thumb.ThumbnailUrl);

                    case VideoInformationSource.Ranking:
                        return RawValueUtility.ConvertUri(RankingDetail.ThumbnailUrl);

                    default:
                        throw new NotImplementedException();
                }
            }
        }
        public DateTime FirstRetrieve { get {
                switch(VideoInformationSource) {
                    case VideoInformationSource.Getthumbinfo:
                        return RawValueUtility.ConvertDateTime(Thumb.FirstRetrieve);

                    case VideoInformationSource.Ranking:
                        return RawValueUtility.ConvertDateTime(RankingDetail.FirstRetrieve);

                    default:
                        throw new NotImplementedException();
                }
            } }
        public TimeSpan Length { get { return GetthumbinfoUtility.ConvertTimeSpan(Thumb.Length); } }
        public MovieType MovieType { get { return GetthumbinfoUtility.ConvertMovieType(Thumb.MovieType); } }
        public long SizeHigh { get { return RawValueUtility.ConvertLong(Thumb.SizeHigh); } }
        public long SizeLow{ get { return RawValueUtility.ConvertLong(Thumb.SizeLow); } }
        public int ViewCounter { get { return RawValueUtility.ConvertInteger(Thumb.ViewCounter); } }
        public int CommentNum { get { return RawValueUtility.ConvertInteger(Thumb.CommentNum); } }
        public int MylistCounter { get { return RawValueUtility.ConvertInteger(Thumb.MylistCounter); } }
        public Uri WatchUrl
        {
            get {
                switch(VideoInformationSource) {
                    case VideoInformationSource.Getthumbinfo:
                        return RawValueUtility.ConvertUri(Thumb.WatchUrl);

                    case VideoInformationSource.Ranking:
                        return RawValueUtility.ConvertUri(Ranking.Link);

                    default:
                        throw new NotImplementedException();
                }
            }
        }
        public ThumbType ThumbType { get { return GetthumbinfoUtility.ConvertThumbType(Thumb.ThumbType); } }
        public bool Embeddable { get { return GetthumbinfoUtility.IsEmbeddable(Thumb.Embeddable); } }
        public bool LivePlay { get { return GetthumbinfoUtility.IsLivePlay(Thumb.NoLivePlay); } }
        public Uri UserIconUrl { get { return RawValueUtility.ConvertUri(Thumb.UserIconUrl); } }

        #endregion

        public ImageSource ThumbnailImage
        {
            get
            {
                switch(VideoThumbnailLoad) {
                    case VideoThumbnailLoad.None:
                        return null;

                    case VideoThumbnailLoad.ImageChecking:
                        return null;

                    case VideoThumbnailLoad.ImageLoadingFromWeb:
                    case VideoThumbnailLoad.ImageLoadingFromStorage:
                        return null;

                    case VideoThumbnailLoad.Completed:
                        return this._thumbnailImage;

                    case VideoThumbnailLoad.Failure:
                        return null;

                    default:
                        throw new NotImplementedException();
                }
            }
        }


        #endregion

        #region function

        async Task LoadImageAsync_Impl()
        {
            VideoThumbnailLoad = VideoThumbnailLoad.ImageChecking;
            if(Cached) {
                VideoThumbnailLoad = VideoThumbnailLoad.ImageLoadingFromStorage;
            }
            var uri = ThumbnailUri;

            var binary = await RestrictUtility.Block(async () => {
                using(var userAgent = new HttpUserAgentHost()) {
                    return await userAgent.Client.GetByteArrayAsync(uri);
                }
            });
            using(var stream = new MemoryStream(binary)) {
                Debug.WriteLine(uri);
                Debug.WriteLine(binary.Length);
                var image = BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                FreezableUtility.SafeFreeze(image);
                this._thumbnailImage = image;
                VideoThumbnailLoad = VideoThumbnailLoad.Completed;
            }
            
            await Task.CompletedTask;
        }

        public async Task LoadImageAsync()
        {
            await LoadImageAsync_Impl();
        }

        #endregion
    }
}
