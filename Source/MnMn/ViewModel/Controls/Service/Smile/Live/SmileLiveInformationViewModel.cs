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
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Live;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Live.Api;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Live;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Live.Raw;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw.Feed;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Live;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Live.Player;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Live
{
    public class SmileLiveInformationViewModel: InformationViewModelBase
    {
        #region variable

        bool _isPlaying;

        #endregion

        SmileLiveInformationViewModel(Mediation mediation)
        {
            Mediation = mediation;

            Setting = Mediation.GetResultFromRequest<SmileLiveSettingModel>(new RequestModel(RequestKind.Setting, ServiceType.SmileLive));
        }

        public SmileLiveInformationViewModel(Mediation mediation, FeedSmileLiveItemModel feed)
            : this(mediation)
        {
            Feed = feed;
            FeedDetail = SmileLiveFeedUtility.ConvertRawDescription(Feed.Description);

            InformationSource = SmileLiveInformationSource.Feed;

            Initialize();
        }

        #region property

        Mediation Mediation { get; }
        SmileLiveSettingModel Setting { get; }

        public SmileLiveInformationSource InformationSource { get; protected set; }

        RawSmileLiveGetPlayerStatusModel PlayerStatus { get; set; }

        FeedSmileLiveItemModel Feed { get; }
        RawSmileLiveFeedDetailModel FeedDetail { get; set; }

        #region file

        /// <summary>
        /// キャッシュディレクトリ。
        /// </summary>
        public DirectoryInfo CacheDirectory { get; private set; }

        /// <summary>
        /// サムネイル画像ファイル。
        /// </summary>
        public FileInfo ThumbnaiImageFile { get; private set; }

        #endregion

        public bool IsPlaying
        {
            get { return this._isPlaying; }
            set { SetVariableValue(ref this._isPlaying, value); }
        }

        public string Id
        {
            get
            {
                switch(InformationSource) {
                    case SmileLiveInformationSource.Feed:
                        return Feed.Guid.Uri;

                    case SmileLiveInformationSource.GetPlayerStatus:
                        return PlayerStatus.Stream.Id;

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
                    case SmileLiveInformationSource.Feed:
                        return Feed.Title;

                    case SmileLiveInformationSource.GetPlayerStatus:
                        return PlayerStatus.Stream.Title;

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
                    case SmileLiveInformationSource.Feed:
                        return RawValueUtility.ConvertUri(Feed.Link);

                    case SmileLiveInformationSource.GetPlayerStatus:
                        return SmileLiveGetPlayerStatusUtility.GetWatchUrl(PlayerStatus);

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
                    case SmileLiveInformationSource.Feed:
                        return RawValueUtility.ConvertUri(Feed.Thumbnail.Url);

                    case SmileLiveInformationSource.GetPlayerStatus:
                        return RawValueUtility.ConvertUri(PlayerStatus.Stream.PictureUrl);

                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public string UserName
        {
            get
            {
                switch(InformationSource) {
                    case SmileLiveInformationSource.Feed:
                        return Feed.OwnerName;

                    case SmileLiveInformationSource.GetPlayerStatus:
                        return PlayerStatus.Stream.OwnerName;

                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public DateTime ReleaseDate
        {
            get
            {
                switch(InformationSource) {
                    case SmileLiveInformationSource.Feed:
                        return Feed.PubDate;

                    case SmileLiveInformationSource.GetPlayerStatus:
                        return RawValueUtility.ConvertUnixTime(PlayerStatus.Stream.OpenTime);

                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public int ViewCounter
        {
            get
            {
                switch(InformationSource) {
                    case SmileLiveInformationSource.Feed:
                        return RawValueUtility.ConvertInteger(Feed.View);

                    case SmileLiveInformationSource.GetPlayerStatus:
                        return RawValueUtility.ConvertInteger(PlayerStatus.Stream.WatchCount);

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
                    case SmileLiveInformationSource.Feed:
                        return RawValueUtility.ConvertInteger(Feed.NumRes);

                    case SmileLiveInformationSource.GetPlayerStatus:
                        return RawValueUtility.ConvertInteger(PlayerStatus.Stream.CommentCount);

                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public SmileLiveType Type
        {
            get
            {
                switch(InformationSource) {
                    case SmileLiveInformationSource.Feed:
                        return SmileLiveFeedUtility.ConvertType(Feed.Type);

                    case SmileLiveInformationSource.GetPlayerStatus:
                        return SmileLiveGetPlayerStatusUtility.ConvertType(PlayerStatus.Stream.ProviderType);

                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public bool MemberOnly
        {
            get
            {
                switch(InformationSource) {
                    case SmileLiveInformationSource.Feed:
                        return RawValueUtility.ConvertBoolean(Feed.MemberOnly);

                    case SmileLiveInformationSource.GetPlayerStatus:
                        if(Feed != null) {
                            return RawValueUtility.ConvertBoolean(Feed.MemberOnly);
                        }
                        //TODO: どうしよっかねぇ。
                        return false;

                    default:
                        throw new NotImplementedException();
                }
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
                    o => { }
                );
            }
        }

        #endregion

        #region function

        void Initialize()
        {
            var cacheBaseDir = Mediation.GetResultFromRequest<DirectoryInfo>(new RequestModel(RequestKind.CacheDirectory, ServiceType.SmileLive));
            CacheDirectory = Directory.CreateDirectory(Path.Combine(cacheBaseDir.FullName, Id));

            ThumbnaiImageFile = new FileInfo(Path.Combine(CacheDirectory.FullName, PathUtility.CreateFileName(Id, "png")));
            //WatchPageHtmlFile = new FileInfo(Path.Combine(CacheDirectory.FullName, PathUtility.CreateFileName(Id, "html")));
        }

        public Task OpenVideoDefaultAsync(bool forceEconomy)
        {
            return OpenVideoFromOpenParameterAsync(forceEconomy, Setting.Execute.OpenMode, Setting.Execute.OpenPlayerInNewWindow);
        }

        Task OpenVideoFromOpenParameterAsync(bool forceEconomy, ExecuteOrOpenMode openMode, bool openPlayerInNewWindow)
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

        public Task OpenVideoPlayerAsync(bool forceEconomy, bool openPlayerInNewWindow)
        {
            if(IsPlaying) {
                Mediation.Request(new ShowViewRequestModel(RequestKind.ShowView, ServiceType.SmileLive, this, ShowViewState.Foreground));
                return Task.CompletedTask;
            } else {
                IsPlaying = true;

                var vm = new SmileLivePlayerViewModel(Mediation);
                Mediation.Request(new ShowViewRequestModel(RequestKind.ShowView, ServiceType.SmileLive, vm, ShowViewState.Foreground));

                var task = vm.LoadAsync(this, forceEconomy, Constants.ServiceSmileVideoThumbCacheSpan, Constants.ServiceSmileVideoImageCacheSpan);
                return task;
            }
        }

        public Task OpenVideoBrowserAsync(bool forceEconomy)
        {
            ShellUtility.OpenUriInSystemBrowser(WatchUrl, Mediation.Logger);

            return Task.CompletedTask;
        }

        public Task OpenVideoLauncherAsync(bool forceEconomy)
        {
            var args = SmileLiveInformationUtility.MakeLauncherParameter(this, Setting.Execute.LauncherParameter);
            ShellUtility.ExecuteCommand(Setting.Execute.LauncherPath, args, Mediation.Logger);

            return Task.CompletedTask;
        }

        #endregion

        #region InformationViewModelBase

        protected override Task<bool> LoadInformationCoreAsync(CacheSpan cacheSpan, HttpClient client)
        {
            return Task.FromResult(true);
            // 全部取得するのまずいんじゃないすかねぇ。。。
            //var playerStatus = new GetPlayerStatus(Mediation);
            //return SmileLiveInformationUtility.LoadGetPlayerStatusAsync(Mediation, Id, cacheSpan).ContinueWith(t => {
            //    var model = t.Result;
            //    if(!SmileLiveGetPlayerStatusUtility.IsSuccessResponse(model)) {
            //        return false;
            //    }

            //    PlayerStatus = model;
            //    InformationSource = SmileLiveInformationSource.GetPlayerStatus;

            //    return true;
            //});
        }

        protected override Task<bool> LoadThumbnaiImageCoreAsync(CacheSpan cacheSpan, HttpClient client)
        {
            ThumbnailLoadState = LoadState.Preparation;

            if(CacheImageUtility.ExistImage(ThumbnaiImageFile.FullName, cacheSpan)) {
                ThumbnailLoadState = LoadState.Loading;
                var cacheImage = CacheImageUtility.LoadBitmapBinary(ThumbnaiImageFile.FullName);
                SetThumbnaiImage(cacheImage);
                return Task.FromResult(true);
            }

            ThumbnailLoadState = LoadState.Loading;
            return CacheImageUtility.LoadBitmapBinaryDefaultAsync(client, ThumbnailUri, Mediation.Logger).ContinueWith(task => {
                var image = task.Result;
                if(image != null) {
                    SetThumbnaiImage(image);
                    CacheImageUtility.SaveBitmapSourceToPngAsync(image, ThumbnaiImageFile.FullName, Mediation.Logger);
                    return true;
                } else {
                    return false;
                }
            });
        }

        #endregion
    }
}
