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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Raw;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw.Feed;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Service.Smile;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Api
{
    public class MyList: SessionApiBase<SmileSessionViewModel>
    {
        public MyList(Mediation mediation, SmileSessionViewModel sessionViewModel)
            :base(mediation, sessionViewModel)
        { }

        /// <summary>
        /// とりあえずマイリスト内のアイテムを取得する。
        /// </summary>
        /// <returns></returns>
        public async Task<RawSmileAccountMyListDefaultModel> GetAccountDefaultAsync()
        {
            await LoginIfNotLoginAsync();

            using(var page = new PageLoader(Mediation, SessionBase, SmileMediationKey.mylistDefault, ServiceType.Smile)) {
                var response = await page.GetResponseTextAsync(PageLoaderMethod.Get);

                var rawJson = response.Result;
                using(var stream = new MemoryStream(Encoding.UTF8.GetBytes(rawJson))) {
                    return SerializeUtility.LoadJsonDataFromStream<RawSmileAccountMyListDefaultModel>(stream);
                }
            }
        }

        /// <summary>
        /// 自身のマイリスト一覧を取得する。
        /// </summary>
        /// <returns></returns>
        public async Task<RawSmileAccountMyListGroupModel> GetAccountGroupAsync()
        {
            await LoginIfNotLoginAsync();

            using(var page = new PageLoader(Mediation, SessionBase, SmileMediationKey.mylistGroup, ServiceType.Smile)) {
                var response = await page.GetResponseTextAsync(PageLoaderMethod.Get);

                var rawJson = response.Result;
                using(var stream = new MemoryStream(Encoding.UTF8.GetBytes(rawJson))) {
                    return SerializeUtility.LoadJsonDataFromStream<RawSmileAccountMyListGroupModel>(stream);
                }
            }
        }

        /// <summary>
        /// 指定未リストIDからがさーっと取得する。
        /// </summary>
        /// <param name="myListId"></param>
        /// <returns></returns>
        public async Task<FeedSmileVideoModel> GetGroupAsync(string myListId)
        {
            await Task.CompletedTask;// dummy
            throw new NotImplementedException();
        }

    }
}
