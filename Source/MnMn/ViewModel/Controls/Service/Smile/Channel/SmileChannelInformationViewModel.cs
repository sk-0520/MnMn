using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility.UI;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.IF.Control;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.View.Controls;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Channel
{
    public class SmileChannelInformationViewModel : InformationViewModelBase, ISetView
    {
        #region variable

        SourceLoadState _channelLoadState;

        bool _showVideo;

        #endregion

        public SmileChannelInformationViewModel(Mediation mediation, string channelId)
        {
            Mediation = mediation;

            NetworkSetting = Mediation.GetNetworkSetting();
            Logger = Mediation.Logger;

            ChannelId = channelId;

            var dirInfo = Mediation.GetResultFromRequest<DirectoryInfo>(new RequestModel(RequestKind.CacheDirectory, ServiceType.Smile));
            var cachedDirPath = Path.Combine(dirInfo.FullName, Constants.SmileChannelCacheDirectoryName, ChannelId);
            if(Directory.Exists(cachedDirPath)) {
                CacheDirectory = new DirectoryInfo(cachedDirPath);
            } else {
                CacheDirectory = Directory.CreateDirectory(cachedDirPath);
            }

            ThumbnaiImageFile = new FileInfo(Path.Combine(CacheDirectory.FullName, PathUtility.CreateFileName(ChannelId, "png")));

            VideoFinder = new SmileChannelVideoFinderViewModel(Mediation, ChannelId);
        }

        #region property

        Mediation Mediation { get; }

        TabControl TabControl { get; set; }
        TabItem ChannelTabItem { get; set; }
        WebNavigator WebNavigator { get; set; }

        public string ChannelId { get; }

        public string ChannelName => Information?.ChannelName;

        public Uri Uri
        {
            get
            {
                var replaceMap = new StringsModel() {
                    ["channel-id"] = ChannelId,
                };
                var uri = UriUtility.GetConvertedUri(Mediation, SmileMediationKey.channelPage, replaceMap, ServiceType.Smile);
                return uri;
            }
        }
        public string UriString { get { return Uri.OriginalString; } }

        public DirectoryInfo CacheDirectory { get; }
        /// <summary>
        /// サムネイル画像ファイル。
        /// </summary>
        public FileInfo ThumbnaiImageFile { get; }

        public SourceLoadState ChannelLoadState
        {
            get { return this._channelLoadState; }
            set { SetVariableValue(ref this._channelLoadState, value); }
        }

        public bool ShowVideo
        {
            get { return this._showVideo; }
            set
            {
                if(SetVariableValue(ref this._showVideo, value)) {
                    if(ShowVideo) {
                        if(VideoFinder.FinderLoadState == SourceLoadState.None) {
                            VideoFinder.LoadDefaultCacheAsync().ConfigureAwait(false);
                            CallOnPropertyChange(nameof(VideoFinder));
                        }
                    }
                }
            }
        }

        SmileChannelInformationModel Information { get; set; }
        public SmileChannelVideoFinderViewModel VideoFinder { get; }

        /// <summary>
        /// <para>NOTE: 完全に暫定処理</para>
        /// </summary>
        public bool HasPostVideo => Information?.HasPostVideo ?? true;



        #endregion

        #region function

        public Task LoadAsync(CacheSpan userDataCacheSpan, CacheSpan userImageCacheSpan)
        {
            ChannelLoadState = SourceLoadState.SourceLoading;
            return LoadInformationDefaultAsync(userDataCacheSpan).ContinueWith(t => {
                ChannelLoadState = SourceLoadState.InformationLoading;
            }).ContinueWith(t => {
                RefreshWebPage();
            }, TaskScheduler.FromCurrentSynchronizationContext()).ContinueWith(t => {
                ChannelLoadState = SourceLoadState.Completed;
                CallOnPropertyChangeDisplayItem();
            });
        }

        public Task LoadDefaultAsync()
        {
            return LoadAsync(Constants.ServiceSmileChannelDataCacheSpan, Constants.ServiceSmileChannelImageCacheSpan);
        }

        public void RefreshWebPage()
        {
            if(TabControl == null) {
                return;
            }
            if(ChannelTabItem == null) {
                ChannelTabItem = TabControl.ItemContainerGenerator.ContainerFromItem(this) as TabItem;
            }

            if(WebNavigator == null) {
                WebNavigator = UIUtility.FindChildren<WebNavigator>(TabControl).FirstOrDefault();
            }

            if(WebNavigator != null && WebNavigator.HomeSource != Uri) {
                WebNavigator.HomeSource = Uri;
                WebNavigator.Navigate(WebNavigator.HomeSource);
                WebNavigator.CrearHistory();
            }
        }

        #endregion

        #region InformationViewModelBase

        public override string Title => ChannelName;

        protected override Task<bool> LoadInformationCoreAsync(CacheSpan cacheSpan, HttpClient client)
        {
            var channel = new Logic.Service.Smile.Api.V1.Channel(Mediation);
            return channel.LoadInformationAsync(ChannelId).ContinueWith(t => {
                Information = t.Result;
                return Information != null;
            });
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

            var replaceMap = new StringsModel() {
                ["channel-id"] = ChannelId,
            };
            var uri = UriUtility.GetConvertedUri(Mediation, SmileMediationKey.channelThumbnail, replaceMap, ServiceType.Smile);

            return CacheImageUtility.LoadBitmapBinaryDefaultAsync(client, uri, Mediation.Logger).ContinueWith(task => {
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

        protected override void CallOnPropertyChangeDisplayItem()
        {
            base.CallOnPropertyChangeDisplayItem();

            var propertyNames = new[] {
                nameof(Title),
                nameof(ChannelName),
                nameof(Uri),
                nameof(UriString),
            };
            CallOnPropertyChange(propertyNames);

        }

        protected override void Dispose(bool disposing)
        {
            WebNavigator = null;
            ChannelTabItem = null;
            TabControl = null;
            base.Dispose(disposing);
        }

        public override string ToString()
        {
            return ChannelName;
        }

        #endregion

        #region ISetView

        public void SetView(FrameworkElement view)
        {
            TabControl = (TabControl)view;
        }

        #endregion
    }
}
