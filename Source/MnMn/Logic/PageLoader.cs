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
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.Library.Bridging.Model;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Delegate;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.ViewModel;

namespace ContentTypeTextNet.MnMn.MnMn.Logic
{
    /// <summary>
    /// ページ取得からあれこれ解析まで一通り頑張る人。
    /// <para>こまごま public だけど基本的に <see cref="GetResponseTextAsync"/> のみで使い捨て。</para>
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

        //public PageLoader(Mediation mediation, HttpUserAgentHost host, string key, ServiceType serviceType)
        //{
        //    Mediation = mediation;
        //    HttpUserAgent = host.Client;
        //    Key = key;
        //    ServiceType = serviceType;
        //    OwnershipUA = false;
        //}

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
        public Dictionary<string, string> ReplaceRequestHeaders { get; } = new Dictionary<string, string>();
        public Dictionary<string, string> ReplaceRequestParameters { get; } = new Dictionary<string, string>();

        public Uri Uri { get; private set; }

        protected IDictionary<string, string> Headers { get; private set; }

        protected FormUrlEncodedContent PlainContent { get; private set; }
        protected StringContent MappingContent { get; private set; }

        /// <summary>
        /// リクエストパラメータに使用する形式。
        /// </summary>
        protected ParameterType ParameterType { get; private set; }

        /// <summary>
        /// URI構築処理を用いず指定URIの使用を強制する。
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

        void OnExitProcess()
        {
            if(ExitProcess != null) {
                ExitProcess();
            }
        }

        void OnExitSuccess()
        {
            if(ExitSuccess != null) {
                ExitSuccess();
            }
        }

        void OnExitFailure()
        {
            if(ExitFailure != null) {
                ExitFailure();
            }
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
            Mediation.Logger.Trace($"[{ServiceType}] {nameof(Key)}: {Key}, {nameof(Uri)}: {Uri}, {nameof(rawUri.RequestParameterType)}: {rawUri.RequestParameterType}");
        }

        protected void MakeRequestHeader()
        {
            var rawHeader = Mediation.GetRequestHeader(Key, ReplaceRequestHeaders, ServiceType);
            var convertedHeader = Mediation.ConvertRequestHeader(Key, (IReadOnlyDictionary<string, string>)rawHeader, ServiceType);
            Headers = (Dictionary<string, string>)convertedHeader;

            Mediation.Logger.Trace($"[{ServiceType}] {nameof(Key)}: {Key}, {nameof(ParameterType)}: {ParameterType}, count: {Headers.Count}", Headers.Any() ? string.Join(Environment.NewLine, Headers.OrderBy(p => p.Key).Select(p => $"{p.Key}={p.Value}")) : null);
        }

