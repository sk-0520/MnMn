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
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video.Api;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Raw;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw.Feed.RankingRss2;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.View.Controls.Service.Smile.Video;
using HtmlAgilityPack;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video
{
    public class SmileVideoInformationViewModel: ViewModelBase
    {
        #region define

        public static int NoOrderd => -1;

        #endregion

        #region variable

        LoadState _thumbnailLoadState;
        LoadState _informationLoadState;
        LoadState _pageHtmlLoadState;

        BitmapSource _thumbnailImage;

        bool? _isEconomyMode;

        CollectionModel<SmileVideoTagViewModel> _tagList;

        #endregion

        SmileVideoInformationViewModel(Mediation mediation, int number)
        {
            Mediation = mediation;
            Number = number;
            ThumbnailLoadState = LoadState.None;
        }

        public SmileVideoInformationViewModel(Mediation mediation, RawSmileVideoThumbModel thumb, int number)
            : this(mediation, number)
        {
            Thumb = thumb;

            VideoInformationSource = SmileVideoVideoInformationSource.Getthumbinfo;
            Initialize();
        }

        public SmileVideoInformationViewModel(Mediation mediation, RawSmileContentsSearchItemModel search, int number)
            : this(mediation, number)
        {
            Search = search;

            VideoInformationSource = SmileVideoVideoInformationSource.Search;
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

        Mediation Mediation { get; }

        SmileVideoSettingModel Setting { get; set; }

        SmileVideoIndividualVideoSettingModel IndividualVideoSetting { get; set; } = new SmileVideoIndividualVideoSettingModel();
        FileInfo IndividualVideoSettingFile { get; set; }

        RawSmileVideoThumbModel Thumb { get; set; }
        FeedSmileVideoRankingItemModel Ranking { get; set; }
        RawSmileContentsSearchItemModel Search { get; }

        RawSmileVideoRankingDetailModel RankingDetail { get; set; }

        RawSmileVideoGetflvModel Getflv { get; set; }

        DirectoryInfo CacheDirectory { get; set; }

        public int Number { get; private set; }


        public string PageHtml { get; private set; }

        public string PageDescription { get; private set; }

        public SmileVideoVideoInformationSource VideoInformationSource { get; private set; }

        public LoadState ThumbnailLoadState
        {
            get { return this._thumbnailLoadState; }
            set
            {
                if(SetVariableValue(ref this._thumbnailLoadState, value)) {
                    CallOnPropertyChange(nameof(ThumbnailImage));
                }
            }
        }

        public LoadState InformationLoadState
        {
            get { return this._informationLoadState; }
            set
            {
                if(SetVariableValue(ref this._informationLoadState, value)) {
                    CallOnPropertyChange(nameof(ThumbnailImage));
                }
            }
        }

        public LoadState PageHtmlLoadState
        {
            get { return this._pageHtmlLoadState; }
            set { SetVariableValue(ref this._pageHtmlLoadState, value); }
        }

        

        public bool HasLength
        {
            get
            {
                return VideoInformationSource != SmileVideoVideoInformationSource.Search;
            }
        }

        public bool IsLoadVideoInformation
        {
            get { return Setting.LoadVideoTime; }
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

                    case SmileVideoVideoInformationSource.Search:
                        return Search.ContentId;

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

                    case SmileVideoVideoInformationSource.Search:
                        return Search.Title;

                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public string Description
        {
            get
            {
                switch(VideoInformationSource) {
                    case SmileVideoVideoInformationSource.Getthumbinfo:
                        return Thumb.Description;

                    case SmileVideoVideoInformationSource.Ranking:
                        return RankingDetail.Description ?? Ranking.Description;

                    case SmileVideoVideoInformationSource.Search:
                        return Search.Description;

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

                    case SmileVideoVideoInformationSource.Search:
                        return RawValueUtility.ConvertUri(Search.ThumbnailUrl);

                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public DateTime FirstRetrieve
        {
            get
            {
                switch(VideoInformationSource) {
                    case SmileVideoVideoInformationSource.Getthumbinfo:
                        return RawValueUtility.ConvertDateTime(Thumb.FirstRetrieve);

                    case SmileVideoVideoInformationSource.Ranking:
                        return RawValueUtility.ConvertDateTime(RankingDetail.FirstRetrieve);

                    case SmileVideoVideoInformationSource.Search:
                        return RawValueUtility.ConvertDateTime(Search.StartTime);

                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public TimeSpan Length
        {
            get
            {
                switch(VideoInformationSource) {
                    case SmileVideoVideoInformationSource.Getthumbinfo:
                        return SmileVideoGetthumbinfoUtility.ConvertTimeSpan(Thumb.Length);

                    case SmileVideoVideoInformationSource.Ranking:
                        return SmileVideoGetthumbinfoUtility.ConvertTimeSpan(RankingDetail.Length);

                    case SmileVideoVideoInformationSource.Search:
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public SmileVideoMovieType MovieType { get { return SmileVideoGetthumbinfoUtility.ConvertMovieType(Thumb.MovieType); } }
        public long SizeHigh { get { return RawValueUtility.ConvertLong(Thumb.SizeHigh); } }
        public long SizeLow{ get { return RawValueUtility.ConvertLong(Thumb.SizeLow); } }
        public int ViewCounter
        {
            get
            {
                switch(VideoInformationSource) {
                    case SmileVideoVideoInformationSource.Getthumbinfo:
                        return RawValueUtility.ConvertInteger(Thumb.ViewCounter);

                    case SmileVideoVideoInformationSource.Ranking:
                        return SmileVideoRankingUtility.ConvertInteger(RankingDetail.ViewCounter);

                    case SmileVideoVideoInformationSource.Search:
                        return RawValueUtility.ConvertInteger(Search.ViewCounter);

                    default:
                        throw new NotImplementedException();
                }
            }
        }
        public int CommentCount
        {
            get
            {
                switch(VideoInformationSource) {
                    case SmileVideoVideoInformationSource.Getthumbinfo:
                        return RawValueUtility.ConvertInteger(Thumb.CommentNum);

                    case SmileVideoVideoInformationSource.Ranking:
                        return SmileVideoRankingUtility.ConvertInteger(RankingDetail.CommentNum);

                    case SmileVideoVideoInformationSource.Search:
                        return RawValueUtility.ConvertInteger(Search.CommentCounter);

                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public int MylistCounter
        {
            get
            {
                switch(VideoInformationSource) {
                    case SmileVideoVideoInformationSource.Getthumbinfo:
                        return RawValueUtility.ConvertInteger(Thumb.MylistCounter);

                    case SmileVideoVideoInformationSource.Ranking:
                        return SmileVideoRankingUtility.ConvertInteger(RankingDetail.MylistCounter);

                    case SmileVideoVideoInformationSource.Search:
                        return RawValueUtility.ConvertInteger(Search.MylistCounter);

                    default:
                        throw new NotImplementedException();
                }
            }
        }
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

        public CollectionModel<SmileVideoTagViewModel> TagList
        {
            get
            {
                if(this._tagList == null) {
                    switch(VideoInformationSource) {
                        case SmileVideoVideoInformationSource.Getthumbinfo: {
                                // TODO: 言語未考慮
                                this._tagList = new CollectionModel<SmileVideoTagViewModel>();
                                var tagItems = Thumb.Tags.FirstOrDefault();
                                if(tagItems != null) {
                                    var list = tagItems.Tags.Select(t => new SmileVideoTagViewModel(Mediation, t));
                                    this._tagList.InitializeRange(list);
                                }
                            }
                            break;

                        default:
                            throw new NotImplementedException();
                    }
                }

                return this._tagList;
            }
        }

        #region Getflv

        public bool HasError
        {
            get
            {
                ThrowHasNotGetflv();

                return !string.IsNullOrWhiteSpace(Getflv.Error) || string.IsNullOrWhiteSpace(Getflv.MovieServerUrl);
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
                switch(ThumbnailLoadState) {
                    case LoadState.None:
                        return null;

                    case LoadState.Preparation:
                        return null;

                    case LoadState.Loading:
                        return null;

                    case LoadState.Loaded:
                        return this._thumbnailImage;

                    case LoadState.Failure:
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

        #region command

        public ICommand OpenVideoCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        OpenPlayerAsync();
                    }
                );
            }
        }

        #endregion

        #region function

        static string GetCacheFileName(string videoId, string roll, string extension)
        {
            return FileNameUtility.CreateFileName(videoId, roll, extension);
        }

        string GetCacheFileName(string roll, string extension)
        {
            CheckUtility.EnforceNotNullAndNotEmpty(roll);
            return FileNameUtility.CreateFileName(VideoId, roll, extension);
        }
        string GetCacheFileName(string extension)
        {
            return FileNameUtility.CreateFileName(VideoId, extension);
        }

        static async Task<RawSmileVideoThumbResponseModel> LoadGetthumbinfoAsync(Mediation mediation, string videoId, CacheSpan thumbCacheSpan)
        {
            var response = mediation.Request(new RequestModel(RequestKind.CacheDirectory, ServiceType.SmileVideo));
            var dirInfo = (DirectoryInfo)response.Result;
            var cachedDirPath = Path.Combine(dirInfo.FullName, Constants.SmileVideoCacheVideosDirectoryName, videoId);
            if(!Directory.Exists(cachedDirPath)) {
                Directory.CreateDirectory(cachedDirPath);
            }

            RawSmileVideoThumbResponseModel rawGetthumbinfo = null;
            var cachedThumbFilePath = Path.Combine(cachedDirPath, GetCacheFileName(videoId, "thumb", "xml"));
            if(File.Exists(cachedThumbFilePath)) {
                var fileInfo = new FileInfo(cachedThumbFilePath);
                if(thumbCacheSpan.IsCacheTime(fileInfo.LastWriteTime) && Constants.MinimumXmlFileSize <= fileInfo.Length) {
                    using(var stream = new FileStream(cachedThumbFilePath, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                        rawGetthumbinfo = Getthumbinfo.Load(stream);
                    }
                }
            }
            if(rawGetthumbinfo == null || !SmileVideoGetthumbinfoUtility.IsSuccessResponse(rawGetthumbinfo)) {
                var getthumbinfo = new Getthumbinfo(mediation);
                rawGetthumbinfo = await getthumbinfo.GetAsync(videoId);
            }

            // キャッシュ構築
            try {
                SerializeUtility.SaveXmlSerializeToFile(cachedThumbFilePath, rawGetthumbinfo);
            } catch(FileNotFoundException) {
                // BUGS: いかんのう
            }

            return rawGetthumbinfo;
        }

        public static async Task<SmileVideoInformationViewModel> CreateFromVideoIdAsync(Mediation mediation, string videoId, CacheSpan thumbCacheSpan)
        {
            var rawGetthumbinfo = await LoadGetthumbinfoAsync(mediation, videoId, thumbCacheSpan);
            return new SmileVideoInformationViewModel(mediation, rawGetthumbinfo.Thumb, NoOrderd);
        }

        void Initialize()
        {
            var response = Mediation.Request(new RequestModel(RequestKind.CacheDirectory, ServiceType.SmileVideo));
            var dirInfo = (DirectoryInfo)response.Result;
            var cachedDirPath = Path.Combine(dirInfo.FullName, Constants.SmileVideoCacheVideosDirectoryName, VideoId);
            if(Directory.Exists(cachedDirPath)) {
                CacheDirectory = new DirectoryInfo(cachedDirPath);
            } else {
                CacheDirectory = Directory.CreateDirectory(cachedDirPath);
            }

            var resSetting = Mediation.Request(new RequestModel(RequestKind.Setting, ServiceType.SmileVideo));
            Setting = (SmileVideoSettingModel)resSetting.Result;

            IndividualVideoSettingFile = new FileInfo(Path.Combine(CacheDirectory.FullName, GetCacheFileName("setting", "json")));
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

        ImageSource GetImage(Stream stream)
        {
            var image = BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
            FreezableUtility.SafeFreeze(image);
            return this._thumbnailImage = image;
        }

        async Task LoadThumbnaiImageAsync_Impl()
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
                    GetImage(stream);
                    ThumbnailLoadState = LoadState.Loaded;
                }
            } else {
                ThumbnailLoadState = LoadState.Failure;
            }
            await Task.CompletedTask;
        }

        public async Task LoadThumbnaiImageAsync(CacheSpan cacheSpan)
        {
            ThumbnailLoadState = LoadState.Preparation;

            var cachedFilePath = Path.Combine(CacheDirectory.FullName, GetCacheFileName("png"));
            if(File.Exists(cachedFilePath)) {
                var fileInfo = new FileInfo(cachedFilePath);
                if(cacheSpan.IsCacheTime(fileInfo.LastWriteTime) && Constants.MinimumPngFileSize <= fileInfo.Length) {

                    ThumbnailLoadState = LoadState.Loading;

                    using(var stream = new FileStream(cachedFilePath, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                        GetImage(stream);
                        ThumbnailLoadState = LoadState.Loaded;
                        return;
                    }
                }
            }

            await LoadThumbnaiImageAsync_Impl();

            if(ThumbnailLoadState == LoadState.Loaded) {
                // キャッシュ構築
                // TODO: 別スレッドで所有うんぬん
                var frame = BitmapFrame.Create(this._thumbnailImage);
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(frame);
                FileUtility.MakeFileParentDirectory(cachedFilePath);
                using(var stream = new FileStream(cachedFilePath, FileMode.Create, FileAccess.Write, FileShare.Read)) {
                    encoder.Save(stream);
                }
            }
        }

        public async Task LoadInformationAsync(CacheSpan cacheSpan)
        {
            InformationLoadState = LoadState.Preparation;

            InformationLoadState = LoadState.Loading;

            var rawGetthumbinfo = await LoadGetthumbinfoAsync(Mediation, VideoId, cacheSpan);
            //SetThumbModel(rawGetthumbinfo.Thumb);
            Thumb = rawGetthumbinfo.Thumb;
            InformationLoadState = LoadState.Loaded;
            VideoInformationSource = SmileVideoVideoInformationSource.Getthumbinfo;
            var propertyNames = new[] {
                nameof(Length),
                nameof(HasLength),
            };
            CallOnPropertyChange(propertyNames);
        }

        public Task SetPageHtmlAsync(string html, bool isSave)
        {
            PageHtmlLoadState = LoadState.Loading;

            var document = new HtmlDocument();
            return Task.Run(() => {
                PageHtml = html;
                document.LoadHtml(html);
                //document.DocumentNode.SelectSingleNode("@")
                var description = document.DocumentNode.SelectSingleNode("//*[@class='videoDescription']");
                PageDescription = description.InnerHtml;

            }).ContinueWith(task => {
                PageHtmlLoadState = LoadState.Loaded;
            }).ContinueWith(task => {
                if(isSave) {
                    var filePath = Path.Combine(CacheDirectory.FullName, GetCacheFileName("page", "html"));
                    File.WriteAllText(filePath, PageHtml);
                }
            });
        }

        /// <summary>
        /// <para>通信はしない！</para>
        /// </summary>
        public Task LoadLocalPageHtmlAsync()
        {
            PageHtmlLoadState = LoadState.Preparation;

            var file = new FileInfo(Path.Combine(CacheDirectory.FullName, GetCacheFileName("page", "html")));
            if(file.Exists && Constants.MinimumHtmlFileSize <= file.Length) {
                using(var stream = file.OpenText()) {
                    var html = stream.ReadToEnd();
                    return SetPageHtmlAsync(html, false);
                }
            } else {
                PageHtmlLoadState = LoadState.None;
                return Task.CompletedTask;
            }
        }

        public void SetGetflvModel(RawSmileVideoGetflvModel getFlvModel)
        {
            Getflv = getFlvModel;
        }

        //public void SetThumbModel(RawSmileVideoThumbModel thumbModel)
        //{
        //    Thumb = thumbModel;
        //    VideoInformationSource = SmileVideoVideoInformationSource.Getthumbinfo;
        //}

        public bool SaveSetting()
        {
            if(!IsChanged) {
                return false;
            }

            SerializeUtility.SaveJsonDataToFile(IndividualVideoSettingFile.FullName, IndividualVideoSetting);
            ResetChangeFlag();
            return true;
        }

        public async void OpenPlayerAsync()
        {
            var vm = new SmileVideoPlayerViewModel(Mediation);
            var window = new SmileVideoPlayerWindow() {
                DataContext = vm,
            };
            vm.SetView(window);
            window.Show();

            await vm.LoadAsync(this, Constants.ServiceSmileVideoThumbCacheSpan, Constants.ServiceSmileVideoImageCacheSpan);
        }

        #endregion
    }
}
