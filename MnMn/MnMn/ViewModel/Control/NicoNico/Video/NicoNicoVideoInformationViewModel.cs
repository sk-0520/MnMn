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
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.NicoNico.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.NicoNico.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.NicoNico.Video.Raw;
using ContentTypeTextNet.MnMn.MnMn.Model.NicoNico.Video.Raw.Feed.RankingRss2;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.NicoNico.Video
{
    public class NicoNicoVideoInformationViewModel: ViewModelBase
    {
        #region define

        public static int NoOrderd => -1;

        #endregion

        #region variable

        NicoNicoVideoVideoThumbnailLoad _videoThumbnailLoad;
        ImageSource _thumbnailImage;

        bool? _isEconomyMode;

        #endregion

        NicoNicoVideoInformationViewModel(Mediation mediation, int number)
        {
            Mediation = mediation;
            Number = number;
            VideoThumbnailLoad = NicoNicoVideoVideoThumbnailLoad.None;
        }

        public NicoNicoVideoInformationViewModel(Mediation mediation, RawNicoNicoVideoThumbModel thumb, int number)
            : this(mediation, number)
        {
            Thumb = thumb;

            VideoInformationSource = NicoNicoVideoVideoInformationSource.Getthumbinfo;
        }

        public NicoNicoVideoInformationViewModel(Mediation mediation, FeedNicoNicoVideoRankingItemModel ranking, int number)
            : this(mediation, number)
        {
            Ranking = ranking;
            RankingDetail = NicoNicoVideoRankingUtility.ConvertRawDescription(Ranking.Description);
            RankingDetail.VideoId = NicoNicoVideoRankingUtility.GetVideoId(Ranking);
            RankingDetail.Title = NicoNicoVideoRankingUtility.GetTitle(Ranking.Title);

            VideoInformationSource = NicoNicoVideoVideoInformationSource.Ranking;
        }

        #region property

        Mediation Mediation { get; set; }

        RawNicoNicoVideoThumbModel Thumb { get; set; }
        FeedNicoNicoVideoRankingItemModel Ranking { get; set; }

        RawNicoNicoVideoRankingDetailModel RankingDetail { get; set; }

        RawNicoNicoVideoGetflvModel Getflv { get; set; }

        public int Number { get; private set; }

        public NicoNicoVideoVideoInformationSource VideoInformationSource { get; private set; }

        public NicoNicoVideoVideoThumbnailLoad VideoThumbnailLoad
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
                    case NicoNicoVideoVideoInformationSource.Getthumbinfo:
                        return Thumb.VideoId;

                    case NicoNicoVideoVideoInformationSource.Ranking:
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
                    case NicoNicoVideoVideoInformationSource.Getthumbinfo:
                        return Thumb.Title;

                    case NicoNicoVideoVideoInformationSource.Ranking:
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
                    case NicoNicoVideoVideoInformationSource.Getthumbinfo:
                        return RawValueUtility.ConvertUri(Thumb.ThumbnailUrl);

                    case NicoNicoVideoVideoInformationSource.Ranking:
                        return RawValueUtility.ConvertUri(RankingDetail.ThumbnailUrl);

                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public DateTime FirstRetrieve { get {
                switch(VideoInformationSource) {
                    case NicoNicoVideoVideoInformationSource.Getthumbinfo:
                        return RawValueUtility.ConvertDateTime(Thumb.FirstRetrieve);

                    case NicoNicoVideoVideoInformationSource.Ranking:
                        return RawValueUtility.ConvertDateTime(RankingDetail.FirstRetrieve);

                    default:
                        throw new NotImplementedException();
                }
            } }

        public TimeSpan Length { get { return NicoNicoVideoGetthumbinfoUtility.ConvertTimeSpan(Thumb.Length); } }
        public NicoNicoVideoMovieType MovieType { get { return NicoNicoVideoGetthumbinfoUtility.ConvertMovieType(Thumb.MovieType); } }
        public long SizeHigh { get { return RawValueUtility.ConvertLong(Thumb.SizeHigh); } }
        public long SizeLow{ get { return RawValueUtility.ConvertLong(Thumb.SizeLow); } }
        public int ViewCounter { get { return RawValueUtility.ConvertInteger(Thumb.ViewCounter); } }
        public int CommentNum { get { return RawValueUtility.ConvertInteger(Thumb.CommentNum); } }
        public int MylistCounter { get { return RawValueUtility.ConvertInteger(Thumb.MylistCounter); } }
        public Uri WatchUrl
        {
            get {
                switch(VideoInformationSource) {
                    case NicoNicoVideoVideoInformationSource.Getthumbinfo:
                        return RawValueUtility.ConvertUri(Thumb.WatchUrl);

                    case NicoNicoVideoVideoInformationSource.Ranking:
                        return RawValueUtility.ConvertUri(Ranking.Link);

                    default:
                        throw new NotImplementedException();
                }
            }
        }
        public NicoNicoVideoThumbType ThumbType { get { return NicoNicoVideoGetthumbinfoUtility.ConvertThumbType(Thumb.ThumbType); } }
        public bool Embeddable { get { return NicoNicoVideoGetthumbinfoUtility.IsEmbeddable(Thumb.Embeddable); } }
        public bool LivePlay { get { return NicoNicoVideoGetthumbinfoUtility.IsLivePlay(Thumb.NoLivePlay); } }
        public Uri UserIconUrl { get { return RawValueUtility.ConvertUri(Thumb.UserIconUrl); } }

        #region Getflv

        public bool Done
        {
            get
            {
                ThrowHasNotGetflv();
                return RawValueUtility.ConvertBoolean(Getflv.Done);
            }
        }
        

        public bool IsEconomyMode
        {
            get
            {
                ThrowHasNotGetflv();

                if(!this._isEconomyMode.HasValue) {
                    object outIsEconomyMode;
                    var converted = Mediation.ConvertValue(out outIsEconomyMode, typeof(bool), NicoNicoVideoMediationKey.inputEconomyMode, Getflv.MovieServerUrl, typeof(string), ServiceType.NicoNicoVideo);
                    if(!converted) {
                        throw new Exception();
                    }
                    this._isEconomyMode = (bool)outIsEconomyMode;
                }

                return this._isEconomyMode.Value;
            }
        }

        public long VideoSize
        {
            get
            {
                ThrowNotGetthumbinfoSource();
                return IsEconomyMode ? SizeLow : SizeHigh;
            }
        }

        public Uri MovieServerUrl
        {
            get
            {
                ThrowHasNotGetflv();
                return new Uri(Getflv.MovieServerUrl);
            }
        }
        public Uri MessageServerUrl
        {
            get
            {
                ThrowHasNotGetflv();
                return new Uri(Getflv.MessageServerUrl);
            }
        }
        public Uri MessageServerSubUrl
        {
            get
            {
                ThrowHasNotGetflv();
                return new Uri(Getflv.MessageServerSubUrl);
            }
        }

        #endregion

        #endregion

        public ImageSource ThumbnailImage
        {
            get
            {
                switch(VideoThumbnailLoad) {
                    case NicoNicoVideoVideoThumbnailLoad.None:
                        return null;

                    case NicoNicoVideoVideoThumbnailLoad.ImageChecking:
                        return null;

                    case NicoNicoVideoVideoThumbnailLoad.ImageLoadingFromWeb:
                    case NicoNicoVideoVideoThumbnailLoad.ImageLoadingFromStorage:
                        return null;

                    case NicoNicoVideoVideoThumbnailLoad.Completed:
                        return this._thumbnailImage;

                    case NicoNicoVideoVideoThumbnailLoad.Failure:
                        return null;

                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public bool HasGetflv => Getflv != null;

        #endregion

        #region function

        void ThrowHasNotGetflv()
        {
            if(!HasGetflv) {
                throw new InvalidOperationException(nameof(Getflv));
            }
        }
        void ThrowNotGetthumbinfoSource()
        {
            if(VideoInformationSource != NicoNicoVideoVideoInformationSource.Getthumbinfo) {
                throw new InvalidOperationException($"{nameof(VideoInformationSource)}: {VideoInformationSource}");
            }
        }

        async Task LoadImageAsync_Impl()
        {
            VideoThumbnailLoad = NicoNicoVideoVideoThumbnailLoad.ImageChecking;
            if(Cached) {
                VideoThumbnailLoad = NicoNicoVideoVideoThumbnailLoad.ImageLoadingFromStorage;
            }
            var uri = ThumbnailUri;

            var binary = await RestrictUtility.Block(async () => {
                using(var userAgent = new HttpUserAgentHost()) {
                    try {
                        return await userAgent.Client.GetByteArrayAsync(uri);
                    } catch(HttpRequestException ex) {
                        Debug.WriteLine(ex);
                        return null;
                    }
                }
            });
            if(binary != null) {
                using(var stream = new MemoryStream(binary)) {
                    var image = BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                    FreezableUtility.SafeFreeze(image);
                    this._thumbnailImage = image;
                    VideoThumbnailLoad = NicoNicoVideoVideoThumbnailLoad.Completed;
                }
            } else {
                VideoThumbnailLoad = NicoNicoVideoVideoThumbnailLoad.Failure;
            }
            await Task.CompletedTask;
        }

        public async Task LoadImageAsync()
        {
            await LoadImageAsync_Impl();
        }

        public void SetGetflvModel(RawNicoNicoVideoGetflvModel getFlvModel)
        {
            Getflv = getFlvModel;
        }

        public void SetThumbModel(RawNicoNicoVideoThumbModel thumbModel)
        {
            Thumb = thumbModel;
            VideoInformationSource = NicoNicoVideoVideoInformationSource.Getthumbinfo;
        }


        #endregion
    }
}
