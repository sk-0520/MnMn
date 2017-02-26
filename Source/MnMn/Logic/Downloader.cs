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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
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
        public static long UnknownDownloadSize { get; } = -1;
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
        public Downloader(Uri downloadUri, ICreateHttpUserAgent userAgentCreator, CancellationToken cancelToken)
            : this(downloadUri, userAgentCreator)
        {
            CancelToken = cancelToken;
        }

        #region property

        public CancellationToken? CancelToken { get; private set; }

        public Uri DownloadUri { get; protected set; }
        protected ICreateHttpUserAgent UserAgentCreator { get; set; }
        protected HttpClient UserAgent { get; set; }

        public long RangeHeadPotision { get; set; }
        public long RangeTailPotision { get; set; } = RangeAll;

        public bool UsingRangeDownload => RangeHeadPotision != 0;

        /// <summary>
        /// 受信時のバッファサイズ。
        /// </summary>
        public int ReceiveBufferSize { get; set; } = DefaultReceiveBufferSize;

        /// <summary>
        /// ダウンロードするデータ量。
        /// </summary>
        public long DownloadTotalSize { get; set; } = UnknownDownloadSize;

        /// <summary>
        /// ダウンロード済みデータ量。
        /// </summary>
        public long DownloadedSize { get; protected set; } = UnknownDownloadSize;

        public bool Canceled { get; protected set; }
        public bool Completed { get; protected set; }

        public HttpContentHeaders ResponseHeaders { get; protected set; }

        #endregion

        #region function

        protected DownloadStartEventArgs OnDownloadStart(DownloadStartType downloadStart, long rangeHeadPosition, long rangeTailPosition)
        {
            var e = new DownloadStartEventArgs(downloadStart, rangeHeadPosition, rangeTailPosition);
            if(DownloadStart != null) {
                DownloadStart(this, e);
            }

            return e;
        }

        protected bool OnDownloading(ArraySegment<byte> data, int counter, long secondsDownlodingSize)
        {
            if(Downloading == null) {
                return false;
            }


            var e = new DownloadingEventArgs(data, counter, secondsDownlodingSize);
            if(CancelToken.HasValue && CancelToken.Value.IsCancellationRequested) {
                e.Cancel = true;
            }
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
            if(UsingRangeDownload) {
                if(RangeTailPotision == RangeAll) {
                    UserAgent.DefaultRequestHeaders.Range = new RangeHeaderValue(RangeHeadPotision, null);
                } else {
                    UserAgent.DefaultRequestHeaders.Range = new RangeHeaderValue(RangeHeadPotision, RangeTailPotision);
                }
            }
        }

        protected virtual Task<Stream> GetStreamAsync()
        {
            //cancel = false;
            UserAgent = UserAgentCreator.CreateHttpUserAgent();
            IfUsingSetRangeHeader();

            var responseTask = CancelToken.HasValue
                ? UserAgent.GetAsync(DownloadUri, HttpCompletionOption.ResponseHeadersRead, CancelToken.Value)
                : UserAgent.GetAsync(DownloadUri, HttpCompletionOption.ResponseHeadersRead)
            ;

            return responseTask.ContinueWith(t => {
                var response = t.Result;
                ResponseHeaders = response.Content.Headers;
                return response.Content.ReadAsStreamAsync();
            }).Unwrap();

            //ResponseHeaders = response.Content.Headers;
            //return response.Content.ReadAsStreamAsync();
            //return UserAgent.GetStreamAsync();
        }

        protected virtual bool CheckHeader()
        {
            return true;
        }

        public virtual Task StartAsync(CancellationToken? cancelToken = null)
        {
            if(cancelToken.HasValue) {
                CancelToken = cancelToken;
            }
            DownloadedSize = 0;
            return GetStreamAsync().ContinueWith(task => {
                if(task.Result == null) {
                    Canceled = true;
                    return;
                }
                if(!CheckHeader()) {
                    Canceled = true;
                    return;
                }
                using(var reader = new BinaryReader(task.Result)) {
                    var downloadStartType = UsingRangeDownload ? DownloadStartType.Range : DownloadStartType.Begin;
                    var downloadStartArgs = OnDownloadStart(downloadStartType, RangeHeadPotision, RangeTailPotision);
                    if(downloadStartArgs.Cancel) {
                        Canceled = true;
                        Completed = downloadStartArgs.Completed;
                        if(Completed) {
                            OnDownloaded();
                        }
                        return;
                    }

                    byte[] buffer = new byte[ReceiveBufferSize];
                    int counter = 1;

                    var secondsStopWatch = new Stopwatch();
                    var secondsBaseTime = TimeSpan.FromSeconds(1);
                    long secondsReadSize = 0;
                    long prevSecondsDownloadingSize = 0;

                    while(true) {
                        if(secondsReadSize == 0) {
                            secondsStopWatch.Restart();
                        }

                        int currentReadSize = 0;

                        int errorCounter = 1;
                        while(true) {
                            try {
                                if(CancelToken.HasValue && CancelToken.Value.IsCancellationRequested) {
                                    Canceled = true;
                                    return;
                                }
                                currentReadSize = reader.Read(buffer, 0, buffer.Length);
                                secondsReadSize += currentReadSize;
                                break;
                            } catch(IOException ex) {
                                var cancel = OnDownloadingError(errorCounter++, ex);
                                if(cancel) {
                                    Canceled = true;
                                    return;
                                }
                            }
                        }
                        if(currentReadSize == 0) {
                            // TODO: 最後かどうかわからんけどとりあえず今のところは最後認識。イベントでも作るべし
                            break;
                        }

                        var elapsedTime = secondsStopWatch.Elapsed;
                        long secondsDownlodingSize = prevSecondsDownloadingSize;

                        if(secondsBaseTime <= elapsedTime) {
                            //DODO: 超過分をきちんと割合比較
                            secondsDownlodingSize = secondsReadSize;
                            prevSecondsDownloadingSize = secondsDownlodingSize;
                            secondsReadSize = 0;
                        }

                        DownloadedSize += currentReadSize;
                        var slice = new ArraySegment<byte>(buffer, 0, currentReadSize);
                        if(OnDownloading(slice, counter++, secondsDownlodingSize)) {
                            Canceled = true;
                            return;
                        }
                    }

                    Completed = true;
                    OnDownloaded();
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
