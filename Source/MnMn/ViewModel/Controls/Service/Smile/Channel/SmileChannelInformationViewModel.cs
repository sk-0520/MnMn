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

        bool _showVideoTabItem;

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

        public string ChannelId { get; }

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

        public bool ShowVideoTabItem
        {
            get { return this._showVideoTabItem; }
            set
            {
                if(SetVariableValue(ref this._showVideoTabItem, value)) {
                    if(ShowVideoTabItem) {
                        VideoFinder.LoadDefaultCacheAsync().ConfigureAwait(false);
                        CallOnPropertyChange(nameof(VideoFinder));
                    }
                }
            }
        }

        SmileChannelInformationModel Information { get; set; }
        public SmileChannelVideoFinderViewModel VideoFinder { get;  }

        #endregion

        #region function

        public Task LoadAsync(CacheSpan userDataCacheSpan, CacheSpan userImageCacheSpan)
        {
            ChannelLoadState = SourceLoadState.SourceLoading;
            return LoadInformationDefaultAsync(userDataCacheSpan).ContinueWith(t => {
                ChannelLoadState = SourceLoadState.InformationLoading;
            }).ContinueWith(t => {
                var tabItem = TabControl.ItemContainerGenerator.ContainerFromItem(this) as TabItem;
                var web = UIUtility.FindChildren<WebNavigator>(TabControl).FirstOrDefault();
                if(web != null) {
                    if(web.IsEmptyContent) {
                        web.Navigate(web.HomeSource);
                    }
                }
            }, TaskScheduler.FromCurrentSynchronizationContext()).ContinueWith(t => {
                ChannelLoadState = SourceLoadState.Completed;
                CallOnPropertyChangeDisplayItem();
            });
        }

        public Task LoadDefaultAsync()
        {
            return LoadAsync(Constants.ServiceSmileChannelDataCacheSpan, Constants.ServiceSmileChannelImageCacheSpan);
        }

        #endregion

        #region InformationViewModelBase

        public override string Title => Information?.ChannelName;

        protected override Task<bool> LoadInformationCoreAsync(CacheSpan cacheSpan, HttpClient client)
        {
            var channel = new Logic.Service.Smile.Api.V1.Channel(Mediation);
            return channel.LoadInformation(ChannelId).ContinueWith(t => {
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
                nameof(Uri),
                nameof(UriString),
            };
            CallOnPropertyChange(propertyNames);

        }

        protected override void Dispose(bool disposing)
        {
            TabControl = null;
            base.Dispose(disposing);
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
