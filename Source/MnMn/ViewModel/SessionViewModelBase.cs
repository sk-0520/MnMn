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
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.Setting;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.View.Controls;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel
{
    /// <summary>
    /// セッションを管理する。
    /// <para>基本的なログイン・ログアウトをサポート。</para>
    /// </summary>
    public abstract class SessionViewModelBase: ViewModelBase, IHttpUserAgentCreator
    {
        #region define

        static DateTime NotLoginTime { get; } = DateTime.MaxValue;

        #endregion

        #region variable

        LoginState _loginState;

        DateTime _lastLoginTime = NotLoginTime;

        #endregion

        public SessionViewModelBase(Mediation mediation)
        {
            Mediation = mediation;
            NetworkSetting = Mediation.GetNetworkSetting();
        }

        #region property

        /// <summary>
        /// 内部連携。
        /// </summary>
        protected Mediation Mediation { get; private set; }
        protected IReadOnlyNetworkSetting NetworkSetting { get; }

        /// <summary>
        /// HttpClient用ハンドラ。
        /// </summary>
        protected HttpClientHandler ClientHandler { get; } = new HttpClientHandler() {
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
        };

        public abstract bool EnabledStartupAutoLogin { get; }

        /// <summary>
        /// ログイン状態。
        /// </summary>
        public LoginState LoginState
        {
            get { return this._loginState; }
            set
            {
                if(SetVariableValue(ref this._loginState, value)) {
                    if(this._loginState == LoginState.LoggedIn) {
                        RenewalLastLoginTime();
                    } else {
                        ClearLastLoginTime();
                    }
                    CallOnPropertyChange(nameof(IsLoggedIn));
                }
            }
        }

        public bool IsLoggedIn { get { return LoginState == LoginState.LoggedIn; } }

        /// <summary>
        /// ログイン時間。
        /// </summary>
        public DateTime LastLoginTime
        {
            get { return this._lastLoginTime; }
            private set { SetVariableValue(ref this._lastLoginTime, value); }
        }

        /// <summary>
        /// ログイン済みとみなす時間。
        /// <para>現在時間と最終ログイン時間がこの値未満であれば再ログイン実施。</para>
        /// </summary>
        public abstract TimeSpan RegardLoginTime { get; }

        #endregion

        #region function

        public abstract Task LoginAsync();
        public abstract Task LogoutAsync();
        public abstract Task<bool> CheckLoginAsync();

        /// <summary>
        /// ログインを自動実行する。
        /// </summary>
        /// <returns></returns>
        public async Task LoginIfNotLoginAsync()
        {
            if(LoginState == LoginState.LoggedIn) {
                if(DateTime.Now - LastLoginTime < RegardLoginTime) {
                    RenewalLastLoginTime();
                    return;
                }
            }
            if(!await CheckLoginAsync()) {
                await LoginAsync();
            }
        }

        public void RenewalLastLoginTime()
        {
            if(LoginState == LoginState.LoggedIn) {
                LastLoginTime = DateTime.Now;
            }
        }

        public void ClearLastLoginTime()
        {
            LastLoginTime = NotLoginTime;
        }

        protected abstract void ApplyToWebNavigatorEngineCore(WebNavigatorEngine engine, Uri uri);

        public void ApplyToWebNavigatorEngine(WebNavigatorEngine engine, Uri uri)
        {
            ApplyToWebNavigatorEngineCore(engine, uri);
        }

        #endregion

        #region ICreateHttpUserAgent

        public DateTime LastProxyChangedTimestamp { get; protected set; } = DateTime.MinValue;

        public HttpClient CreateHttpUserAgent()
        {
            if(NetworkUtility.CanSetProxy(this, NetworkSetting.LogicProxy)) {
                LastProxyChangedTimestamp = NetworkSetting.LogicProxy.ChangedTimestamp;
                ClientHandler.SetProxy(NetworkSetting.LogicProxy, Mediation.Logger);
            } else if(!ClientHandler.UseProxy) {
                ClientHandler.UseProxy = false;
            }

            // #665
            ClientHandler.CookieContainer.Add(new Cookie("watch_html5", "0", "/", ".nicovideo.jp"));
            ClientHandler.CookieContainer.Add(new Cookie("watch_flash", "1", "/", ".nicovideo.jp"));
            var httpUserAgent = new HttpClient(ClientHandler, false);
            httpUserAgent.SetLogicUserAgentText(NetworkSetting);

            return httpUserAgent;
        }

        #endregion
    }
}
