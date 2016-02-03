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
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Service.Smile
{
    /// <summary>
    /// ニコニコユーザーのセッションを管理。
    /// </summary>
    public class SmileSessionViewModel: SessionViewModelBase
    {

        public SmileSessionViewModel(Mediation mediation, SmileUserAccountModel userAccountModel)
            : base(mediation)
        {
            UserAccount = userAccountModel;
        }

        #region property

        /// <summary>
        /// ユーザーアカウント情報。
        /// </summary>
        SmileUserAccountModel UserAccount { get; set; }

        #endregion

        #region function

        public string GetSession(Uri uri)
        {
            return ClientHandler.CookieContainer.GetCookies(uri)["user_session"].Value;
        }

        public override async Task LoginAsync()
        {
            if(LoginState == LoginState.In || LoginState == LoginState.Check || LoginState == LoginState.Logged) {
                return;
            }

            LoginState = LoginState.In;
            using(var page = new PageScraping(Mediation, this, SmileMediationKey.videoLogin, ServiceType.Smile)) {
                page.StopHeaderCheck = true;

                page.ReplaceRequestParameters["user"] = UserAccount.User;
                page.ReplaceRequestParameters["pass"] = UserAccount.Password;

                page.JudgeFailureStatusCode = response => {
                    LoginState = LoginState.Failure;
                    return CheckModel.Failure();
                };
                page.JudgeSuccessStatusCode = response => {
                    LoginState = LoginState.Check;
                    return CheckModel.Success();
                };

                page.JudgeCheckResponseHeaders = response => {
                    var successLogin = response.Headers
                        .FirstOrDefault(h => h.Key == "x-niconico-authflag")
                        .Value.Any(s => s == "1")
                    ;

                    if(successLogin) {
                        LoginState = LoginState.Logged;
                        return CheckModel.Success();
                    }

                    LoginState = LoginState.Failure;

                    return CheckModel.Failure();
                };

                await page.GetResponseTextAsync(Define.HttpMethod.Post);
            }
            return;
        }

        public override async Task LogoutAsync()
        {
            if(LoginState == LoginState.Failure || LoginState == LoginState.None) {
                return;
            }

            using(var page = new PageScraping(Mediation, this, SmileMediationKey.videoLogout, ServiceType.Smile)) {
                page.StopHeaderCheck = true;
                page.ExitProcess = () => {
                    LoginState = LoginState.None;
                };
                await page.GetResponseTextAsync(Define.HttpMethod.Get);
            }
        }

        public override async Task<bool> CheckLoginAsync()
        {
            if(LoginState == LoginState.None || LoginState == LoginState.Failure) {
                return false;
            }

            using(var page = new PageScraping(Mediation, this, SmileMediationKey.videoCheck, ServiceType.Smile)) {
                page.StopHeaderCheck = true;
                page.JudgeCheckResponseHeaders = response => {
                    var successLogin = response.Headers
                        .FirstOrDefault(h => h.Key == "x-niconico-authflag")
                        .Value.Any(s => s == "1")
                    ;
                    if(successLogin) {
                        LoginState = LoginState.Logged;
                        return CheckModel.Success();
                    }

                    return CheckModel.Failure();
                };
                var result = await page.GetResponseTextAsync(Define.HttpMethod.Get);
                return result.IsSuccess;
            }

        }

        #endregion
    }
}
