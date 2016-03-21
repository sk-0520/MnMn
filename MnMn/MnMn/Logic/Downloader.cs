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
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.MnMn.MnMn.Define.Event;
using ContentTypeTextNet.MnMn.MnMn.IF;

namespace ContentTypeTextNet.MnMn.MnMn.Logic
{
    public class Downloader: DisposeFinalizeBase
    {
        #region define

        public static int DefaultReceiveBufferSize { get; } = 8 * 1024;
        public static long UnknownDonwloadSize { get; } = -1;
        public static long RangeAll { get; } = -1;

        #endregion

        #region event

        /// <summary>
        /// タウンロード開始時に呼び出される。
        /// <para>ストリーム作成時。</para>
        /// </summary>
        public event EventHandler<DownloadStartEventArgs> DownloadStart;
        /// <summary>
        /// データ受信時に発生する。
        /// </summary>
        public event EventHandler<DownloadingEventArgs> Downloading;
        /// <summary>
        /// データ受信完了時に発生する。
        /// </summary>
        public event EventHandler<DownloaderEventArgs> Downloaded;
        /// <summary>
        /// ダウンロード失敗時に呼び出される。
        /// <para>キャンセルが立てば処理終了。</para>
        /// </summary>
        public event EventHandler<DownloadingErrorEventArgs> DownloadingError;

        #endregion

        public Downloader(Uri downloadUri, ICreateHttpUserAgent userAgentCreator)
        {
            DownloadUri = downloadUri;
            UserAgentCreator = userAgentCreator;
        }

        #region property

        public Uri DownloadUri { get; protected set; }
        protected ICreateHttpUserAgent UserAgentCreator { get; set; }
        protected HttpClient UserAgent { get; set; }

        public long RangeHeadPotision { get; set; }
        public long RangeTailPotision { get; set; } = RangeAll;

        public bool UsingRangeDonwload => RangeHeadPotision != 0;

        /// <summary>
        /// 受信時のバッファサイズ。
        /// </summary>
        public int ReceiveBufferSize { get; set; } = DefaultReceiveBufferSize;

        /// <summary>
        /// ダウンロードするデータ量。
        /// </summary>
        public long DownloadTotalSize { get; set; } = UnknownDonwloadSize;

        /// <summary>
        /// ダウンロード済みデータ量。
        /// </summary>
        public long DownloadedSize { get; protected set; } = UnknownDonwloadSize;

        public bool Cancled { get; protected set; }
        public bool Completed { get; protected set; }

        #endregion

        #region function

        protected bool OnDownloadStart(DownloadStartType downloadStart, long rangeHeadPosition, long rangeTailPosition)
        {
            if(DownloadStart == null) {
                return false;
            }

            var e = new DownloadStartEventArgs(downloadStart, rangeHeadPosition, rangeTailPosition);
            DownloadStart(this, e);

            return e.Cancel;
        }

        protected bool OnDonwloading(ArraySegment<byte> data, int counter)
        {
            if(Downloading == null) {
                return false;
            }

            var e = new DownloadingEventArgs(data, counter);
            Downloading(this, e);

            return e.Cancel;
        }

        protected void OnDownloaded()
        {
            if(Downloaded == null) {
                return;
            }

            var e = new DownloaderEventArgs();
            Downloaded(this, e);
        }

        protected bool OnDownloadingError(int counter, Exception ex)
        {
            if(DownloadingError == null) {
                return true;
            }

            var e = new DownloadingErrorEventArgs(counter, ex);
            DownloadingError(this, e);

            return e.Cancel;
        }

        protected void IfUsingSetRangeHeader()
        {
            if(UsingRangeDonwload) {
                if(RangeTailPotision == RangeAll) {
                    UserAgent.DefaultRequestHeaders.Range = new RangeHeaderValue(RangeHeadPotision, null);
                } else {
                    UserAgent.DefaultRequestHeaders.Range = new RangeHeaderValue(RangeHeadPotision, RangeTailPotision);
                }
            }
        }

        protected virtual Task<Stream> GetStreamAsync(out bool cancel)
        {
            cancel = false;
            UserAgent = UserAgentCreator.CreateHttpUserAgent();
            IfUsingSetRangeHeader();
            return UserAgent.GetStreamAsync(DownloadUri);
        }

        public virtual Task StartAsync()
        {
            bool isCancel;
            return GetStreamAsync(out isCancel).ContinueWith(task => {
                if(isCancel) {
                    Cancled = true;
                    return;
                }
                using(var reader = new BinaryReader(task.Result)) {
                    var downloadStartType = UsingRangeDonwload ? DownloadStartType.Range : DownloadStartType.Begin;
                    if(OnDownloadStart(downloadStartType, RangeHeadPotision, RangeTailPotision)) {
                        Cancled = true;
                        return;
                    }

                    byte[] buffer = new byte[ReceiveBufferSize];
                    int counter = 1;

                    while(true) {
                        int readSize = 0;

                        int errorCounter = 1;
                        while(true) {
                            try {
                                readSize = reader.Read(buffer, 0, buffer.Length);
                                break;
                            } catch(IOException ex) {
                                var cancel = OnDownloadingError(errorCounter++, ex);
                                if(cancel) {
                                    Cancled = true;
                                    return;
                                }
                            }
                        }
                        if(readSize == 0) {
                            // TODO: 最後かどうかわからんけどとりあえず今のところは最後認識。イベントでも作るべし
                            break;
                        }

                        DownloadedSize += readSize;
                        var slice = new ArraySegment<byte>(buffer, 0, readSize);
                        if(OnDonwloading(slice, counter++)) {
                            Cancled = true;
                            return;
                        }
                    }

                    Completed = true;
                }
            });
        }

        #endregion

        #region DisposeFinalizeBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(UserAgent != null) {
                    UserAgent.CancelPendingRequests();
                    UserAgent.Dispose();
                    UserAgent = null;
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