        protected void MakeRequestParameter()
        {
            switch(ParameterType) {
                case ParameterType.Plain: {
                        var rawContent = Mediation.GetRequestParameter(Key, ReplaceRequestParameters, ServiceType);
                        var singleContent = Mediation.ConvertRequestParameter((IReadOnlyDictionary<string, string>)rawContent, ServiceType);
                        var multiContents = singleContent
                            .Where(p => p.Value.Any(c => c == MultiStrings.defaultSeparator))
                            .ToArray()
                        ;
                        var hasMultiValue = multiContents.Any();
                        if(hasMultiValue) {
                            foreach(var key in multiContents.Select(p => p.Key)) {
                                singleContent.Remove(key);
                            }
                        }
                        var convertedContent = singleContent.ToList();
                        if(hasMultiValue) {
                            foreach(var pair in multiContents) {
                                var ms = new MultiStrings(pair.Value);
                                var pairs = ms.Values.Select(s => new KeyValuePair<string, string>(pair.Key, s));
                                convertedContent.AddRange(pairs);
                            }
                        }
                        Mediation.Logger.Trace($"[{ServiceType}] {nameof(Key)}: {Key}, {nameof(ParameterType)}: {ParameterType}, count: {convertedContent.Count}", convertedContent.Any() ? string.Join(Environment.NewLine, convertedContent.OrderBy(p => p.Key).Select(p => $"{p.Key}={p.Value}")) : null);
                        var content = new FormUrlEncodedContent(convertedContent);
                        PlainContent = content;
                    }
                    break;

                case ParameterType.Mapping: {
                        var mappingResult = Mediation.GetRequestMapping(Key, ReplaceRequestParameters, ServiceType);
                        var convertedContent = Mediation.ConvertRequestMapping(mappingResult.Result, ServiceType);
                        Mediation.Logger.Trace($"[{ServiceType}] {nameof(Key)}: {Key}, {nameof(ParameterType)}: {ParameterType}, byte: {convertedContent.Length}", convertedContent);
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
            HttpUserAgent.DefaultRequestHeaders.Clear();
            foreach(var pair in Headers) {
                HttpUserAgent.DefaultRequestHeaders.Add(pair.Key, pair.Value);
            }

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
            using(var stream = GlobalManager.MemoryStream.GetStreamWidthAutoTag()) {
                using(var responseStream = await response.Content.ReadAsStreamAsync()) {
                    responseStream.CopyTo(stream);
                    stream.Position = 0;
                }

                Mediation.ConvertBinary(Uri, stream, ServiceType);
                var encoding = Mediation.GetEncoding(Uri, stream, ServiceType);

                var binary = stream.GetBuffer();
                var length = (int)stream.Length;

                var plainText = encoding.GetString(binary, 0, length);

                var convertedText = Mediation.ConvertString(Uri, plainText, ServiceType);

                return convertedText;
            }
        }

        /// <summary>
        /// 一連の処理を自動化してテキストデータを返す。
        /// </summary>
        /// <returns></returns>
        public async Task<CheckResultModel<string>> GetResponseTextAsync(Define.PageLoaderMethod httpMethod)
        {
            try {
                MakeUri();
                MakeRequestHeader();
                MakeRequestParameter();

                var executor = GetExecutor(httpMethod);

                using(var response = await executor()) {
                    Mediation.Logger.Trace($"[{ServiceType}] {nameof(Key)}: {Key}, {response.StatusCode}", response.ToString());
                    if(!response.IsSuccessStatusCode) {
                        if(JudgeFailureStatusCode != null) {
                            var judge = JudgeFailureStatusCode(response);
                            if(!judge.IsSuccess) {
                                OnExitFailure();
                                return CheckResultModel.Failure<string>(judge.ToString());
                            }
                        } else {
                            OnExitFailure();
                            return CheckResultModel.Failure<string>(response.ToString());
                        }
                    }

                    if(JudgeSuccessStatusCode != null) {
                        var judge = JudgeSuccessStatusCode(response);
                        if(!judge.IsSuccess) {
                            OnExitFailure();
                            return CheckResultModel.Failure<string>(judge.ToString());
                        }
                    }

                    // ヘッダチェック。
                    if(JudgeCheckResponseHeaders != null) {
                        var judge = JudgeCheckResponseHeaders(response);
                        if(!judge.IsSuccess) {
                            OnExitFailure();
                            return CheckResultModel.Failure<string>(judge.ToString());
                        }
                        if(HeaderCheckOnly) {
                            OnExitSuccess();
                            return CheckResultModel.Success(judge.ToString());
                        }
                    }
                    var check = CheckResponseHeaders(response);
                    if(!check.IsSuccess) {
                        OnExitFailure();
                        return CheckResultModel.Failure<string>(check.ToString());
                    }
                    if(HeaderCheckOnly) {
                        OnExitSuccess();
                        return CheckResultModel.Success(check.ToString());
                    }

                    var text = await GetTextAsync(response);
                    OnExitSuccess();
                    return CheckResultModel.Success(text);
                }
            } finally {
                OnExitProcess();
            }
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
