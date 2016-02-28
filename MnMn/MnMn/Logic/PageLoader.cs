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
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Delegate;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.ViewModel;

namespace ContentTypeTextNet.MnMn.MnMn.Logic
{
    /// <summary>
    /// ページ取得からあれこれ解析まで一通り頑張る人。
    /// <para>こまごま public だけど基本的に GetTextAsync のみで使い捨て。</para>
    /// </summary>
    public class PageLoader: DisposeFinalizeBase
    {
        public PageLoader(Mediation mediation, ICreateHttpUserAgent userAgentCreator, string key, ServiceType serviceType)
        {
            Mediation = mediation;
            HttpUserAgent = userAgentCreator.CreateHttpUserAgent();
            Key = key;
            ServiceType = serviceType;
            OwnershipUA = true;
        }

        public PageLoader(Mediation mediation, HttpUserAgentHost host, string key, ServiceType serviceType)
        {
            Mediation = mediation;
            HttpUserAgent = host.Client;
            Key = key;
            ServiceType = serviceType;
            OwnershipUA = false;
        }

        #region proeprty

        /// <summary>
        /// UAは本オブジェクトの所有物か。
        /// </summary>
        bool OwnershipUA { get; set; }

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

        public FormUrlEncodedContent PlainContent { get; private set; }
        public StringContent MappingContent { get; private set; }

        /// <summary>
        /// リクエストパラメータに使用する形式。
        /// </summary>
        public ParameterType ParameterType { get; private set; }

        /// <summary>
        /// URI構築処理を用いず指定URIの仕様を強制する。
        /// </summary>
        public Uri ForceUri { get; set; }

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
        public bool HeaderCheckOnly { get; set; }
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
            ParameterType = rawUri.RequestParameterType;
            Uri = RestrictUtility.IsNull(
                ForceUri, () => {
                    var convertedUri = Mediation.ConvertUri(rawUri.Uri, ServiceType);
                    return new Uri(convertedUri);
                }, 
                uri => uri
            );
            Debug.WriteLine($"{nameof(Uri)}-> {Uri}, {nameof(rawUri.RequestParameterType)} -> {rawUri.RequestParameterType}");
        }

        protected void MakeRequestParameter()
        {
            switch(ParameterType) {
                case ParameterType.Plain: {
                        var rawContent = Mediation.GetRequestParameter(Key, ReplaceRequestParameters, ServiceType);
                        var convertedContent = Mediation.ConvertRequestParameter((IReadOnlyDictionary<string, string>)rawContent, ServiceType);
                        var content = new FormUrlEncodedContent(convertedContent);
                        PlainContent = content;
                    }
                    break;

                case ParameterType.Mapping: {
                        var mappingResult = Mediation.GetRequestMapping(Key, ReplaceRequestParameters, ServiceType);
                        var convertedContent = Mediation.ConvertRequestMapping(mappingResult.Result, ServiceType);
                        Debug.WriteLine("request param");
                        Debug.WriteLine(convertedContent);
                        MappingContent = new StringContent(convertedContent);
                        if(!string.IsNullOrWhiteSpace(mappingResult.ContentType)) {
                            MappingContent.Headers.ContentType = new MediaTypeHeaderValue(mappingResult.ContentType);
                        }
                    }
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        protected Func<Task<HttpResponseMessage>> GetExecutor(Define.PageLoaderMethod httpMethod)
        {
            var method = new Dictionary<Define.PageLoaderMethod, Func<Task<HttpResponseMessage>>>() {
                { Define.PageLoaderMethod.Get, () => {
                    if(HeaderCheckOnly) {
                        return HttpUserAgent.GetAsync(Uri, HttpCompletionOption.ResponseHeadersRead);
                    } else {
                        return HttpUserAgent.GetAsync(Uri);
                    }
                } },
                { Define.PageLoaderMethod.Post, () => ParameterType == ParameterType.Plain ? HttpUserAgent.PostAsync(Uri, PlainContent): HttpUserAgent.PostAsync(Uri, MappingContent) },
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
        public async Task<CheckResultModel<string>> GetResponseTextAsync(Define.PageLoaderMethod httpMethod)
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
                            return CheckResultModel.Failure<string>(response.ToString());
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
                        if(HeaderCheckOnly) {
                            CallExitSuccess();
                            return CheckResultModel.Success(judge.ToString());
                        }
                    }
                    var check = CheckResponseHeaders(response);
                    if(!check.IsSuccess) {
                        CallExitFailure();
                        return CheckResultModel.Failure<string>(check.ToString());
                    }
                    if(HeaderCheckOnly) {
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

        private void Count(Func<object, bool> p)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region DisposeFinalizeBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(OwnershipUA) {
                    //HttpUserAgent.CancelPendingRequests();
                    HttpUserAgent.Dispose();
                }
                HttpUserAgent = null;

                Mediation = null;
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
