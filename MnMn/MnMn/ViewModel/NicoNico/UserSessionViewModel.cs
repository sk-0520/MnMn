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
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Model.NicoNico;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.NicoNico
{
    /// <summary>
    /// ニコニコユーザーのセッションを管理。
    /// </summary>
    public class UserSessionViewModel: ViewModelBase
    {
        #region variable

        LoginState _loginState;

        #endregion

        public UserSessionViewModel(UserAccountModel userAccountModel, Mediation mediation)
        {
            UserAccount = userAccountModel;
            Mediation = mediation;
        }

        #region property

        /// <summary>
        /// 内部連携。
        /// </summary>
        Mediation Mediation { get; set; }

        /// <summary>
        /// HttpClient用ハンドラ。
        /// </summary>
        HttpClientHandler ClientHandler { get; } = new HttpClientHandler();

        /// <summary>
        /// ユーザーアカウント情報。
        /// </summary>
        UserAccountModel UserAccount { get; set; }

        /// <summary>
        /// ログイン状態。
        /// </summary>
        public LoginState LoginState
        {
            get { return this._loginState; }
            set { SetVariableValue(ref this._loginState, value); }
        }

        #endregion

        #region function

        public async Task<LoginState> LoginAsync()
        {
            if(LoginState == LoginState.In || LoginState == LoginState.Logged) {
                return LoginState;
            }

            LoginState = LoginState.In;

            var client = new HttpClient(ClientHandler);
            //var url = "https://secure.nicovideo.jp/secure/login?site=niconico";
            var url = Mediation.GetUri("video-login", Mediation.EmptyMap, ServiceType.NicoNico);
            var content = new FormUrlEncodedContent(new Dictionary<string, string> {
                { "next_url", string.Empty },
                { "mail", UserAccount.User },
                { "password", UserAccount.Password }
            });

            var response = await client.PostAsync(url, content);

            if(!response.IsSuccessStatusCode) {
                LoginState = LoginState.Failure;
                return LoginState;
            }

            LoginState = LoginState.Check;

            var result = await response.Content.ReadAsStringAsync();
            //TODO: 解析
            Debug.WriteLine(result);

            return LoginState.Logged;
        }

        #endregion
    }
}
