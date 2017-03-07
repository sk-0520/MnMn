using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Raw;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Market
{
    /// <summary>
    /// TODO: <see cref="InformationViewModelBase"/>使うべきだったか。
    /// </summary>
    public class SmileMarketVideoRelationItemViewModel: SingleModelWrapperViewModelBase<SmileMarketVideoItemModel>
    {
        #region property

        LoadState _thumbnailLoadState;
        BitmapSource _thumbnailImage;

        #endregion

        public SmileMarketVideoRelationItemViewModel(Mediation mediation, SmileMarketVideoItemModel model)
            : base(model)
        {
            Mediation = mediation;
        }

        #region property

        Mediation Mediation { get; set; }

        public string Title => Model.Title;

        public Uri MarketUri => MakeUri(Model.MarketUrl);
        public Uri CashRegisterUri => MakeUri(Model.CashRegisterUrl);
        public Uri ThumbnailUri => MakeUri(Model.ThumbnailUrl);

        public string Id => MarketUri?.Segments.Last();

        public LoadState ThumbnailLoadState
        {
            get { return this._thumbnailLoadState; }
            set { SetVariableValue(ref this._thumbnailLoadState, value); }
        }

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

        public object ThumbnaiImageFile { get; private set; }

        #endregion

        #region function

        Uri MakeUri(string uri)
        {
            Uri outUri;
            if(Uri.TryCreate(uri, UriKind.RelativeOrAbsolute, out outUri)) {
                return outUri;
            }

            return null;
        }

        protected Task<bool> LoadThumbnaiImageCoreAsync(CacheSpan cacheSpan, HttpClient client)
        {
            ThumbnailLoadState = LoadState.Preparation;

            var dirInfo = Mediation.GetResultFromRequest<DirectoryInfo>(new RequestModel(RequestKind.CacheDirectory, ServiceType.Smile));
            var cachedDirPath = Path.Combine(dirInfo.FullName, Constants.SmileMarketCacheDirectoryName);
            var marketDir = Directory.CreateDirectory(cachedDirPath);
            var thumbnaiImageFilePath = Path.Combine(marketDir.FullName, PathUtility.AppendExtension(PathUtility.ToSafeNameDefault(Id), "png"));

            if(CacheImageUtility.ExistImage(thumbnaiImageFilePath, cacheSpan)) {
                ThumbnailLoadState = LoadState.Loading;
                var cacheImage = CacheImageUtility.LoadBitmapBinary(thumbnaiImageFilePath);
                SetThumbnaiImage(cacheImage);
                //ThumbnailLoadState = LoadState.Loaded;
                return Task.FromResult(true);
            }

            ThumbnailLoadState = LoadState.Loading;
            return CacheImageUtility.LoadBitmapBinaryDefaultAsync(client, ThumbnailUri, Mediation.Logger).ContinueWith(task => {
                var image = task.Result;
                if(image != null) {
                    //this._thumbnailImage = image;
                    SetThumbnaiImage(image);
                    CacheImageUtility.SaveBitmapSourceToPngAsync(image, thumbnaiImageFilePath, Mediation.Logger);
                    return true;
                } else {
                    return false;
                }
            });
        }

        public Task LoadThumbnaiImageAsync(CacheSpan cacheSpan, HttpClient client)
        {
            ThumbnailLoadState = LoadState.Preparation;

            return LoadThumbnaiImageCoreAsync(cacheSpan, client).ContinueWith(t => {
                if(t.IsFaulted) {
                    ThumbnailLoadState = LoadState.Failure;
                    return;
                }

                var result = t.Result;
                if(result) {
                    ThumbnailLoadState = LoadState.Loaded;
                } else {
                    ThumbnailLoadState = LoadState.Failure;
                }
            });
        }

        protected void SetThumbnaiImage(BitmapSource image)
        {
            this._thumbnailImage = image;
        }

        #endregion
    }
}
