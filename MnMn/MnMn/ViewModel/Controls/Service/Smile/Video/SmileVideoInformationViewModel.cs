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
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw.Feed.RankingRss2;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video
{
    public class SmileVideoInformationViewModel: ViewModelBase
    {
        #region define

        public static int NoOrderd => -1;

        #endregion

        #region variable

        SmileVideoVideoThumbnailLoad _videoThumbnailLoad;
        BitmapSource _thumbnailImage;

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
            Initialize();
        }

        public SmileVideoInformationViewModel(Mediation mediation, FeedSmileVideoRankingItemModel ranking, int number)
            : this(mediation, number)
        {
            Ranking = ranking;
            RankingDetail = SmileVideoRankingUtility.ConvertRawDescription(Ranking.Description);
            RankingDetail.VideoId = SmileVideoRankingUtility.GetVideoId(Ranking);
            RankingDetail.Title = SmileVideoRankingUtility.GetTitle(Ranking.Title);

            VideoInformationSource = SmileVideoVideoInformationSource.Ranking;
            Initialize();
        }

        #region property

        Mediation Mediation { get; set; }

        SmileVideoIndividualVideoSettingModel IndividualVideoSetting { get; set; } = new SmileVideoIndividualVideoSettingModel();
        FileInfo IndividualVideoSettingFile { get; set; }

        RawSmileVideoThumbModel Thumb { get; set; }
        FeedSmileVideoRankingItemModel Ranking { get; set; }

        RawSmileVideoRankingDetailModel RankingDetail { get; set; }

        RawSmileVideoGetflvModel Getflv { get; set; }

        DirectoryInfo CacheDirectory { get; set; }

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

        public bool HasError
        {
            get
            {
                ThrowHasNotGetflv();

                return !string.IsNullOrWhiteSpace(Getflv.Error);
            }
        }

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

        public string ThreadId
        {
            get
            {
                ThrowHasNotGetflv();
                return Getflv.ThreadId;
            }
        }

        public string UserId
        {
            get
            {
                ThrowHasNotGetflv();
                return Getflv.UserId;
            }
        }

        public string OptionalThreadId
        {
            get
            {
                ThrowHasNotGetflv();
                return Getflv.OptionalThreadId;
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

        public bool LoadedNormalVideo
        {
            get { return IndividualVideoSetting.LoadedNormal; }
            set { SetPropertyValue(IndividualVideoSetting, value, nameof(IndividualVideoSetting.LoadedNormal)); }
        }
        public bool LoadedEconomyVideo
        {
            get { return IndividualVideoSetting.LoadedEconomyMode; }
            set { SetPropertyValue(IndividualVideoSetting, value, nameof(IndividualVideoSetting.LoadedEconomyMode)); }
        }

        #endregion

        #region function

        void Initialize()
        {
            var response = Mediation.Request(new RequestModel(RequestKind.CacheDirectory, ServiceType.SmileVideo));
            var dirInfo = (DirectoryInfo)response.Result;
            var cachedDirPath = Path.Combine(dirInfo.FullName, VideoId);
            if(Directory.Exists(cachedDirPath)) {
                CacheDirectory = new DirectoryInfo(cachedDirPath);
            } else {
                CacheDirectory = Directory.CreateDirectory(cachedDirPath);
            }

            IndividualVideoSettingFile = new FileInfo(Path.Combine(CacheDirectory.FullName, Constants.SmileVideoIndividualVideoSettingName));
            if(IndividualVideoSettingFile.Exists && IndividualVideoSettingFile.Length <= Constants.MinimumJsonFileSize) {
                IndividualVideoSetting = SerializeUtility.LoadJsonDataFromFile<SmileVideoIndividualVideoSettingModel>(IndividualVideoSettingFile.FullName);
            } else {
                IndividualVideoSetting = new SmileVideoIndividualVideoSettingModel();
            }
        }

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

            return SmileVideoGetthumbinfoUtility.GetFileName(VideoId, MovieType, isEconomyMode);
        }

        ImageSource Load(Stream stream)
        {
            var image = BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
            FreezableUtility.SafeFreeze(image);
            return this._thumbnailImage = image;
        }

        async Task LoadImageAsync_Impl()
        {
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
                    Load(stream);
                    VideoThumbnailLoad = SmileVideoVideoThumbnailLoad.Completed;
                }
            } else {
                VideoThumbnailLoad = SmileVideoVideoThumbnailLoad.Failure;
            }
            await Task.CompletedTask;
        }

        public async Task LoadImageAsync(CacheSpan cacheSpan)
        {
            VideoThumbnailLoad = SmileVideoVideoThumbnailLoad.ImageChecking;

            var cachedFilePath = Path.Combine(CacheDirectory.FullName, VideoId + ".png");
            if(File.Exists(cachedFilePath)) {
                var fileInfo = new FileInfo(cachedFilePath);
                if(cacheSpan.IsCacheTime(fileInfo.LastWriteTime) && Constants.MinimumPngFileSize <= fileInfo.Length) {

                    VideoThumbnailLoad = SmileVideoVideoThumbnailLoad.ImageLoadingFromStorage;

                    using(var stream = new FileStream(cachedFilePath, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                        Load(stream);
                        VideoThumbnailLoad = SmileVideoVideoThumbnailLoad.Completed;
                        return;
                    }
                }
            }

            await LoadImageAsync_Impl();

            if(VideoThumbnailLoad == SmileVideoVideoThumbnailLoad.Completed) {
                // キャッシュ構築
                var frame = BitmapFrame.Create(this._thumbnailImage);
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(frame);
                FileUtility.MakeFileParentDirectory(cachedFilePath);
                using(var stream = new FileStream(cachedFilePath, FileMode.Create, FileAccess.Write, FileShare.Read)) {
                    encoder.Save(stream);
                }
            }
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

        public bool SaveSetting()
        {
            if(!IsChanged) {
                return false;
            }

            SerializeUtility.SaveJsonDataToFile(IndividualVideoSettingFile.FullName, IndividualVideoSetting);
            ResetChangeFlag();
            return true;
        }

        #endregion
    }
}
