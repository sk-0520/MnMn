﻿/*
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
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.IF.Control;
using ContentTypeTextNet.MnMn.MnMn.IF.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Api;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video.Api;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video.Api.V1;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Video.Parameter;
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
    public class SmileVideoInformationViewModel: ViewModelBase
    {
        #region variable

        LoadState _thumbnailLoadState;
        LoadState _informationLoadState;
        LoadState _pageHtmlLoadState;
        LoadState _relationVideoLoadState;

        BitmapSource _thumbnailImage;

        bool? _isEconomyMode;

        CollectionModel<SmileVideoTagViewModel> _tagList;

        bool _isPlaying = false;
        bool _isDownloading = false;

        #endregion

        SmileVideoInformationViewModel(Mediation mediation, int number, SmileVideoInformationFlags informationFlags)
        {
            Mediation = mediation;
            ThumbnailLoadState = LoadState.None;
            InformationFlags = informationFlags;
        }

        public SmileVideoInformationViewModel(Mediation mediation, RawSmileVideoThumbModel thumb, int number)
            : this(mediation, number, SmileVideoInformationFlags.All)
        {
            Thumb = thumb;

            InformationSource = SmileVideoInformationSource.Getthumbinfo;
            InformationLoadState = LoadState.Loaded;

            Initialize();
        }

        public SmileVideoInformationViewModel(Mediation mediation, RawSmileContentsSearchItemModel search, int number)
            : this(mediation, number, SmileVideoInformationFlags.CommentCounter | SmileVideoInformationFlags.MylistCounter | SmileVideoInformationFlags.ViewCounter)
        {
            Search = search;

            InformationSource = SmileVideoInformationSource.Search;
            Initialize();
        }

        public SmileVideoInformationViewModel(Mediation mediation, FeedSmileVideoItemModel feed, int number, SmileVideoInformationFlags informationFlags)
            : this(mediation, number, informationFlags)
        {
            Feed = feed;
            FeedDetail = SmileVideoFeedUtility.ConvertRawDescription(Feed.Description);
            FeedDetail.VideoId = SmileVideoFeedUtility.GetVideoId(Feed);
            FeedDetail.Title = SmileVideoFeedUtility.GetTitle(Feed.Title);

            InformationSource = SmileVideoInformationSource.Feed;
            Initialize();
        }

        #region property

        Mediation Mediation { get; }
        SmileVideoSettingModel Setting { get; set; }
        SmileVideoInformationFlags InformationFlags { get; }
        public bool NeedLoadInformationFlag { get { return InformationFlags != SmileVideoInformationFlags.All; } }

        SmileVideoIndividualVideoSettingModel IndividualVideoSetting { get; set; } = new SmileVideoIndividualVideoSettingModel();
        FileInfo IndividualVideoSettingFile { get; set; }

        RawSmileVideoThumbModel Thumb { get; set; }
        FeedSmileVideoItemModel Feed { get; set; }
        RawSmileContentsSearchItemModel Search { get; }

        RawSmileVideoFeedDetailModel FeedDetail { get; set; }

        RawSmileVideoGetflvModel Getflv { get; set; }

        public DirectoryInfo CacheDirectory { get; private set; }


        public string PageHtml { get; private set; }

        public string DescriptionHtml { get; private set; }
        public string PageVideoToken { get; private set; }

        public SmileVideoInformationSource InformationSource { get; private set; }

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

        /// <summary>
        /// 動画をダウンロード中か。
        /// <para>ダウンロード中かどうかを示すためダウンロード済みは偽となる。</para>
        /// </summary>
        public bool IsDownloading
        {
            get { return this._isDownloading; }
            set { SetVariableValue(ref this._isDownloading, value); }
        }
        /// <summary>
        /// 動画をプレイヤーで表示中か。
        /// </summary>
        public bool IsPlaying
        {
            get { return this._isPlaying; }
            set { SetVariableValue(ref this._isPlaying, value); }
        }

        #region 生データから取得

        public string VideoId
        {
            get
            {
                switch(InformationSource) {
                    case SmileVideoInformationSource.Getthumbinfo:
                        return Thumb.VideoId;

                    case SmileVideoInformationSource.Feed:
                        return FeedDetail.VideoId;

                    case SmileVideoInformationSource.Search:
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
                switch(InformationSource) {
                    case SmileVideoInformationSource.Getthumbinfo:
                        return Thumb.Title;

                    case SmileVideoInformationSource.Feed:
                        return FeedDetail.Title ?? Feed.Title;

                    case SmileVideoInformationSource.Search:
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
                switch(InformationSource) {
                    case SmileVideoInformationSource.Getthumbinfo:
                        return Thumb.Description;

                    case SmileVideoInformationSource.Feed:
                        return FeedDetail.Description ?? Feed.Description;

                    case SmileVideoInformationSource.Search:
                        return Search.Description;

                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public Uri ThumbnailUri
        {
            get
            {
                switch(InformationSource) {
                    case SmileVideoInformationSource.Getthumbinfo:
                        return RawValueUtility.ConvertUri(Thumb.ThumbnailUrl);

                    case SmileVideoInformationSource.Feed:
                        return RawValueUtility.ConvertUri(FeedDetail.ThumbnailUrl);

                    case SmileVideoInformationSource.Search:
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
                switch(InformationSource) {
                    case SmileVideoInformationSource.Getthumbinfo:
                        return RawValueUtility.ConvertDateTime(Thumb.FirstRetrieve);

                    case SmileVideoInformationSource.Feed:
                        return RawValueUtility.ConvertDateTime(FeedDetail.FirstRetrieve);

                    case SmileVideoInformationSource.Search:
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
                switch(InformationSource) {
                    case SmileVideoInformationSource.Getthumbinfo:
                        return SmileVideoGetthumbinfoUtility.ConvertTimeSpan(Thumb.Length);

                    case SmileVideoInformationSource.Feed:
                        return SmileVideoGetthumbinfoUtility.ConvertTimeSpan(FeedDetail.Length);

                    case SmileVideoInformationSource.Search:
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public SmileVideoMovieType MovieType { get { return SmileVideoGetthumbinfoUtility.ConvertMovieType(Thumb.MovieType); } }
        public long SizeHigh { get { return RawValueUtility.ConvertLong(Thumb.SizeHigh); } }
        public long SizeLow { get { return RawValueUtility.ConvertLong(Thumb.SizeLow); } }
        public int ViewCounter
        {
            get
            {
                switch(InformationSource) {
                    case SmileVideoInformationSource.Getthumbinfo:
                        return RawValueUtility.ConvertInteger(Thumb.ViewCounter);

                    case SmileVideoInformationSource.Feed:
                        return RawValueUtility.ConvertInteger(FeedDetail.ViewCounter);

                    case SmileVideoInformationSource.Search:
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
                switch(InformationSource) {
                    case SmileVideoInformationSource.Getthumbinfo:
                        return RawValueUtility.ConvertInteger(Thumb.CommentNum);

                    case SmileVideoInformationSource.Feed:
                        return RawValueUtility.ConvertInteger(FeedDetail.CommentNum);

                    case SmileVideoInformationSource.Search:
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
                switch(InformationSource) {
                    case SmileVideoInformationSource.Getthumbinfo:
                        return RawValueUtility.ConvertInteger(Thumb.MylistCounter);

                    case SmileVideoInformationSource.Feed:
                        return RawValueUtility.ConvertInteger(FeedDetail.MylistCounter);

                    case SmileVideoInformationSource.Search:
                        return RawValueUtility.ConvertInteger(Search.MylistCounter);

                    default:
                        throw new NotImplementedException();
                }
            }
        }
        public Uri WatchUrl
        {
            get
            {
                switch(InformationSource) {
                    case SmileVideoInformationSource.Getthumbinfo:
                        return RawValueUtility.ConvertUri(Thumb.WatchUrl);

                    case SmileVideoInformationSource.Feed:
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
                    if(InformationLoadState == LoadState.Failure) {
                        return new CollectionModel<SmileVideoTagViewModel>();
                    }
                    switch(InformationSource) {
                        case SmileVideoInformationSource.Getthumbinfo: {
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

        public string UserId
        {
            get
            {
                ThrowNotGetthumbinfoSource();
                return Thumb.UserId;
            }
        }

        public bool IsOfficialVideo
        {
            get
            {
                object resultIsWeb;
                if(Mediation.ConvertValue(out resultIsWeb, typeof(bool), SmileMediationKey.inputIsScrapingVideoId, VideoId, typeof(string), ServiceType.Smile)) {
                    var result = (bool)resultIsWeb;
                    return result;
                } else {
                    throw new NotSupportedException(VideoId);
                }
            }
        }

        #region Getflv

        public bool HasGetflvError
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

        public string OptionalThreadId
        {
            get
            {
                ThrowHasNotGetflv();
                return Getflv.OptionalThreadId;
            }
        }

        public bool IsPremium
        {
            get
            {
                ThrowHasNotGetflv();
                return Getflv.IsPremium == "1";
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

        #region IndividualVideoSetting

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
        public bool ConvertedSwf
        {
            get { return IndividualVideoSetting.ConvertedSwf; }
            set { SetPropertyValue(IndividualVideoSetting, value, nameof(IndividualVideoSetting.ConvertedSwf)); }
        }

        public bool IsEnabledGlobalCommentFilering
        {
            get { return IndividualVideoSetting.IsEnabledGlobalCommentFilering; }
            set { SetPropertyValue(IndividualVideoSetting, value, nameof(IndividualVideoSetting.IsEnabledGlobalCommentFilering)); }
        }

        /// <summary>
        /// この動画に対するフィルタ。
        /// </summary>
        public SmileVideoFilteringSettingModel Filtering
        {
            get { return IndividualVideoSetting.Filtering; }
        }

        #endregion

        /// <summary>
        /// 視聴済みか。
        /// </summary>
        public bool ViewingAlready
        {
            get
            {
                return Setting.History.Any(v => v.VideoId == VideoId);
            }
        }
        public DateTime ViewingTimestamp
        {
            get
            {
                var history = Setting.History.FirstOrDefault(v => v.VideoId == VideoId);
                if(history != null) {
                    return history.LastTimestamp;
                }

                return DateTime.MinValue;
            }
        }

        #endregion

        #region command

        public ICommand OpenVideoCommand
        {
            get { return CreateCommand(o => { OpenPlayerAsync(false); }); }
        }

        public ICommand OpeneconomyVideoCommand
        {
            get { return CreateCommand(o => { OpenPlayerAsync(true); }); }
        }

        #endregion

        #region function

        public SmileVideoVideoItemModel ToVideoItemModel()
        {
            var result = new SmileVideoVideoItemModel() {
                FirstRetrieve = this.FirstRetrieve,
                Length = this.Length,
                VideoId = this.VideoId,
                VideoTitle = this.Title,
                WatchUrl = this.WatchUrl,
            };

            return result;
        }

        static string GetCacheFileName(string videoId, string roll, string extension)
        {
            return PathUtility.CreateFileName(videoId, roll, extension);
        }

        string GetCacheFileName(string roll, string extension)
        {
            CheckUtility.EnforceNotNullAndNotEmpty(roll);
            return PathUtility.CreateFileName(VideoId, roll, extension);
        }
        string GetCacheFileName(string extension)
        {
            return PathUtility.CreateFileName(VideoId, extension);
        }

        static DirectoryInfo GetCahceDirectory(Mediation mediation, string videoId)
        {
            var parentDir = mediation.GetResultFromRequest<DirectoryInfo>(new RequestModel(RequestKind.CacheDirectory, ServiceType.SmileVideo));
            var cachedDirPath = Path.Combine(parentDir.FullName, videoId);

            return Directory.CreateDirectory(cachedDirPath);
        }

        //static async Task<RawSmileVideoThumbResponseModel> LoadGetthumbinfoAsync(Mediation mediation, string videoId, CacheSpan thumbCacheSpan)
        //{
        //    var cacheDir = GetCahceDirectory(mediation, videoId);

        //    RawSmileVideoThumbResponseModel rawGetthumbinfo = null;
        //    var cachedThumbFilePath = Path.Combine(cacheDir.FullName, GetCacheFileName(videoId, "thumb", "xml"));
        //    if(File.Exists(cachedThumbFilePath)) {
        //        var fileInfo = new FileInfo(cachedThumbFilePath);
        //        if(thumbCacheSpan.IsCacheTime(fileInfo.LastWriteTime) && Constants.MinimumXmlFileSize <= fileInfo.Length) {
        //            using(var stream = new FileStream(cachedThumbFilePath, FileMode.Open, FileAccess.Read, FileShare.Read)) {
        //                rawGetthumbinfo = Getthumbinfo.ConvertFromRawData(stream);
        //            }
        //        }
        //    }
        //    if(rawGetthumbinfo == null || !SmileVideoGetthumbinfoUtility.IsSuccessResponse(rawGetthumbinfo)) {
        //        var getthumbinfo = new Getthumbinfo(mediation);
        //        rawGetthumbinfo = await getthumbinfo.LoadAsync(videoId);
        //    }

        //    // キャッシュ構築
        //    try {
        //        SerializeUtility.SaveXmlSerializeToFile(cachedThumbFilePath, rawGetthumbinfo);
        //    } catch(FileNotFoundException) {
        //        // BUGS: いかんのう
        //    }

        //    return rawGetthumbinfo;
        //}

        //[Obsolete]
        //public static async Task<SmileVideoInformationViewModel> CreateFromVideoIdAsync(Mediation mediation, string videoId, CacheSpan thumbCacheSpan)
        //{
        //    var rawGetthumbinfo = await SmileVideoInformationUtility.LoadGetthumbinfoAsync(mediation, videoId, thumbCacheSpan);
        //    return new SmileVideoInformationViewModel(mediation, rawGetthumbinfo.Thumb, NoOrderd);
        //}

        void Initialize()
        {
            CacheDirectory = GetCahceDirectory(Mediation, VideoId);

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
            if(InformationSource != SmileVideoInformationSource.Getthumbinfo) {
                throw new InvalidOperationException($"{nameof(InformationSource)}: {InformationSource}");
            }
        }

        public string GetVideoFileName(bool isEconomyMode)
        {
            ThrowNotGetthumbinfoSource();

            return SmileVideoGetthumbinfoUtility.GetFileName(VideoId, MovieType, isEconomyMode);
        }

        public Task LoadThumbnaiImageAsync(CacheSpan cacheSpan, HttpClient client)
        {
            ThumbnailLoadState = LoadState.Preparation;

            var cachedFilePath = Path.Combine(CacheDirectory.FullName, GetCacheFileName("png"));
            if(CacheImageUtility.ExistImage(cachedFilePath, cacheSpan)) {
                ThumbnailLoadState = LoadState.Loading;
                this._thumbnailImage = CacheImageUtility.LoadBitmapBinary(cachedFilePath);
                ThumbnailLoadState = LoadState.Loaded;
                return Task.CompletedTask;
            }

            ThumbnailLoadState = LoadState.Loading;
            return CacheImageUtility.LoadBitmapBinaryDefaultAsync(client, ThumbnailUri, Mediation.Logger).ContinueWith(task => {
                var image = task.Result;
                if(image != null) {
                    this._thumbnailImage = image;
                    ThumbnailLoadState = LoadState.Loaded;
                    CacheImageUtility.SaveBitmapSourceToPngAsync(image, cachedFilePath, Mediation.Logger);
                } else {
                    ThumbnailLoadState = LoadState.Failure;
                }
            });
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

            return SmileVideoInformationUtility.LoadGetthumbinfoAsync(Mediation, VideoId, cacheSpan).ContinueWith(task => {
                var rawGetthumbinfo = task.Result;
                if(!SmileVideoGetthumbinfoUtility.IsSuccessResponse(rawGetthumbinfo)) {
                    InformationLoadState = LoadState.Failure;
                    return;
                }
                //SmileVideoTh
                //if(rawGetthumbinfo.Status)
                Thumb = rawGetthumbinfo.Thumb;
                InformationLoadState = LoadState.Loaded;
                InformationSource = SmileVideoInformationSource.Getthumbinfo;
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

            var htmlDocument = new HtmlDocument() {
                OptionAutoCloseOnEnd = true,
            };
            return Task.Run(() => {
                PageHtml = html;
                htmlDocument.LoadHtml(html);
                //document.DocumentNode.SelectSingleNode("@")
                var description = htmlDocument.DocumentNode.SelectSingleNode("//*[@class='videoDescription']");
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

                    //var videoInformation = new SmileVideoInformationViewModel(Mediation, item, video.Index + 1, SmileVideoInformationFlags.All);
                    var request = new SmileVideoInformationCacheRequestModel(new SmileVideoInformationCacheParameterModel(item, SmileVideoInformationFlags.All));
                    var videoInformation = Mediation.GetResultFromRequest<SmileVideoInformationViewModel>(request);

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

        public Task OpenPlayerAsync(bool forceEconomy)
        {
            var vm = new SmileVideoPlayerViewModel(Mediation);
            var task = vm.LoadAsync(this, forceEconomy, Constants.ServiceSmileVideoThumbCacheSpan, Constants.ServiceSmileVideoImageCacheSpan);

            Mediation.Request(new ShowViewRequestModel(RequestKind.ShowView, ServiceType.SmileVideo, vm, ShowViewState.Foreground));

            return task;
        }

        #endregion
    }
}