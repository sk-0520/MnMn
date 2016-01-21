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
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Delegate;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.ViewModel;

namespace ContentTypeTextNet.MnMn.MnMn.Logic
{
    /// <summary>
    /// ページ取得からあれこれ解析まで一通り頑張る人。
    /// <para>名前似てるけどウェブスクレイピングを明示的に行う処理ではない、名前思いつかなかった。</para>
    /// <para>こまごま public だけど基本的に GetTextAsync のみで使い捨て。</para>
    /// </summary>
    public class PageScraping: DisposeFinalizeBase
    {
        public PageScraping(Mediation mediation, ICreateHttpUserAgent userAgentCreator, string key, ServiceType serviceType)
        {
            Mediation = mediation;
            HttpUserAgent = userAgentCreator.CreateHttpUserAgent();
            Key = key;
            ServiceType = serviceType;
        }

        #region proeprty

        Mediation Mediation { get; set; }

        HttpClient HttpUserAgent { get; set; }

        /// <summary>
        /// 各種URI・パラメータ取得用キー。
        /// </summary>
        public string Key { get; private set; }
        /// <summary>
        /// 使用目的サービス。
        /// </summary>
        public ServiceType ServiceType { get; private set; }

        public Dictionary<string, string> ReplaceUriParameters { get; } = new Dictionary<string, string>();

        public Dictionary<string, string> ReplaceRequestParameters { get; } = new Dictionary<string, string>();

        public Uri Uri { get; private set; }

        public FormUrlEncodedContent Content { get; private set; }

        /// <summary>
        /// 処理終了時に呼び出される。
        /// </summary>
        public Action ExitProcess { get; set; }
        /// <summary>
        /// 処理成功時に呼び出される。
        /// </summary>
        public Action ExitSuccess { get; set; }
        /// <summary>
        /// 処理失敗時に呼び出される。
        /// </summary>
        public Action ExitFailure { get; set; }
        /// <summary>
        /// ステータスコードがおかしかった時に呼び出される。
        /// </summary>
        public JudgeResponsePageScraping JudgeFailureStatusCode { get; set; }
        /// <summary>
        /// ステータスコードが有効な際に呼び出される。
        /// </summary>
        public JudgeResponsePageScraping JudgeSuccessStatusCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool StopHeaderCheck { get; set; }
        /// <summary>
        /// ヘッダ調査。
        /// </summary>
        public JudgeResponsePageScraping JudgeCheckResponseHeaders { get; set; }


        #endregion

        #region function

        void CallExitSuccess()
        {

        }

        void CallExitFailure()
        {

        }

        protected void MakeUri()
        {
            var rawUri = Mediation.GetUri(Key, ReplaceUriParameters, ServiceType);
            var convertedUri = Mediation.ConvertUri(rawUri, ServiceType);
            var uri = new Uri(convertedUri);
            Uri = uri;
        }

        protected void MakeRequestParameter()
        {
            var rawContent = Mediation.GetRequestParameter(Key, ReplaceRequestParameters, ServiceType);
            var convertedContent = Mediation.ConvertRequestParameter((IReadOnlyDictionary<string, string>)rawContent, ServiceType);
            var content = new FormUrlEncodedContent(convertedContent);
            Content = content;
        }

        protected Func<Task<HttpResponseMessage>> GetExecutor(Define.HttpMethod httpMethod)
        {
            var method = new Dictionary<Define.HttpMethod, Func<Task<HttpResponseMessage>>>() {
                { Define.HttpMethod.Get, () => {
                    if(StopHeaderCheck) {
                        return HttpUserAgent.GetAsync(Uri, HttpCompletionOption.ResponseHeadersRead);
                    } else {
                        return HttpUserAgent.GetAsync(Uri);
                    }
                } },
                { Define.HttpMethod.Post, () => HttpUserAgent.PostAsync(Uri, Content) },
            };

            return method[httpMethod];
        }

        protected CheckModel CheckResponseHeaders(HttpResponseMessage response)
        {
            return Mediation.CheckResponseHeader(Uri, response.Headers, ServiceType);
        }

        protected async Task<string> GetTextAsync(HttpResponseMessage response)
        {
            var rawBinary = await response.Content.ReadAsByteArrayAsync();
            var convertedBinary = Mediation.ConvertBinary(Uri, rawBinary, ServiceType);
            var encoding = Mediation.GetEncoding(Uri, convertedBinary, ServiceType);
            var plainText = encoding.GetString(convertedBinary);
            var convertedText = Mediation.ConvertString(Uri, plainText, ServiceType);

            return convertedText;
        }

        /// <summary>
        /// 一連の処理を自動化してテキストデータを返す。
        /// </summary>
        /// <returns></returns>
        public async Task<CheckResultModel<string>> GetResponseTextAsync(Define.HttpMethod httpMethod)
        {
            try {
                MakeUri();
                MakeRequestParameter();

                var executor = GetExecutor(httpMethod);

                using(var response = await executor()) {
                    if(!response.IsSuccessStatusCode) {
                        if(JudgeFailureStatusCode != null) {
                            var judge = JudgeFailureStatusCode(response);
                            if(!judge.IsSuccess) {
                                CallExitFailure();
                                return CheckResultModel.Failure<string>(judge.ToString());
                            }
                        } else {
                            CallExitFailure();
                            return CheckResultModel.Failure<string>(response.IsSuccessStatusCode.ToString());
                        }
                    }

                    if(JudgeSuccessStatusCode != null) {
                        var judge = JudgeSuccessStatusCode(response);
                        if(!judge.IsSuccess) {
                            CallExitFailure();
                            return CheckResultModel.Failure<string>(judge.ToString());
                        }
                    }

                    // ヘッダチェック。
                    if(JudgeCheckResponseHeaders != null) {
                        var judge = JudgeCheckResponseHeaders(response);
                        if(!judge.IsSuccess) {
                            CallExitFailure();
                            return CheckResultModel.Failure<string>(judge.ToString());
                        }
                        if(StopHeaderCheck) {
                            CallExitSuccess();
                            return CheckResultModel.Success(judge.ToString());
                        }
                    }
                    var check = CheckResponseHeaders(response);
                    if(!check.IsSuccess) {
                        CallExitFailure();
                        return CheckResultModel.Failure<string>(check.ToString());
                    }
                    if(StopHeaderCheck) {
                        CallExitSuccess();
                        return CheckResultModel.Success(check.ToString());
                    }

                    var text = await GetTextAsync(response);
                    CallExitSuccess();
                    return CheckResultModel.Success(text);
                }
            } finally {
                if(ExitProcess != null) {
                    ExitProcess();
                }
            }
        }
        #endregion

        #region DisposeFinalizeBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                HttpUserAgent.CancelPendingRequests();
                HttpUserAgent.Dispose();
                HttpUserAgent = null;

                Mediation = null;
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
