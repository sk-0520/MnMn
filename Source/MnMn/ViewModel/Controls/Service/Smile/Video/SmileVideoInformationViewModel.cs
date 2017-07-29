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
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.Library.Bridging.IF.ReadOnly;
using ContentTypeTextNet.MnMn.Library.Bridging.Model;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.IF.Control;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.IF.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Api;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video.Api;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video.Api.V1;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.MultiCommandParameter.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Video.Parameter;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Raw;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw.Feed;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.View.Controls.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Market;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Laboratory;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Player;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Service.Smile;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video
{
    /// <summary>
    /// 動画情報。
    /// </summary>
    public class SmileVideoInformationViewModel : InformationViewModelBase, IGarbageCollection
    {
        #region variable

        //LoadState _thumbnailLoadState;
        //LoadState _informationLoadState;
        LoadState _pageHtmlLoadState;
        LoadState _relationVideoLoadState;

        //BitmapSource _thumbnailImage;

        bool? _isEconomyMode;

        CollectionModel<SmileVideoTagViewModel> _tagList;

        bool _isPlaying = false;
        bool _isDownloading = false;

        JObject _dmcInfo;

        //int _referenceCount;
        string _descriptionHtmlSource;

        bool _force_Issue665NA = false;
        bool _force_Issue665NA_forceFlashPage = false;

        #endregion

        protected SmileVideoInformationViewModel(Mediator mediator, int number, SmileVideoInformationFlags informationFlags)
        {
            Mediator = mediator;
            ThumbnailLoadState = LoadState.None;
            InformationFlags = informationFlags;

            NetworkSetting = Mediator.GetNetworkSetting();
            Logger = Mediator.Logger;
        }

        public SmileVideoInformationViewModel(Mediator mediator, RawSmileVideoThumbModel thumb, int number)
            : this(mediator, number, SmileVideoInformationFlags.All)
        {
            Thumb = thumb;

            InformationSource = SmileVideoInformationSource.Getthumbinfo;
            InformationLoadState = LoadState.Loaded;

            Initialize();
        }

        public SmileVideoInformationViewModel(Mediator mediator, RawSmileContentsSearchItemModel search, int number)
            : this(mediator, number, SmileVideoInformationFlags.CommentCounter | SmileVideoInformationFlags.MylistCounter | SmileVideoInformationFlags.ViewCounter)
        {
            ContentsSearch = search;

            InformationSource = SmileVideoInformationSource.ContentsSearch;
            Initialize();
        }

        public SmileVideoInformationViewModel(Mediator mediator, RawSmileVideoSearchItemModel search, int number)
            : this(mediator, number, SmileVideoInformationFlags.CommentCounter | SmileVideoInformationFlags.MylistCounter | SmileVideoInformationFlags.ViewCounter)
        {
            OfficialSearch = search;

            InformationSource = SmileVideoInformationSource.OfficialSearch;
            Initialize();
        }

        public SmileVideoInformationViewModel(Mediator mediator, FeedSmileVideoItemModel feed, int number, SmileVideoInformationFlags informationFlags)
            : this(mediator, number, informationFlags)
        {
            Feed = feed;
            FeedDetail = SmileVideoFeedUtility.ConvertRawDescription(Feed.Description);
            FeedDetail.VideoId = SmileVideoFeedUtility.GetVideoId(Feed);
            FeedDetail.Title = SmileVideoFeedUtility.GetTitle(Feed.Title);

            InformationSource = SmileVideoInformationSource.Feed;
            Initialize();
        }

        #region property

        /// <summary>
        /// 橋渡し。
        /// </summary>
        Mediator Mediator { get; }
        /// <summary>
        /// 動画サービス設定。。
        /// </summary>
        SmileVideoSettingModel Setting { get; set; }
        /// <summary>
        /// 自身の初期化時に所持している動画情報。
        /// </summary>
        SmileVideoInformationFlags InformationFlags { get; }
        /// <summary>
        /// 動画情報を読み込む必要があるか。
        /// <para>全情報を保持していなければ何かしらのデータを引っ張ってくる必要あり。</para>
        /// </summary>
        public bool NeedLoadInformationFlag { get { return InformationFlags != SmileVideoInformationFlags.All; } }
        /// <summary>
        /// 本動画に対する個別設定。
        /// </summary>
        SmileVideoIndividualVideoSettingModel IndividualVideoSetting { get; set; } = new SmileVideoIndividualVideoSettingModel();

        #region service

        RawSmileVideoThumbModel Thumb { get; set; }
        FeedSmileVideoItemModel Feed { get; set; }
        RawSmileContentsSearchItemModel ContentsSearch { get; set; }
        RawSmileVideoSearchItemModel OfficialSearch { get; set; }
        /// <summary>
        /// フィードデータの詳細部。
        /// </summary>
        RawSmileVideoFeedDetailModel FeedDetail { get; set; }

        RawSmileVideoGetflv_Issue665NA_Model Getflv_Issue665NA { get; set; }
        SmileVideoWatchDataModel WatchData { get; set; }

        public RawSmileVideoGetthreadkeyModel Getthreadkey { get; private set; }

        #endregion

        #region ファイル

        /// <summary>
        /// 動画個別設定。
        /// </summary>
        protected virtual FileInfo IndividualVideoSettingFile { get; set; }
        /// <summary>
        /// キャッシュディレクトリ。
        /// </summary>
        public virtual DirectoryInfo CacheDirectory { get; private set; }
        /// <summary>
        /// 視聴ページHTMLファイル。
        /// </summary>
        public FileInfo WatchPageHtmlFile { get; private set; }
        /// <summary>
        /// サムネイル画像ファイル。
        /// </summary>
        public FileInfo ThumbnaiImageFile { get; private set; }
        /// <summary>
        ///動画実情報ファイル。
        /// </summary>
        public FileInfo GetflvFile { get; private set; }
        public FileInfo WatchDataFile { get; private set; }
        /// <summary>
        /// コメントファイル。
        /// <para>Flash版。</para>
        /// </summary>
        public FileInfo MsgFile_Issue665NA { get; private set; }
        /// <summary>
        /// コメントファイル。
        /// </summary>
        public FileInfo MsgFile { get; private set; }

        /// <summary>
        /// DMC形式受信ファイル。
        /// </summary>
        public FileInfo DmcFile { get; private set; }

        #endregion

        /// <summary>
        /// 視聴ページのHTMLソース。
        /// </summary>
        public string WatchPageHtmlSource_Issue665NA { get; private set; }
        /// <summary>
        /// 動画紹介HTMLソース。
        /// </summary>
        public string DescriptionHtmlSource
        {
            get { return this._descriptionHtmlSource; }
            // DescriptionBase用のIFとしてpublic。
            set { SetVariableValue(ref this._descriptionHtmlSource, value); }
        }

        public string PageVideoToken {
            get
            {
                if(IsCompatibleIssue665NA) {
                    return PageVideoToken_Issue665NA;
                }

                return WatchData.RawData.Api.Context.CsrfToken;
            }
        }
        public string PageVideoToken_Issue665NA { get; set; }

        /// <summary>
        /// 元にしている動画生情報。
        /// </summary>
        public SmileVideoInformationSource InformationSource { get; private set; }
        /// <summary>
        /// 元にしている動画生情報は複数あるか。
        /// </summary>
        public bool IsMultiInformationSource { get; private set; }

        /// <summary>
        /// ページ読み込み状態。
        /// </summary>
        public LoadState PageHtmlLoadState
        {
            get { return this._pageHtmlLoadState; }
            set { SetVariableValue(ref this._pageHtmlLoadState, value); }
        }

        /// <summary>
        /// 関連動画読み込み状態。
        /// </summary>
        public LoadState RelationVideoLoadState
        {
            get { return this._relationVideoLoadState; }
            set { SetVariableValue(ref this._relationVideoLoadState, value); }
        }
        /// <summary>
        /// 動画の長さを保持しているか。
        /// </summary>
        public bool HasLength { get { return InformationFlags.HasFlag(SmileVideoInformationFlags.Length); } }
        /// <summary>
        /// 視聴数を保持しているか。
        /// </summary>
        public bool HasViewConter { get { return InformationFlags.HasFlag(SmileVideoInformationFlags.ViewCounter); } }
        /// <summary>
        /// コメント数を保持しているか。
        /// </summary>
        public bool HasCommentCounter { get { return InformationFlags.HasFlag(SmileVideoInformationFlags.CommentCounter); } }
        /// <summary>
        /// マイリスト数を保持しているか。
        /// </summary>
        public bool HasMylistCounter { get { return InformationFlags.HasFlag(SmileVideoInformationFlags.MylistCounter); } }
        /// <summary>
        /// 投稿日を保持しているか。
        /// </summary>
        public bool HasFirstRetrieve { get { return InformationFlags.HasFlag(SmileVideoInformationFlags.FirstRetrieve); } }

        [Obsolete]
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

        public DateTime LastShowTimestamp
        {
            get { return IndividualVideoSetting.LastShowTimestamp; }
            set { SetPropertyValue(IndividualVideoSetting, value); }
        }

        #region 生データから取得

        public virtual string VideoId
        {
            get
            {
                switch(InformationSource) {
                    case SmileVideoInformationSource.Getthumbinfo:
                        return Thumb.VideoId;

                    case SmileVideoInformationSource.Feed:
                        return FeedDetail.VideoId;

                    case SmileVideoInformationSource.ContentsSearch:
                        return ContentsSearch.ContentId;

                    case SmileVideoInformationSource.OfficialSearch:
                        return OfficialSearch.Id;

                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public override string Title
        {
            get
            {
                switch(InformationSource) {
                    case SmileVideoInformationSource.Getthumbinfo:
                        return Thumb.Title;

                    case SmileVideoInformationSource.Feed:
                        return FeedDetail.Title ?? Feed.Title;

                    case SmileVideoInformationSource.ContentsSearch:
                        return ContentsSearch.Title;

                    case SmileVideoInformationSource.OfficialSearch:
                        return OfficialSearch.Title;

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

                    case SmileVideoInformationSource.ContentsSearch:
                        return ContentsSearch.Description;

                    case SmileVideoInformationSource.OfficialSearch:
                        return OfficialSearch.DescriptionShort;

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

                    case SmileVideoInformationSource.ContentsSearch:
                        return RawValueUtility.ConvertUri(ContentsSearch.ThumbnailUrl);

                    case SmileVideoInformationSource.OfficialSearch:
                        return RawValueUtility.ConvertUri(OfficialSearch.ThumbnailUrl);

                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public virtual DateTime FirstRetrieve
        {
            get
            {
                switch(InformationSource) {
                    case SmileVideoInformationSource.Getthumbinfo:
                        return RawValueUtility.ConvertDateTime(Thumb.FirstRetrieve);

                    case SmileVideoInformationSource.Feed:
                        return RawValueUtility.ConvertDateTime(FeedDetail.FirstRetrieve);

                    case SmileVideoInformationSource.ContentsSearch:
                        return RawValueUtility.ConvertDateTime(ContentsSearch.StartTime);

                    case SmileVideoInformationSource.OfficialSearch:
                        return RawValueUtility.ConvertDateTime(OfficialSearch.FirstRetrieve);

                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public virtual TimeSpan Length
        {
            get
            {
                if(IsMultiInformationSource) {
                    var multiLength = new[] {
                        Thumb?.Length,
                        FeedDetail?.Length,
                        OfficialSearch?.Length
                    };

                    return multiLength
                        .Where(s => !string.IsNullOrEmpty(s))
                        .Select(s => SmileVideoGetthumbinfoUtility.ConvertTimeSpan(s))
                        .Max()
                    ;
                }

                switch(InformationSource) {
                    case SmileVideoInformationSource.Getthumbinfo:
                        return SmileVideoGetthumbinfoUtility.ConvertTimeSpan(Thumb.Length);

                    case SmileVideoInformationSource.Feed:
                        return SmileVideoGetthumbinfoUtility.ConvertTimeSpan(FeedDetail.Length);

                    case SmileVideoInformationSource.OfficialSearch:
                        return SmileVideoGetthumbinfoUtility.ConvertTimeSpan(OfficialSearch.Length);

                    case SmileVideoInformationSource.ContentsSearch:
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public virtual SmileVideoMovieType MovieType
        {
            get
            {
                ThrowNotGetthumbinfoSource();
                return SmileVideoGetthumbinfoUtility.ConvertMovieType(Thumb.MovieType);
            }
        }
        public bool IsCompatibleIssue665NA => this._force_Issue665NA || MovieType == SmileVideoMovieType.Swf;

        public long SizeHigh { get { return RawValueUtility.ConvertLong(Thumb.SizeHigh); } }
        public long SizeLow { get { return RawValueUtility.ConvertLong(Thumb.SizeLow); } }
        public virtual int ViewCounter
        {
            get
            {
                switch(InformationSource) {
                    case SmileVideoInformationSource.Getthumbinfo:
                        return RawValueUtility.ConvertInteger(Thumb.ViewCounter);

                    case SmileVideoInformationSource.Feed:
                        return RawValueUtility.ConvertInteger(FeedDetail.ViewCounter);

                    case SmileVideoInformationSource.ContentsSearch:
                        return RawValueUtility.ConvertInteger(ContentsSearch.ViewCounter);

                    case SmileVideoInformationSource.OfficialSearch:
                        return RawValueUtility.ConvertInteger(OfficialSearch.ViewCounter);

                    default:
                        throw new NotImplementedException();
                }
            }
        }
        public virtual int CommentCounter
        {
            get
            {
                if(IsMultiInformationSource) {
                    var multiLength = new[] {
                        Thumb?.CommentNum,
                        FeedDetail?.CommentNum,
                        ContentsSearch?.CommentCounter,
                        OfficialSearch?.NumRes,
                    };

                    return multiLength
                        .Where(s => !string.IsNullOrEmpty(s))
                        .Select(s => RawValueUtility.ConvertInteger(s))
                        .Max()
                    ;
                }

                switch(InformationSource) {
                    case SmileVideoInformationSource.Getthumbinfo:
                        return RawValueUtility.ConvertInteger(Thumb.CommentNum);

                    case SmileVideoInformationSource.Feed:
                        return RawValueUtility.ConvertInteger(FeedDetail.CommentNum);

                    case SmileVideoInformationSource.ContentsSearch:
                        return RawValueUtility.ConvertInteger(ContentsSearch.CommentCounter);

                    case SmileVideoInformationSource.OfficialSearch:
                        return RawValueUtility.ConvertInteger(OfficialSearch.NumRes);

                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public virtual int MylistCounter
        {
            get
            {
                switch(InformationSource) {
                    case SmileVideoInformationSource.Getthumbinfo:
                        return RawValueUtility.ConvertInteger(Thumb.MylistCounter);

                    case SmileVideoInformationSource.Feed:
                        return RawValueUtility.ConvertInteger(FeedDetail.MylistCounter);

                    case SmileVideoInformationSource.ContentsSearch:
                        return RawValueUtility.ConvertInteger(ContentsSearch.MylistCounter);

                    case SmileVideoInformationSource.OfficialSearch:
                        return RawValueUtility.ConvertInteger(OfficialSearch.MylistCounter);

                    default:
                        throw new NotImplementedException();
                }
            }
        }
        public virtual Uri WatchUrl
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

        public string WatchUrlString
        {
            get
            {
                try {
                    return WatchUrl?.OriginalString;
                } catch(NotImplementedException) {
                    return string.Empty;
                }
            }
        }

        public SmileVideoThumbType ThumbType { get { return SmileVideoGetthumbinfoUtility.ConvertThumbType(Thumb.ThumbType); } }
        public bool Embeddable { get { return SmileVideoGetthumbinfoUtility.IsEmbeddable(Thumb.Embeddable); } }
        public bool LivePlay { get { return SmileVideoGetthumbinfoUtility.IsLivePlay(Thumb.NoLivePlay); } }
        public Uri UserIconUrl { get { return RawValueUtility.ConvertUri(Thumb.UserIconUrl); } }

        public virtual CollectionModel<SmileVideoTagViewModel> TagList
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
                                    var list = tagItems.Tags.Select(t => new SmileVideoTagViewModel(Mediator, t));
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

        public virtual string UserName
        {
            get
            {
                ThrowNotGetthumbinfoSource();

                return Thumb.UserNickname;
            }
        }

        public virtual string UserId
        {
            get
            {
                ThrowNotGetthumbinfoSource();
                return Thumb.UserId;
            }
        }

        public virtual string ChannelId
        {
            get
            {
                ThrowNotGetthumbinfoSource();

                if(string.IsNullOrEmpty(Thumb.ChannelId)) {
                    return string.Empty;
                }

                return SmileIdUtility.ConvertChannelId(Thumb.ChannelId, Mediator);
            }
        }

        public virtual string ChannelName
        {
            get
            {
                ThrowNotGetthumbinfoSource();
                return Thumb.ChannelName;
            }
        }

        /// <summary>
        /// チャンネル動画か。
        /// </summary>
        public virtual bool IsChannelVideo
        {
            get
            {
                ThrowNotGetthumbinfoSource();

                return string.IsNullOrEmpty(UserId);
            }
        }


        public bool IsOfficialVideo
        {
            get
            {
                return SmileIdUtility.IsScrapingVideoId(VideoId, Mediator);
            }
        }

        #region Getflv

        public bool HasGetflvError
        {
            get
            {
                ThrowHasNotGetflv_Issue665NA();

                return !string.IsNullOrWhiteSpace(Getflv_Issue665NA.Error) || string.IsNullOrWhiteSpace(Getflv_Issue665NA.MovieServerUrl);
            }
        }

        public bool HasWatchDataError
        {
            get
            {
                ThrowHasNotWatchData();

                return WatchData.RawData == null;//|| string.IsNullOrWhiteSpace(Getflv.MovieServerUrl);
            }
        }

        [Obsolete]
        public bool Done
        {
            get
            {
                if(IsCompatibleIssue665NA) {
                    ThrowHasNotGetflv_Issue665NA();
                    return RawValueUtility.ConvertBoolean(Getflv_Issue665NA.Done);
                }

                ThrowHasNotGetflv_Issue665NA();
                return RawValueUtility.ConvertBoolean(Getflv_Issue665NA.Done);
            }
        }

        bool IsEconomyMode_Issue665NA
        {
            get
            {
                ThrowHasNotGetflv_Issue665NA();

                if(!this._isEconomyMode.HasValue) {
                    this._isEconomyMode = SmileVideoGetflvUtility.IsEconomyMode(Getflv_Issue665NA.MovieServerUrl, Mediator);
                }

                return this._isEconomyMode.Value;
            }
        }

        public bool IsEconomyMode
        {
            get
            {
                if(IsCompatibleIssue665NA) {
                    return IsEconomyMode_Issue665NA;
                }

                ThrowHasNotWatchData();

                if(!this._isEconomyMode.HasValue) {
                    this._isEconomyMode = SmileVideoGetflvUtility.IsEconomyMode(MovieServerUrl.OriginalString, Mediator);
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
                if(IsCompatibleIssue665NA) {
                    ThrowHasNotGetflv_Issue665NA();
                    return new Uri(Getflv_Issue665NA.MovieServerUrl);
                }

                ThrowHasNotWatchData();
                return new Uri(WatchData.RawData.Api.Video.SmileInfo.Url);
            }
        }

        public Uri MessageServerUrl
        {
            get
            {
                if(IsCompatibleIssue665NA) {
                    ThrowHasNotGetflv_Issue665NA();
                    return new Uri(Getflv_Issue665NA.MessageServerUrl);
                }

                ThrowHasNotWatchData();
                return new Uri(WatchData.RawData.Api.Thread.ServerUrl);
            }
        }

        public Uri MessageServerSubUrl
        {
            get
            {
                if(IsCompatibleIssue665NA) {
                    ThrowHasNotGetflv_Issue665NA();
                    return new Uri(Getflv_Issue665NA.MessageServerSubUrl);
                }

                ThrowHasNotWatchData();
                return new Uri(WatchData.RawData.Api.Thread.SubServerUrl);
            }
        }

        public string ThreadId
        {
            get
            {
                if(IsCompatibleIssue665NA) {
                    ThrowHasNotGetflv_Issue665NA();
                    return Getflv_Issue665NA.ThreadId;
                }

                ThrowHasNotWatchData();
                return WatchData.RawData.Api.Thread.Ids.Default;
            }
        }

        public string CommunityThreadId
        {
            get
            {

                ThrowHasNotWatchData();
                return WatchData.RawData.Api.Thread.Ids.Community;
            }
        }

        public string OptionalThreadId
        {
            get
            {
                if(IsCompatibleIssue665NA) {
                    ThrowHasNotGetflv_Issue665NA();
                    return Getflv_Issue665NA.OptionalThreadId;
                }

                ThrowHasNotWatchData();
                var dmcInfo = WatchData.RawData.Api.Video.DmcInfo;
                if(dmcInfo != null) {
                    return dmcInfo.Thread.OptionalThreadId;
                }
                return string.Empty; ;
            }
        }

        public bool IsPremium
        {
            get
            {
                if(IsCompatibleIssue665NA) {
                    ThrowHasNotGetflv_Issue665NA();
                    return Getflv_Issue665NA.IsPremium == "1";
                }

                ThrowHasNotWatchData();
                return RawValueUtility.ConvertBoolean(WatchData.RawData.Api.Viewer.IsPremium);
            }
        }

        public IReadOnlyList<RawSmileVideoWatchDataTagModel> WatchTagItems
        {
            get
            {
                return WatchData.RawData.Api.Tags;
            }
        }

        #region dmc

        public bool IsDmc
        {
            get
            {
                if(IsCompatibleIssue665NA) {
                    ThrowHasNotGetflv_Issue665NA();
                    return RawValueUtility.ConvertBoolean(Getflv_Issue665NA.IsDmc);
                }

                ThrowHasNotWatchData();
                return WatchData.RawData.Api.Video.DmcInfo != null;
            }
        }

        //[Obsolete("SWF形式への互換性を残す目的でGetflv経由のDMCはもう保守しない")]
        // #703 の影響で保守せざるを得ん
        public JObject DmcInfo_Issue665NA
        {
            get
            {
                ThrowHasNotGetflv_Issue665NA();
                if(this._dmcInfo == null) {
                    this._dmcInfo = JObject.Parse(Getflv_Issue665NA.DmcInfo);
                }

                return this._dmcInfo;
            }
        }

        public RawSmileVideoWatchDataDmcInfoModel DmcInfo2
        {
            get
            {
                ThrowHasNotWatchData();
                return WatchData.RawData.Api.Video.DmcInfo;
            }
        }

        public string UserKey
        {
            get
            {
                ThrowHasNotWatchData();
                return WatchData.RawData.Api.Context.Userkey;
            }
        }

        public bool HasOriginalPostedComment
        {
            get
            {
                ThrowHasNotWatchData();

                return RawValueUtility.ConvertBoolean(WatchData.RawData.Api.Thread.HasOwnerThread);
            }
        }
        #endregion

        #endregion

        #endregion

        public bool HasGetflv => Getflv_Issue665NA != null;

        public bool HasWatchData => WatchData != null && WatchData.RawData != null;

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

        public IDictionary<string, SmileVideoDmcItemModel> DmcItems { get { return IndividualVideoSetting.DmcItems; } }

        public bool LoadedDmcVideos => DmcItems.Any(i => i.Value.IsLoaded);


        public bool IsEnabledGlobalCommentFilering
        {
            get { return IndividualVideoSetting.IsEnabledGlobalCommentFilering; }
            set { SetPropertyValue(IndividualVideoSetting, value, nameof(IndividualVideoSetting.IsEnabledGlobalCommentFilering)); }
        }

        /// <summary>
        /// この動画に対するフィルタ。
        /// </summary>
        public virtual SmileVideoCommentFilteringSettingModel Filtering
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

        /// <summary>
        /// <see cref="GarbageCollection(GarbageCollectionLevel, CacheSpan, bool)"/>を呼び出し可能か。
        /// <para>あくまでGCを実行できるかどうかであり削除可能ファイルが存在するかではない。</para>
        /// </summary>
        public bool CanGarbageCollection
        {
            get
            {
                if(IsPlaying || IsDownloading || IsDisposed) {
                    return false;
                }
                if(InformationSource != SmileVideoInformationSource.Getthumbinfo) {
                    return false;
                }

                return true;
            }
        }

        #endregion

        #region command

        public ICommand OpenVideoDefaultCommand
        {
            get { return CreateCommand(o => { OpenVideoDefaultAsync(false); }); }
        }

        public ICommand OpenEconomyVideoDefaultCommand
        {
            get { return CreateCommand(o => { OpenVideoDefaultAsync(true); }); }
        }

        public ICommand OpenVideoFrommParameterCommnad
        {
            get
            {
                return CreateCommand(
                    o => {
                        var commandParameter = (SmileVideoOpenVideoCommandParameterModel)o;
                        OpenVideoFromOpenParameterAsync(false, commandParameter.OpenMode, commandParameter.OpenPlayerInNewWindow);
                    },
                    o => {
                        var commandParameter = (SmileVideoOpenVideoCommandParameterModel)o;
                        if(commandParameter.OpenMode == ExecuteOrOpenMode.Launcher) {
                            var items = new[] {
                                Setting.Execute.LauncherPath,
                                Setting.Execute.LauncherParameter,
                            };
                            return items.All(s => !string.IsNullOrWhiteSpace(s));
                        } else {
                            return true;
                        }
                    }
                );
            }
        }

        public ICommand ClearCacheCommand
        {
            get
            {
                return CreateCommand(
                    o => ClearCache(),
                    o => CanGarbageCollection
                );
            }
        }

        #endregion

        #region function

        void ClearCache()
        {
            try {
                var checkResult = GarbageCollection(GarbageCollectionLevel.Large | GarbageCollectionLevel.Small | GarbageCollectionLevel.Temporary, CacheSpan.NoCache, true);
                if(checkResult.IsSuccess) {
                    Mediator.Logger.Information($"cache clear: [{VideoId}] {RawValueUtility.ConvertHumanLikeByte(checkResult.Result)}");
                } else {
                    Mediator.Logger.Warning($"cache clear: [{VideoId}] fail!");
                }
                CallOnPropertyChangeDisplayItem();
            } catch(Exception ex) {
                Mediator.Logger.Warning(ex);
            }
        }

        public void SetDmcLoaded(string video, string audio, bool isLoaded)
        {
            var role = SmileVideoInformationUtility.GetDmcRoleKey(video, audio);

            DmcItems[role].IsLoaded = isLoaded;

            CallOnPropertyChange(nameof(LoadedDmcVideos));
        }

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

        static string GetCacheFileName(string videoId, string role, string extension)
        {
            return PathUtility.CreateFileName(videoId, role, extension);
        }

        string GetCacheFileName(string role, string extension)
        {
            CheckUtility.EnforceNotNullAndNotEmpty(role);
            return PathUtility.CreateFileName(VideoId, role, extension);
        }
        string GetCacheFileName(string extension)
        {
            return PathUtility.CreateFileName(VideoId, extension);
        }

        static DirectoryInfo GetCahceDirectory(Mediator mediator, string videoId)
        {
            var parentDir = mediator.GetResultFromRequest<DirectoryInfo>(new RequestModel(RequestKind.CacheDirectory, ServiceType.SmileVideo));
            var cachedDirPath = Path.Combine(parentDir.FullName, videoId);

            return Directory.CreateDirectory(cachedDirPath);
        }

        void Initialize()
        {
            CacheDirectory = GetCahceDirectory(Mediator, VideoId);

            WatchPageHtmlFile = new FileInfo(Path.Combine(CacheDirectory.FullName, GetCacheFileName("page", "html")));
            ThumbnaiImageFile = new FileInfo(Path.Combine(CacheDirectory.FullName, GetCacheFileName("png")));
            GetflvFile = new FileInfo(Path.Combine(CacheDirectory.FullName, GetCacheFileName("getflv", "xml")));
            WatchDataFile = new FileInfo(Path.Combine(CacheDirectory.FullName, GetCacheFileName("watch-data", "json")));
            MsgFile_Issue665NA = new FileInfo(Path.Combine(CacheDirectory.FullName, GetCacheFileName(VideoId, "msg", "xml")));
            MsgFile = new FileInfo(Path.Combine(CacheDirectory.FullName, GetCacheFileName(VideoId, "msg", "json")));
            DmcFile = new FileInfo(Path.Combine(CacheDirectory.FullName, GetCacheFileName(VideoId, "dmc", "xml")));

            var resSetting = Mediator.Request(new RequestModel(RequestKind.Setting, ServiceType.SmileVideo));
            Setting = (SmileVideoSettingModel)resSetting.Result;

            IndividualVideoSettingFile = new FileInfo(Path.Combine(CacheDirectory.FullName, GetCacheFileName("setting", "json")));
            if(IndividualVideoSettingFile.Exists && Constants.MinimumJsonFileSize <= IndividualVideoSettingFile.Length) {
                IndividualVideoSetting = SerializeUtility.LoadJsonDataFromFile<SmileVideoIndividualVideoSettingModel>(IndividualVideoSettingFile.FullName);
            } else {
                IndividualVideoSetting = new SmileVideoIndividualVideoSettingModel();
            }
        }

        void ThrowHasNotGetflv_Issue665NA()
        {
            if(!HasGetflv) {
                throw new InvalidOperationException(nameof(Getflv_Issue665NA));
            }
        }

        void ThrowHasNotWatchData()
        {
            if(!HasWatchData) {
                throw new InvalidOperationException(Constants.ServiceSmileVideoGetVideoError);
            }
        }

        void ThrowNotGetthumbinfoSource()
        {
            if(InformationSource != SmileVideoInformationSource.Getthumbinfo) {
                throw new InvalidOperationException($"{VideoId}: {nameof(InformationSource)}: {InformationSource}");
            }
        }

        public string GetVideoFileName(bool isEconomyMode)
        {
            ThrowNotGetthumbinfoSource();

            return SmileVideoGetthumbinfoUtility.GetFileName(VideoId, MovieType, isEconomyMode);
        }


        public string GetDmcVideoFileName(string video, string audio, string ext)
        {
            ThrowNotGetthumbinfoSource();

            var roll = $"video.{SmileVideoInformationUtility.GetDmcRoleKey(video, audio)}.dmc";
            var baseName = GetCacheFileName(VideoId, roll, ext);
            var safeName = PathUtility.ToSafeNameDefault(baseName);
            return Path.Combine(CacheDirectory.FullName, safeName);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="isSave"></param>
        /// <param name="usingDmc">ダウンロードにDMC形式を使用するか</param>
        /// <returns></returns>
        public Task<IReadOnlyCheck> LoadGetflvAsync(bool isSave, bool usingDmc)
        {
            if(InformationLoadState == LoadState.Failure) {
                return Task.FromResult(CheckModel.Failure());
            }

            PageHtmlLoadState = LoadState.Loading;

            var getflv = new Getflv_Issue665NA(Mediator);

            return getflv.LoadAsync(VideoId, WatchUrl, MovieType, usingDmc, this._force_Issue665NA_forceFlashPage).ContinueWith(t => {
                PageHtmlLoadState = LoadState.Loaded;
                var rawVideoGetflvModel = t.Result;

                if(rawVideoGetflvModel != null) {
                    Getflv_Issue665NA = rawVideoGetflvModel;
                    SetPageHtml_Issue665NA(rawVideoGetflvModel.HtmlSource, isSave);

                    this._dmcInfo = null;
                    if(isSave) {
                        SerializeUtility.SaveXmlSerializeToFile(GetflvFile.FullName, rawVideoGetflvModel);
                    }
                    return CheckModel.Success();
                } else {
                    return CheckModel.Failure();
                }
            });
        }

        public Task<IReadOnlyCheck> LoadWatchDataAsync(bool isSave, bool usingDmc)
        {
            PageHtmlLoadState = LoadState.Loading;

            var watchData = new WatchData(Mediator);
            return watchData.LoadWatchDataAsync(WatchUrl, MovieType).ContinueWith(t => {
                PageHtmlLoadState = LoadState.Loaded;
                var wd = t.Result;
                if(wd != null) {
                    WatchData = wd;
                    SetPageHtml(WatchData.HtmlSource, isSave);
                    if(isSave) {
                        SerializeUtility.SaveJsonDataToFile(WatchDataFile.FullName, WatchData.RawData);
                        using(var writer = WatchPageHtmlFile.CreateText()) {
                            writer.Write(WatchData.HtmlSource);
                        }
                    }
                    return CheckModel.Success();
                } else {
                    return CheckModel.Failure();
                }
            });
        }

        public Task LoadGetthreadkeyAsync()
        {
            var getThreadkey = new Getthreadkey(Mediator);
            if(IsCompatibleIssue665NA) {
                return getThreadkey.Load_Issue665NA_Async(ThreadId).ContinueWith(t => {
                    if(t.IsFaulted) {
                        return CheckModel.Failure(t.Exception.InnerException);
                    } else {
                        Getthreadkey = t.Result;
                        return CheckModel.Success();
                    }
                });
            } else {
                return getThreadkey.LoadAsync(CommunityThreadId).ContinueWith(t => {
                    if(t.IsFaulted) {
                        return CheckModel.Failure(t.Exception.InnerException);
                    } else {
                        Getthreadkey = t.Result;
                        return CheckModel.Success();
                    }
                });
            }
        }

        void SetPageHtml_Issue665NA(string html, bool isSave)
        {
            PageHtmlLoadState = LoadState.Loading;

            var htmlDocument = new HtmlDocument() {
                OptionAutoCloseOnEnd = true,
            };
            WatchPageHtmlSource_Issue665NA = html;
            htmlDocument.LoadHtml(html);
            var description = htmlDocument.DocumentNode.SelectSingleNode("//*[@class='videoDescription']");
            DescriptionHtmlSource = description.InnerHtml;

            var json = SmileVideoWatchAPI_Issue665NA_Utility.ConvertJsonFromWatchPage(html);
            var flashvars = json.SelectToken("flashvars");
            PageVideoToken_Issue665NA = flashvars.Value<string>("csrfToken");

            PageHtmlLoadState = LoadState.Loaded;
            if(isSave) {
                File.WriteAllText(WatchPageHtmlFile.FullName, WatchPageHtmlSource_Issue665NA);
            }
        }

        protected virtual void SetPageHtml(string html, bool isSave)
        {
            if(IsCompatibleIssue665NA) {
                SetPageHtml_Issue665NA(html, isSave);
                return;
            }

            var htmlDocument = new HtmlDocument() {
                OptionAutoCloseOnEnd = true,
            };

            DescriptionHtmlSource = WatchData.RawData.Api.Video.Description;

            PageHtmlLoadState = LoadState.Loaded;
        }

        public Task<IEnumerable<SmileVideoInformationViewModel>> LoadRelationVideosAsync(CacheSpan cacheSpan)
        {
            RelationVideoLoadState = LoadState.Preparation;

            RelationVideoLoadState = LoadState.Loading;
            var getRelation = new Getrelation(Mediator);
            return getRelation.LoadAsync(VideoId, 1, Constants.ServiceSmileVideoRelationVideoSort, ContentTypeTextNet.Library.SharedLibrary.Define.OrderBy.Ascending).ContinueWith(task => {
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

                    var request = new SmileVideoInformationCacheRequestModel(new SmileVideoInformationCacheParameterModel(item, SmileVideoInformationFlags.All));
                    var videoInformation = Mediator.GetResultFromRequest<SmileVideoInformationViewModel>(request);

                    result.Add(videoInformation);
                }

                return (IEnumerable<SmileVideoInformationViewModel>)result;
            });
        }

        public Task<IEnumerable<SmileMarketVideoRelationItemViewModel>> LoadMarketItemsAsync()
        {
            var market = new Logic.Service.Smile.Api.V1.Market(Mediator);
            return market.LoadVideoRelationAsync(VideoId).ContinueWith(t => {
                var model = t.Result;
                var items = SmileMarketUtility.GetVideoRelationItems(model);
                if(items.Any()) {
                    return items.Select(i => new SmileMarketVideoRelationItemViewModel(Mediator, i));
                } else {
                    return Enumerable.Empty<SmileMarketVideoRelationItemViewModel>();
                }
            });
        }

        /// <summary>
        /// </summary>
        public virtual void LoadLocalPageHtml()
        {
            //PageHtmlLoadState = LoadState.Preparation;

            WatchPageHtmlFile.Refresh();
            if(WatchPageHtmlFile.Exists && Constants.MinimumHtmlFileSize <= WatchPageHtmlFile.Length) {
                using(var stream = WatchPageHtmlFile.OpenText()) {
                    var html = stream.ReadToEnd();
                    SetPageHtml(html, false);
                }
            }
        }

        public virtual bool SaveSetting(bool force)
        {
            if(!force && !IsChanged) {
                return false;
            }

            SerializeUtility.SaveJsonDataToFile(IndividualVideoSettingFile.FullName, IndividualVideoSetting);
            ResetChangeFlag();
            return true;
        }

        public Task OpenVideoDefaultAsync(bool forceEconomy)
        {
            return OpenVideoFromOpenParameterAsync(forceEconomy, Setting.Execute.OpenMode, Setting.Execute.OpenPlayerInNewWindow);
        }

        public Task OpenVideoCurrentWindowAsync(bool forceEconomy)
        {
            return OpenVideoFromOpenParameterAsync(forceEconomy, Setting.Execute.OpenMode, false);
        }

        public Task OpenVideoNewWindowAsync(bool forceEconomy)
        {
            return OpenVideoFromOpenParameterAsync(forceEconomy, Setting.Execute.OpenMode, true);
        }

        public Task OpenVideoFromOpenParameterAsync(bool forceEconomy, ExecuteOrOpenMode openMode, bool openPlayerInNewWindow)
        {
            switch(openMode) {
                case ExecuteOrOpenMode.Application:
                    return OpenVideoPlayerAsync(forceEconomy, openPlayerInNewWindow);

                case ExecuteOrOpenMode.Browser:
                    return OpenVideoBrowserAsync(forceEconomy);

                case ExecuteOrOpenMode.Launcher:
                    return OpenVideoLauncherAsync(forceEconomy);

                default:
                    throw new NotImplementedException();
            }
        }

        Task OpenVideoPlayerAsync(bool forceEconomy, bool openPlayerInNewWindow)
        {
            if(IsPlaying) {
                Mediator.Request(new ShowViewRequestModel(RequestKind.ShowView, ServiceType.SmileVideo, this, ShowViewState.Foreground));
                return Task.CompletedTask;
            } else if(IsDownloading) {
                Mediator.Logger.Warning($"[{VideoId}] {nameof(IsDownloading)}: {IsDownloading}");
                return Task.CompletedTask;
            } else {
                if(!openPlayerInNewWindow) {
                    var players = Mediator.GetResultFromRequest<IEnumerable<SmileVideoPlayerViewModel>>(new RequestModel(RequestKind.WindowViewModels, ServiceType.SmileVideo));
                    var workingPlayer = players
                        .Where(p => !(p is SmileVideoLaboratoryPlayerViewModel)) // 任意再生は除外
                        .FirstOrDefault(p => p.IsWorkingPlayer.Value)
                    ;
                    if(workingPlayer != null) {
                        // メインプレイヤーで再生
                        workingPlayer.MoveForeground();
                        return workingPlayer.LoadAsync(this, forceEconomy, Constants.ServiceSmileVideoThumbCacheSpan, Constants.ServiceSmileVideoImageCacheSpan);
                    }
                }

                var vm = new SmileVideoPlayerViewModel(Mediator);
                var task = vm.LoadAsync(this, forceEconomy, Constants.ServiceSmileVideoThumbCacheSpan, Constants.ServiceSmileVideoImageCacheSpan);

                Mediator.Request(new ShowViewRequestModel(RequestKind.ShowView, ServiceType.SmileVideo, vm, ShowViewState.Foreground));

                return task;
            }
        }

        public Task OpenVideoBrowserAsync(bool forceEconomy)
        {
            ShellUtility.OpenUriInDefaultBrowser(WatchUrl, Mediator.Logger);
            return Task.CompletedTask;
        }

        public Task OpenVideoLauncherAsync(bool forceEconomy)
        {
            try {
                var keyword = Mediator.GetResultFromRequest<IReadOnlySmileVideoKeyword>(new SmileVideoOtherDefineRequestModel(SmileVideoOtherDefineKind.Keyword));
                var args = SmileVideoInformationUtility.MakeLauncherParameter(this, keyword, Setting.Execute.LauncherParameter);
                Process.Start(Setting.Execute.LauncherPath, args);
            } catch(Exception ex) {
                Mediator.Logger.Error(ex);
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// 早めに死んでほしい処理。
        /// </summary>
        internal void Force_Issue665NA(bool forceFlashPage)
        {
            Mediator.Logger.Warning($"!!force!! [{VideoId}] #665");
            this._force_Issue665NA = true;
            this._force_Issue665NA_forceFlashPage = forceFlashPage;
            CallOnPropertyChange(nameof(IsCompatibleIssue665NA));
        }

        #endregion

        #region IGarbageCollection

        IReadOnlyCheckResult<long> GarbageCollectionFromFile(FileInfo file, CacheSpan cacheSpan, bool force)
        {
            try {
                file.Refresh();

                if(file.Exists) {
                    var timestamps = new[] {
                        file.CreationTime,
                        file.LastWriteTime,
                        IndividualVideoSetting.LastShowTimestamp,
                    };
                    var timestamp = timestamps.Max();
                    if(force || !cacheSpan.IsCacheTime(timestamp)) {
                        var fileSize = file.Length;
                        file.Delete();
                        return CheckResultModel.Success(fileSize);
                    }
                }
            } catch(Exception ex) {
                Mediator.Logger.Warning($"{file}: {ex.Message}", ex);
            }

            return CheckResultModel.Failure<long>();
        }

        long GarbageCollectionLarge(CacheSpan cacheSpan, bool force)
        {
            if(!force && cacheSpan.IsNoExpiration) {
                return 0;
            }

            // 動画ファイル破棄
            var normalFile = new FileInfo(Path.Combine(CacheDirectory.FullName, GetVideoFileName(false)));
            var economyFile = new FileInfo(Path.Combine(CacheDirectory.FullName, GetVideoFileName(true)));
            // Flash変換後ファイル
            var normalFlashConvertedFile = new FileInfo(PathUtility.AppendExtension(normalFile.FullName, SmileVideoInformationUtility.flashConvertedExtension));
            // エコノミーでFlashってのは多分ないんだろうね
            var economyFlashConvertedFile = new FileInfo(PathUtility.AppendExtension(normalFile.FullName, SmileVideoInformationUtility.flashConvertedExtension));

            var normalCheck = GarbageCollectionFromFile(normalFile, cacheSpan, force);
            var economyCheck = GarbageCollectionFromFile(economyFile, cacheSpan, force);
            var normalFlashCheck = GarbageCollectionFromFile(normalFlashConvertedFile, cacheSpan, force);
            var economyFlashCheck = GarbageCollectionFromFile(economyFlashConvertedFile, cacheSpan, force);

            var needSave = false;

            if(normalCheck.IsSuccess) {
                var temp = IndividualVideoSetting.LoadedNormal;
                IndividualVideoSetting.LoadedNormal = false;
                needSave = true;
            }
            if(economyCheck.IsSuccess) {
                var temp = IndividualVideoSetting.LoadedEconomyMode;
                IndividualVideoSetting.LoadedEconomyMode = false;
                needSave = true;
            }
            if(normalFlashCheck.IsSuccess || economyFlashCheck.IsSuccess) {
                var temp = IndividualVideoSetting.ConvertedSwf;
                IndividualVideoSetting.ConvertedSwf = false;
                needSave = true;
            }

            var dmcCheckes = new List<IReadOnlyCheckResult<long>>();

            try {
                var dmcFiles = CacheDirectory.EnumerateFiles("*-video.*.dmc.*", SearchOption.TopDirectoryOnly).ToEvaluatedSequence();
                foreach(var dmcFile in dmcFiles) {
                    var check = GarbageCollectionFromFile(dmcFile, cacheSpan, force);
                    if(check.IsSuccess) {
                        var role = Regex.Replace(dmcFile.Name, @".*-video\.(\[.*\])\.dmc\..*", "$1");
                        SmileVideoDmcItemModel item;
                        if(IndividualVideoSetting.DmcItems.TryGetValue(role, out item)) {
                            item.IsLoaded = false;
                        }
                        needSave = true;
                    }
                    dmcCheckes.Add(check);
                }
                CallOnPropertyChange(nameof(LoadedDmcVideos));
            } catch(Exception ex) {
                Mediator.Logger.Error(ex);
            }

            var checks = new[] {
                normalCheck,
                economyCheck,
                normalFlashCheck,
                economyFlashCheck,
            }.Concat(dmcCheckes);

            if(needSave) {
                SaveSetting(true);
            }

            return checks.Where(c => c.IsSuccess).Sum(c => c.Result);
        }

        private long GarbageCollectionTemporary(CacheSpan cacheSpan, bool force)
        {
            if(!force && cacheSpan.IsNoExpiration) {
                return 0;
            }

            try {
                long size = 0;

                WatchPageHtmlFile.Refresh();
                if(WatchPageHtmlFile.Exists) {
                    size += WatchPageHtmlFile.Length;
                    WatchPageHtmlFile.Delete();
                }

                GetflvFile.Refresh();
                if(GetflvFile.Exists) {
                    size += GetflvFile.Length;
                    GetflvFile.Delete();
                }

                WatchDataFile.Refresh();
                if(WatchDataFile.Exists) {
                    size += WatchDataFile.Length;
                    WatchDataFile.Delete();
                }

                return size;
            } catch(Exception ex) {
                Mediator.Logger.Warning($"{ex.Message}", ex);
            }

            return 0;
        }

        long GarbageCollectionSmall(CacheSpan cacheSpan, bool force)
        {
            if(!force && cacheSpan.IsNoExpiration) {
                return 0;
            }

            try {
                long size = 0;

                var msgCheck = GarbageCollectionFromFile(MsgFile, cacheSpan, force);
                var msgCheck_Issue665NA = GarbageCollectionFromFile(MsgFile_Issue665NA, cacheSpan, force);

                if(msgCheck.IsSuccess) {
                    size += msgCheck.Result;
                }
                if(msgCheck_Issue665NA.IsSuccess) {
                    size += msgCheck_Issue665NA.Result;
                }

                return size;
            } catch(Exception ex) {
                Mediator.Logger.Warning($"{ex.Message}", ex);
            }

            return 0;
        }

        public IReadOnlyCheckResult<long> GarbageCollection(GarbageCollectionLevel garbageCollectionLevel, CacheSpan cacheSpan, bool force)
        {
            if(!CanGarbageCollection) {
                return CheckResultModel.Failure<long>();
            }

            // キャッシュディレクトリ自体がなければ終了する
            CacheDirectory.Refresh();
            if(!CacheDirectory.Exists) {
                return CheckResultModel.Success(0L);
            }

            long largeSize = 0;
            if(garbageCollectionLevel.HasFlag(GarbageCollectionLevel.Large)) {
                largeSize += GarbageCollectionLarge(cacheSpan, force);
            }

            if(garbageCollectionLevel.HasFlag(GarbageCollectionLevel.Small)) {
                largeSize += GarbageCollectionSmall(cacheSpan, force);
            }

            if(garbageCollectionLevel.HasFlag(GarbageCollectionLevel.Temporary)) {
                largeSize += GarbageCollectionTemporary(cacheSpan, force);
            }

            return CheckResultModel.Success(largeSize);
        }

        public void MergeSource(RawSmileContentsSearchItemModel search)
        {
            ContentsSearch = search;
            IsMultiInformationSource = true;
        }

        public void MergeSource(RawSmileVideoSearchItemModel search)
        {
            OfficialSearch = search;
            IsMultiInformationSource = true;
        }

        public void MergeSource(FeedSmileVideoItemModel feed)
        {
            Feed = feed;
            FeedDetail = SmileVideoFeedUtility.ConvertRawDescription(Feed.Description);
            FeedDetail.VideoId = SmileVideoFeedUtility.GetVideoId(Feed);
            FeedDetail.Title = SmileVideoFeedUtility.GetTitle(Feed.Title);
            IsMultiInformationSource = true;
        }

        #endregion

        #region ViewModelBase

        protected override Task<bool> LoadInformationCoreAsync(CacheSpan cacheSpan, HttpClient client)
        {
            return SmileVideoInformationUtility.LoadGetthumbinfoAsync(Mediator, VideoId, cacheSpan).ContinueWith(task => {
                var rawGetthumbinfo = task.Result;
                if(!SmileVideoGetthumbinfoUtility.IsSuccessResponse(rawGetthumbinfo)) {
                    return false;
                }

                Thumb = rawGetthumbinfo.Thumb;
                InformationSource = SmileVideoInformationSource.Getthumbinfo;

                IsMultiInformationSource = true;

                var propertyNames = new[] {
                    nameof(Length),
                    nameof(HasLength),
                    nameof(InformationLoadState),
                };
                CallOnPropertyChange(propertyNames);

                return true;
            });
        }

        protected override Task<bool> LoadThumbnaiImageCoreAsync(CacheSpan cacheSpan, HttpClient client)
        {
            ThumbnailLoadState = LoadState.Preparation;

            if(CacheImageUtility.ExistImage(ThumbnaiImageFile.FullName, cacheSpan)) {
                ThumbnailLoadState = LoadState.Loading;
                var cacheImage = CacheImageUtility.LoadBitmapBinary(ThumbnaiImageFile.FullName);
                SetThumbnaiImage(cacheImage);
                //ThumbnailLoadState = LoadState.Loaded;
                return Task.FromResult(true);
            }

            ThumbnailLoadState = LoadState.Loading;
            return CacheImageUtility.LoadBitmapBinaryDefaultAsync(client, ThumbnailUri, Mediator.Logger).ContinueWith(task => {
                var image = task.Result;
                if(image != null) {
                    //this._thumbnailImage = image;
                    SetThumbnaiImage(image);
                    CacheImageUtility.SaveBitmapSourceToPngAsync(image, ThumbnaiImageFile.FullName, Mediator.Logger);
                    return true;
                } else {
                    return false;
                }
            });
        }

        protected override void CallOnPropertyChangeDisplayItem()
        {
            base.CallOnPropertyChangeDisplayItem();

            var propertyNames = new[] {
                nameof(LoadedNormalVideo),
                nameof(LoadedEconomyVideo),
            };
            CallOnPropertyChange(propertyNames);
        }

        #endregion
    }
}
