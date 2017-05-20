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
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly;
using ContentTypeTextNet.MnMn.MnMn.Logic;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls
{
    public abstract class InformationViewModelBase: ViewModelBase
    {
        #region variable

        LoadState _thumbnailLoadState;
        LoadState _informationLoadState;

        BitmapSource _thumbnailImage;

        int _referenceCount;

        #endregion

        #region property

        protected IReadOnlyNetworkSetting NetworkSetting { get; set; }
        protected ILogger Logger { get; set; }

        /// <summary>
        /// キャッシュ上の参照カウンタ。
        /// </summary>
        public int ReferenceCount
        {
            get { return this._referenceCount; }
            set { SetVariableValue(ref this._referenceCount, value); }
        }

        public abstract string Title { get; }

        /// <summary>
        /// サムネイル読み込み状態。
        /// </summary>
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

        /// <summary>
        /// 動画情報読込状態。
        /// <para>使ってないと思ったらいたるところで使ってて困った。</para>
        /// </summary>
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

        #endregion

        #region function

        /// <summary>
        /// <see cref="ReferenceCount"/>を加算。
        /// </summary>
        public void IncrementReference()
        {
            var prev = ReferenceCount;

            ReferenceCount = RangeUtility.Increment(ReferenceCount);

            Debug.WriteLine($"[+] {prev} -> {ReferenceCount}: {Title}");
        }

        /// <summary>
        /// <see cref="ReferenceCount"/>を減算。
        /// </summary>
        public void DecrementReference()
        {
            var prev = ReferenceCount;

            if(ReferenceCount > 0) {
                ReferenceCount -= 1;
            }

            Debug.WriteLine($"[-] {prev} -> {ReferenceCount}: {Title}");
        }

        protected abstract Task<bool> LoadInformationCoreAsync(CacheSpan cacheSpan, HttpClient client);

        public Task LoadInformationAsync(CacheSpan cacheSpan, HttpClient client)
        {
            InformationLoadState = LoadState.Preparation;

            InformationLoadState = LoadState.Loading;

            return LoadInformationCoreAsync(cacheSpan, client).ContinueWith(t => {
                if(t.IsFaulted) {
                    InformationLoadState = LoadState.Failure;
                    return;
                }

                var result = t.Result;
                if(result) {
                    InformationLoadState = LoadState.Loaded;
                } else {
                    InformationLoadState = LoadState.Failure;
                }
            });
        }

        public Task LoadInformationDefaultAsync(CacheSpan cacheSpan)
        {
            var host = new HttpUserAgentHost(NetworkSetting, Logger);
            var userAgent = host.CreateHttpUserAgent();
            return LoadInformationAsync(cacheSpan, userAgent).ContinueWith(_ => {
                userAgent.Dispose();
                host.Dispose();
            });
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="cacheSpan"></param>
        /// <param name="client"></param>
        /// <returns>読み込めたかどうか。</returns>
        protected abstract Task<bool> LoadThumbnaiImageCoreAsync(CacheSpan cacheSpan, HttpClient client);

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

        public Task LoadThumbnaiImageDefaultAsync(CacheSpan cacheSpan)
        {
            var host = new HttpUserAgentHost(NetworkSetting, Logger);
            var userAgent = host.CreateHttpUserAgent();
            return LoadThumbnaiImageAsync(cacheSpan, userAgent).ContinueWith(_ => {
                userAgent.Dispose();
                host.Dispose();
            });
        }

        protected void SetThumbnaiImage(BitmapSource image)
        {
            this._thumbnailImage = image;
        }

        #endregion
    }
}
