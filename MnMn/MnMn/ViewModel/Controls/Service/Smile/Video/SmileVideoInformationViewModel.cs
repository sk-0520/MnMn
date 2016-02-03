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
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw.Feed.RankingRss2;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video
{
    public class SmileVideoInformationViewModel: ViewModelBase
    {
        #region define

        public static int NoOrderd => -1;

        #endregion

        #region variable

        SmileVideoVideoThumbnailLoad _videoThumbnailLoad;
        ImageSource _thumbnailImage;

        bool? _isEconomyMode;

        #endregion

        SmileVideoInformationViewModel(Mediation mediation, int number)
        {
            Mediation = mediation;
            Number = number;
            VideoThumbnailLoad = SmileVideoVideoThumbnailLoad.None;
        }

        public SmileVideoInformationViewModel(Mediation mediation, RawSmileVideoThumbModel thumb, int number)
            : this(mediation, number)
        {
            Thumb = thumb;

            VideoInformationSource = SmileVideoVideoInformationSource.Getthumbinfo;
        }

        public SmileVideoInformationViewModel(Mediation mediation, FeedSmileVideoRankingItemModel ranking, int number)
            : this(mediation, number)
        {
            Ranking = ranking;
            RankingDetail = SmileVideoRankingUtility.ConvertRawDescription(Ranking.Description);
            RankingDetail.VideoId = SmileVideoRankingUtility.GetVideoId(Ranking);
            RankingDetail.Title = SmileVideoRankingUtility.GetTitle(Ranking.Title);

            VideoInformationSource = SmileVideoVideoInformationSource.Ranking;
        }

        #region property

        Mediation Mediation { get; set; }

        RawSmileVideoThumbModel Thumb { get; set; }
        FeedSmileVideoRankingItemModel Ranking { get; set; }

        RawSmileVideoRankingDetailModel RankingDetail { get; set; }

        RawSmileVideoGetflvModel Getflv { get; set; }

        public int Number { get; private set; }

        public SmileVideoVideoInformationSource VideoInformationSource { get; private set; }

        public SmileVideoVideoThumbnailLoad VideoThumbnailLoad
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
                    case SmileVideoVideoInformationSource.Getthumbinfo:
                        return Thumb.VideoId;

                    case SmileVideoVideoInformationSource.Ranking:
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
                    case SmileVideoVideoInformationSource.Getthumbinfo:
                        return Thumb.Title;

                    case SmileVideoVideoInformationSource.Ranking:
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
                    case SmileVideoVideoInformationSource.Getthumbinfo:
                        return RawValueUtility.ConvertUri(Thumb.ThumbnailUrl);

                    case SmileVideoVideoInformationSource.Ranking:
                        return RawValueUtility.ConvertUri(RankingDetail.ThumbnailUrl);

                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public DateTime FirstRetrieve { get {
                switch(VideoInformationSource) {
                    case SmileVideoVideoInformationSource.Getthumbinfo:
                        return RawValueUtility.ConvertDateTime(Thumb.FirstRetrieve);

                    case SmileVideoVideoInformationSource.Ranking:
                        return RawValueUtility.ConvertDateTime(RankingDetail.FirstRetrieve);

                    default:
                        throw new NotImplementedException();
                }
            } }

        public TimeSpan Length { get { return SmileVideoGetthumbinfoUtility.ConvertTimeSpan(Thumb.Length); } }
        public SmileVideoMovieType MovieType { get { return SmileVideoGetthumbinfoUtility.ConvertMovieType(Thumb.MovieType); } }
        public long SizeHigh { get { return RawValueUtility.ConvertLong(Thumb.SizeHigh); } }
        public long SizeLow{ get { return RawValueUtility.ConvertLong(Thumb.SizeLow); } }
        public int ViewCounter { get { return RawValueUtility.ConvertInteger(Thumb.ViewCounter); } }
        public int CommentNum { get { return RawValueUtility.ConvertInteger(Thumb.CommentNum); } }
        public int MylistCounter { get { return RawValueUtility.ConvertInteger(Thumb.MylistCounter); } }
        public Uri WatchUrl
        {
            get {
                switch(VideoInformationSource) {
                    case SmileVideoVideoInformationSource.Getthumbinfo:
                        return RawValueUtility.ConvertUri(Thumb.WatchUrl);

                    case SmileVideoVideoInformationSource.Ranking:
                        return RawValueUtility.ConvertUri(Ranking.Link);

                    default:
                        throw new NotImplementedException();
                }
            }
        }
        public SmileVideoThumbType ThumbType { get { return SmileVideoGetthumbinfoUtility.ConvertThumbType(Thumb.ThumbType); } }
        public bool Embeddable { get { return SmileVideoGetthumbinfoUtility.IsEmbeddable(Thumb.Embeddable); } }
        public bool LivePlay { get { return SmileVideoGetthumbinfoUtility.IsLivePlay(Thumb.NoLivePlay); } }
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
                    var converted = Mediation.ConvertValue(out outIsEconomyMode, typeof(bool), SmileVideoMediationKey.inputEconomyMode, Getflv.MovieServerUrl, typeof(string), ServiceType.SmileVideo);
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
                    case SmileVideoVideoThumbnailLoad.None:
                        return null;

                    case SmileVideoVideoThumbnailLoad.ImageChecking:
                        return null;

                    case SmileVideoVideoThumbnailLoad.ImageLoadingFromWeb:
                    case SmileVideoVideoThumbnailLoad.ImageLoadingFromStorage:
                        return null;

                    case SmileVideoVideoThumbnailLoad.Completed:
                        return this._thumbnailImage;

                    case SmileVideoVideoThumbnailLoad.Failure:
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
            if(VideoInformationSource != SmileVideoVideoInformationSource.Getthumbinfo) {
                throw new InvalidOperationException($"{nameof(VideoInformationSource)}: {VideoInformationSource}");
            }
        }

        public string GetVideoFileName(bool isEconomyMode)
        {
            ThrowNotGetthumbinfoSource();
            ThrowHasNotGetflv();

            var ext = SmileVideoGetthumbinfoUtility.GetFileExtension(MovieType);
            var eco = isEconomyMode ? "-" + SmileVideoGetthumbinfoUtility.EconomyFileSuffix : string.Empty;

            return $"{VideoId}{eco}.{ext}";
        }

        async Task LoadImageAsync_Impl()
        {
            VideoThumbnailLoad = SmileVideoVideoThumbnailLoad.ImageChecking;
            if(Cached) {
                VideoThumbnailLoad = SmileVideoVideoThumbnailLoad.ImageLoadingFromStorage;
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
                    VideoThumbnailLoad = SmileVideoVideoThumbnailLoad.Completed;
                }
            } else {
                VideoThumbnailLoad = SmileVideoVideoThumbnailLoad.Failure;
            }
            await Task.CompletedTask;
        }

        public async Task LoadImageAsync()
        {
            await LoadImageAsync_Impl();
        }

        public void SetGetflvModel(RawSmileVideoGetflvModel getFlvModel)
        {
            Getflv = getFlvModel;
        }

        public void SetThumbModel(RawSmileVideoThumbModel thumbModel)
        {
            Thumb = thumbModel;
            VideoInformationSource = SmileVideoVideoInformationSource.Getthumbinfo;
        }


        #endregion
    }
}
