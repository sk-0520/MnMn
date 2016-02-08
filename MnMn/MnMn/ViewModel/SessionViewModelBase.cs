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
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.Logic;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel
{
    /// <summary>
    /// セッションを管理する。
    /// <para>基本的なログイン・ログアウトをサポート。</para>
    /// </summary>
    public abstract class SessionViewModelBase: ViewModelBase, ICreateHttpUserAgent
    {
        #region variable

        LoginState _loginState;

        #endregion

        public SessionViewModelBase(Mediation mediation)
        {
            Mediation = mediation;
        }

        #region property

        /// <summary>
        /// 内部連携。
        /// </summary>
        public Mediation Mediation { get; private set; }

        /// <summary>
        /// HttpClient用ハンドラ。
        /// </summary>
        protected HttpClientHandler ClientHandler { get; } = new HttpClientHandler();

        /// <summary>
        /// ログイン状態。
        /// </summary>
        public LoginState LoginState
        {
            get { return this._loginState; }
            set { SetVariableValue(ref this._loginState, value); }
        }

        /// <summary>
        /// この期間内であればログイン済みと判断する。
        /// </summary>
        public　virtual  CacheSpan CacheSpan { get; }

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
            if(!await CheckLoginAsync()) {
                await LoginAsync();
            }
        }

        #endregion

        #region ICreateHttpUserAgent

        public HttpClient CreateHttpUserAgent()
        {
            return new HttpClient(ClientHandler, false);
        }

        #endregion
    }
}
