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
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.Library.Bridging.Model;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.HalfBakedApi;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Raw;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile;
using Gecko;

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

        RawSmileUserAccountSimpleModel SimpleUserAccount { get; set; }

        public string UserId
        {
            get { return SimpleUserAccount.UserId; }
        }

        public bool IsPremium
        {
            get { return RawValueUtility.ConvertBoolean(SimpleUserAccount.IsPremium); }
        }

        public bool IsOver18
        {
            get { return RawValueUtility.ConvertBoolean(SimpleUserAccount.IsOver18); }
        }

        /// <summary>
        /// 年齢取得。
        /// <para>ログインしていない、何かしらの事情で年齢が取得できない場合は 0 未満が返る。</para>
        /// </summary>
        public int Age
        {
            get
            {
                if(IsLoggedIn) {
                    return RawValueUtility.ConvertInteger(SimpleUserAccount.Age);
                }
                return -1;
            }
        }

        #endregion

        #region function

        //public IEnumerable<Cookie> GetCookies(Uri uri)
        //{
        //    //return ClientHandler.CookieContainer.GetCookies(uri).[Constants.ServiceSmileSessionKey].Value;
        //    var cookies = ClientHandler.CookieContainer.GetCookies(uri)
        //        .Cast<Cookie>()
        //    //.Select(c => $"{c.Name}={c.Value}")
        //    ;
        //    //return string.Join(";", cookies);
        //    return cookies;
        //}

        public Task ChangeUserAccountAsync(SmileUserAccountModel userAccount)
        {
            if(LoginState == LoginState.LoggedIn) {
                return LogoutAsync().ContinueWith(_ => {
                    UserAccount = userAccount;
                });
            }

            UserAccount = userAccount;
            return Task.CompletedTask;
        }

        #endregion

        #region SessionViewModelBase

        public override TimeSpan RegardLoginTime { get; } = TimeSpan.FromMinutes(30);

        public override bool EnabledStartupAutoLogin
        {
            get { return UserAccount.EnabledStartupAutoLogin; }
        }

        public override async Task LoginAsync()
        {
            if(LoginState == LoginState.In || LoginState == LoginState.Check || LoginState == LoginState.LoggedIn) {
                return;
            }

            LoginState = LoginState.In;
            using(var page = new PageLoader(Mediation, this, SmileMediationKey.videoLogin, ServiceType.Smile)) {
                //page.HeaderCheckOnly = true;

                page.ReplaceRequestParameters["user"] = UserAccount.Name;
                page.ReplaceRequestParameters["pass"] = UserAccount.Password;

                page.JudgeFailureStatusCode = res => {
                    LoginState = LoginState.Failure;
                    return CheckModel.Failure();
                };
                page.JudgeSuccessStatusCode = res => {
                    LoginState = LoginState.Check;
                    return CheckModel.Success();
                };

                page.JudgeCheckResponseHeaders = res => {
                    var successLogin = res.Headers
                        .FirstOrDefault(h => h.Key == "x-niconico-authflag")
                        .Value.Any(s => s == "1" || s == "3")
                    ;

                    if(successLogin) {
                        LoginState = LoginState.LoggedIn;
                        return CheckModel.Success();
                    }

                    LoginState = LoginState.Failure;

                    return CheckModel.Failure();
                };

                var response = await page.GetResponseTextAsync(Define.PageLoaderMethod.Post);
                if(response.IsSuccess) {
                    var user = new User(Mediation);
                    SimpleUserAccount = user.GetSimpleUserAccountModelFromHtmlSource(response.Result);
                }
            }
            return;
        }

        public override async Task LogoutAsync()
        {
            if(LoginState == LoginState.Failure || LoginState == LoginState.None) {
                return;
            }

            using(var page = new PageLoader(Mediation, this, SmileMediationKey.videoLogout, ServiceType.Smile)) {
                page.HeaderCheckOnly = true;
                page.ExitProcess = () => {
                    LoginState = LoginState.None;
                };
                await page.GetResponseTextAsync(Define.PageLoaderMethod.Get);
            }
        }

        public override async Task<bool> CheckLoginAsync()
        {
            if(LoginState == LoginState.None || LoginState == LoginState.Failure) {
                return false;
            }

            using(var page = new PageLoader(Mediation, this, SmileMediationKey.videoCheck, ServiceType.Smile)) {
                page.HeaderCheckOnly = true;
                page.JudgeCheckResponseHeaders = response => {
                    var successLogin = response.Headers
                        .FirstOrDefault(h => h.Key == "x-niconico-authflag")
                        .Value.Any(s => s == "1" || s == "3")
                    ;
                    if(successLogin) {
                        LoginState = LoginState.LoggedIn;
                        return CheckModel.Success();
                    }

                    return CheckModel.Failure();
                };
                var result = await page.GetResponseTextAsync(Define.PageLoaderMethod.Get);
                return result.IsSuccess;
            }
        }

        protected override void ApplyToWebNavigatorEngineCore(WebNavigatorEngine engine, Uri uri)
        {
            var cookies = ClientHandler.CookieContainer.GetCookies(uri)
                .Cast<System.Net.Cookie>()
            ;
            foreach(var cookie in cookies) {
                if(IsLoggedIn) {
                    CookieManager.Add(cookie.Domain, cookie.Path, cookie.Name, cookie.Value, cookie.Secure, cookie.HttpOnly, true, cookie.Expires.Ticks);
                } else {
                    CookieManager.Remove(cookie.Domain, cookie.Name, cookie.Path, false);
                }
            }
            //foreach(var cookie in cookies) {
            //    CookieManager.Add(cookie.Domain, cookie.Path, cookie.Name, cookie.Value, cookie.Secure, cookie.HttpOnly, true, cookie.Expires.Ticks);
            //}
        }

        #endregion
    }
}
