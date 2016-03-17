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
using System.Threading;
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
using ContentTypeTextNet.MnMn.MnMn.IF.Control;
using ContentTypeTextNet.MnMn.MnMn.IF.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Api;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video.Api;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video.Api.V1;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Raw;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw.Feed;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.View.Controls.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Player;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Service.Smile;
using HtmlAgilityPack;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video
{
    public class SmileVideoInformationViewModel: ViewModelBase, ICheckable
    {
        #region define

        public static int NoOrderd => -1;

        #endregion

        #region variable

        LoadState _thumbnailLoadState;
        LoadState _informationLoadState;
        LoadState _pageHtmlLoadState;
        LoadState _relationVideoLoadState;

        BitmapSource _thumbnailImage;

        bool? _isEconomyMode;

        CollectionModel<SmileVideoTagViewModel> _tagList;

        bool? _isChecked = false;

        #endregion

        SmileVideoInformationViewModel(Mediation mediation, int number, SmileVideoInformationFlags informationFlags)
        {
            Mediation = mediation;
            Number = number;
            ThumbnailLoadState = LoadState.None;
            InformationFlags = informationFlags;
        }

        public SmileVideoInformationViewModel(Mediation mediation, RawSmileVideoThumbModel thumb, int number)
            : this(mediation, number, SmileVideoInformationFlags.All)
        {
            Thumb = thumb;

            VideoInformationSource = SmileVideoVideoInformationSource.Getthumbinfo;
            InformationLoadState = LoadState.Loaded;

            Initialize();
        }

        public SmileVideoInformationViewModel(Mediation mediation, RawSmileContentsSearchItemModel search, int number)
            : this(mediation, number, SmileVideoInformationFlags.CommentCounter | SmileVideoInformationFlags.MylistCounter| SmileVideoInformationFlags.ViewCounter)
        {
            Search = search;

            VideoInformationSource = SmileVideoVideoInformationSource.Search;
            Initialize();
        }

        public SmileVideoInformationViewModel(Mediation mediation, FeedSmileVideoItemModel feed, int number, SmileVideoInformationFlags informationFlags)
            : this(mediation, number, informationFlags)
        {
            Feed = feed;
            FeedDetail = SmileVideoFeedUtility.ConvertRawDescription(Feed.Description);
            FeedDetail.VideoId = SmileVideoFeedUtility.GetVideoId(Feed);
            FeedDetail.Title = SmileVideoFeedUtility.GetTitle(Feed.Title);

            VideoInformationSource = SmileVideoVideoInformationSource.Feed;
            Initialize();
        }

        #region property

        Mediation Mediation { get; }
        SmileVideoSettingModel Setting { get; set; }
        SmileVideoInformationFlags InformationFlags { get; }
        public bool NeedLoadInformationFlag { get { return InformationFlags != SmileVideoInformationFlags.All; } }

        internal SmileVideoIndividualVideoSettingModel IndividualVideoSetting { get; private set; } = new SmileVideoIndividualVideoSettingModel();
        FileInfo IndividualVideoSettingFile { get; set; }

        RawSmileVideoThumbModel Thumb { get; set; }
        FeedSmileVideoItemModel Feed { get; set; }
        RawSmileContentsSearchItemModel Search { get; }

        RawSmileVideoFeedDetailModel FeedDetail { get; set; }

        RawSmileVideoGetflvModel Getflv { get; set; }

        public DirectoryInfo CacheDirectory { get; private set; }

        public int Number { get; private set; }


        public string PageHtml { get; private set; }

        public string DescriptionHtml { get; private set; }
        public string PageVideoToken { get; private set; }

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

        public LoadState RelationVideoLoadState
        {
            get { return this._relationVideoLoadState; }
            set { SetVariableValue(ref this._relationVideoLoadState, value); }
        }

        public bool HasLength { get { return InformationFlags.HasFlag(SmileVideoInformationFlags.Length); } }
        public bool HasViewConter { get { return InformationFlags.HasFlag(SmileVideoInformationFlags.ViewCounter); } }
        public bool HasCommentCounter { get { return InformationFlags.HasFlag(SmileVideoInformationFlags.CommentCounter); } }
        public bool HasMylistCounter { get { return InformationFlags.HasFlag(SmileVideoInformationFlags.MylistCounter); } }
        public bool HasFirstRetrieve { get { return InformationFlags.HasFlag(SmileVideoInformationFlags.FirstRetrieve); } }

        public bool IsLoadVideoInformation { get { return Setting.Search.LoadInformation; } }

        #region 生データから取得

        public string VideoId
        {
            get
            {
                switch(VideoInformationSource) {
                    case SmileVideoVideoInformationSource.Getthumbinfo:
                        return Thumb.VideoId;

                    case SmileVideoVideoInformationSource.Feed:
                        return FeedDetail.VideoId;

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

                    case SmileVideoVideoInformationSource.Feed:
                        return FeedDetail.Title ?? Feed.Title;

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

                    case SmileVideoVideoInformationSource.Feed:
                        return FeedDetail.Description ?? Feed.Description;

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

                    case SmileVideoVideoInformationSource.Feed:
                        return RawValueUtility.ConvertUri(FeedDetail.ThumbnailUrl);

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

                    case SmileVideoVideoInformationSource.Feed:
                        return RawValueUtility.ConvertDateTime(FeedDetail.FirstRetrieve);

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

                    case SmileVideoVideoInformationSource.Feed:
                        return SmileVideoGetthumbinfoUtility.ConvertTimeSpan(FeedDetail.Length);

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

                    case SmileVideoVideoInformationSource.Feed:
                        return RawValueUtility.ConvertInteger(FeedDetail.ViewCounter);

                    case SmileVideoVideoInformationSource.Search:
                        return RawValueUtility.ConvertInteger(Search.ViewCounter);

                    default:
                        throw new NotImplementedException();
                }
            }
        }
        public int CommentCounter
        {
            get
            {
                switch(VideoInformationSource) {
                    case SmileVideoVideoInformationSource.Getthumbinfo:
                        return RawValueUtility.ConvertInteger(Thumb.CommentNum);

                    case SmileVideoVideoInformationSource.Feed:
                        return RawValueUtility.ConvertInteger(FeedDetail.CommentNum);

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

                    case SmileVideoVideoInformationSource.Feed:
                        return RawValueUtility.ConvertInteger(FeedDetail.MylistCounter);

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

                    case SmileVideoVideoInformationSource.Feed:
                        return RawValueUtility.ConvertUri(Feed.Link);

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

        public string UserNickname
        {
            get
            {
                ThrowNotGetthumbinfoSource();
                return Thumb.UserNickname;
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
                        rawGetthumbinfo = Getthumbinfo.ConvertFromRawData(stream);
                    }
                }
            }
            if(rawGetthumbinfo == null || !SmileVideoGetthumbinfoUtility.IsSuccessResponse(rawGetthumbinfo)) {
                var getthumbinfo = new Getthumbinfo(mediation);
                rawGetthumbinfo = await getthumbinfo.LoadAsync(videoId);
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
            if(IndividualVideoSettingFile.Exists && Constants.MinimumJsonFileSize <= IndividualVideoSettingFile.Length) {
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
            Application.Current.Dispatcher.Invoke(new Action(() => {
                var image = BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                FreezableUtility.SafeFreeze(image);
                this._thumbnailImage = image;
            }));
            return this._thumbnailImage;
        }

        Task LoadThumbnaiImageAsync_Impl(string savePath, HttpClient client)
        {
            var uri = ThumbnailUri;

            return RestrictUtility.Block(async () => {
                var maxCount = 3;
                var count = 0;
                do {
                    try {
                        Mediation.Logger.Trace($"img -> {uri}");
                        return await client.GetByteArrayAsync(uri);
                    } catch(HttpRequestException ex) {
                        Mediation.Logger.Error($"error img -> {uri}");
                        Mediation.Logger.Warning(ex);
                        if(count != 0) {
                            var wait = TimeSpan.FromSeconds(1);
                            Thread.Sleep(wait);
                        }
                    }
                } while(count++ < maxCount);
                return null;
            }).ContinueWith(task => {
                var binary = task.Result;

                if(binary != null) {
                    using(var stream = new MemoryStream(binary)) {
                        GetImage(stream);
                        ThumbnailLoadState = LoadState.Loaded;
                    }
                    if(this._thumbnailImage != null && Application.Current != null) {
                        // キャッシュ構築
                        Application.Current.Dispatcher.BeginInvoke(new Action(() => {
                            var frame = BitmapFrame.Create(this._thumbnailImage);
                            var encoder = new PngBitmapEncoder();
                            encoder.Frames.Add(frame);
                            FileUtility.MakeFileParentDirectory(savePath);
                            using(var stream = new FileStream(savePath, FileMode.Create, FileAccess.Write, FileShare.Read)) {
                                encoder.Save(stream);
                            }
                        }));
                    }
                } else {
                    ThumbnailLoadState = LoadState.Failure;
                }
            });
        }

        public Task LoadThumbnaiImageAsync(CacheSpan cacheSpan, HttpClient client)
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
                        return Task.CompletedTask;
                    }
                }
            }

            ThumbnailLoadState = LoadState.Loading;
            return LoadThumbnaiImageAsync_Impl(cachedFilePath, client);
        }

        public Task LoadThumbnaiImageDefaultAsync(CacheSpan cacheSpan)
        {
            var client = new HttpClient();
            return LoadThumbnaiImageAsync(cacheSpan, client).ContinueWith(_ => {
                client.Dispose();
            });
        }

        public Task LoadInformationAsync(CacheSpan cacheSpan, HttpClient client)
        {
            InformationLoadState = LoadState.Preparation;

            InformationLoadState = LoadState.Loading;

            return LoadGetthumbinfoAsync(Mediation, VideoId, cacheSpan).ContinueWith(task => {
                var rawGetthumbinfo = task.Result;
                Thumb = rawGetthumbinfo.Thumb;
                InformationLoadState = LoadState.Loaded;
                VideoInformationSource = SmileVideoVideoInformationSource.Getthumbinfo;
                var propertyNames = new[] {
                    nameof(Length),
                    nameof(HasLength),
                    nameof(InformationLoadState),
                };
                CallOnPropertyChange(propertyNames);
            });
        }

        public Task LoadInformationDefaultAsync(CacheSpan cacheSpan)
        {
            var client = new HttpClient();
            return LoadInformationAsync(cacheSpan, client).ContinueWith(_ => {
                client.Dispose();
            });
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
                DescriptionHtml = description.InnerHtml;

                var json = SmileVideoWatchAPIUtility.ConvertJsonFromWatchPage(html);
                var flashvars = json.SelectToken("flashvars");
                PageVideoToken = flashvars.Value<string>("csrfToken");

            }).ContinueWith(task => {
                PageHtmlLoadState = LoadState.Loaded;
            }).ContinueWith(task => {
                if(isSave) {
                    var filePath = Path.Combine(CacheDirectory.FullName, GetCacheFileName("page", "html"));
                    File.WriteAllText(filePath, PageHtml);
                }
            });
        }

        public Task<IEnumerable<SmileVideoInformationViewModel>> LoadRelationVideosAsync(CacheSpan cacheSpan)
        {
            RelationVideoLoadState = LoadState.Preparation;

            RelationVideoLoadState = LoadState.Loading;
            var getRelation = new Getrelation(Mediation);
            return getRelation.LoadAsync(VideoId, 1, "p", Library.SharedLibrary.Define.OrderBy.Ascending).ContinueWith(task => {
                var relation = task.Result;

                if(!SmileVideoGetrelationUtility.IsSuccessResponse(relation)) {
                    return null;
                }

                var result = new List<SmileVideoInformationViewModel>();

                foreach(var video in relation.Videos.Select((item, index) => new { Item = item, Index = index })) {
                    var item = new FeedSmileVideoItemModel();
                    item.Link = video.Item.Url;
                    item.Title = video.Item.Title;

                    var detailModel = new RawSmileVideoFeedDetailModel() {
                        ViewCounter = video.Item.View,
                        MylistCounter = video.Item.Mylist,
                        CommentNum = video.Item.Comment,
                        ThumbnailUrl = video.Item.Thumbnail,
                        Length = video.Item.Length,
                        FirstRetrieve = RawValueUtility.ConvertUnixTime(video.Item.Time).ToString("s"),
                    };
                    item.Description = SmileVideoFeedUtility.ConvertDescriptionFromFeedDetailModel(detailModel);

                    var videoInformation = new SmileVideoInformationViewModel(Mediation, item, video.Index + 1, SmileVideoInformationFlags.All);
                    result.Add(videoInformation);
                }
                
                return (IEnumerable<SmileVideoInformationViewModel>)result;
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

        public bool SaveSetting(bool force)
        {
            if(!force && !IsChanged) {
                return false;
            }

            SerializeUtility.SaveJsonDataToFile(IndividualVideoSettingFile.FullName, IndividualVideoSetting);
            ResetChangeFlag();
            return true;
        }

        public async void OpenPlayerAsync()
        {
            var vm = new SmileVideoPlayerViewModel(Mediation);
            var task = vm.LoadAsync(this, Constants.ServiceSmileVideoThumbCacheSpan, Constants.ServiceSmileVideoImageCacheSpan);

            Mediation.Request(new ShowViewRequestModel(RequestKind.ShowView, ServiceType.SmileVideo, vm, ShowViewState.Foreground));

            await task;
        }

        #endregion

        #region ICheckable

        public bool? IsChecked
        {
            get { return this._isChecked; }
            set { SetVariableValue(ref this._isChecked, value); }
        }

        #endregion
    }
}
