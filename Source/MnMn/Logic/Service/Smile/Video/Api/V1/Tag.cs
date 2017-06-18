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
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw.Feed;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Service.Smile;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video.Api.V1
{
    public class Tag: SessionApiBase<SmileSessionViewModel>
    {
        public Tag(Mediation mediation)
            : base(mediation, ServiceType.Smile)
        { }

        #region function

        [Obsolete]
        RawSmileVideoTagListModel ConvertTagListFromRelation(string tagStrings)
        {
            var dataLine = tagStrings.SplitLines().First();
            var json = JObject.Parse(dataLine);

            var result = new RawSmileVideoTagListModel();

            var values = json["values"]
                .OrderBy(t => t["_rowid"].Value<int>())
                .Where(t => t["tag"] != null)
            ;
            foreach(var tag in values) {
                var item = new RawSmileVideoTagItemModel();
                item.Text = tag["tag"].Value<string>();
                result.Domain = AppUtility.GetCultureName();
                result.Tags.Add(item);
            }

            return result;
        }

        /// <summary>
        /// 関連タグを取得。
        /// </summary>
        /// <param name="tagName"></param>
        /// <returns></returns>
        [Obsolete]
        public Task<RawSmileVideoTagListModel> LoadRelationTagListAsync(string tagName)
        {
            var page = new PageLoader(Mediation, HttpUserAgentHost, SmileVideoMediationKey.tagRelation, ServiceType.SmileVideo);
            page.ReplaceRequestParameters["query"] = tagName;

            return page.GetResponseTextAsync(PageLoaderMethod.Post).ContinueWith(task => {
                page.Dispose();
                var response = task.Result;
                var result = ConvertTagListFromRelation(response.Result);
                return result;
            });
        }

        RawSmileVideoTagListModel ConvertTagListFromTrend(string htmlSource)
        {
            var htmlDocument = new HtmlDocument() {
                OptionAutoCloseOnEnd = true,
            };
            htmlDocument.LoadHtml(htmlSource);

            var result = new RawSmileVideoTagListModel();

            var parents = htmlDocument.DocumentNode.SelectNodes("//*[@class='box']");

            foreach(var patent in parents) {
                var item = new RawSmileVideoTagItemModel();

                var titleElement = patent.SelectSingleNode("./h2/a");
                item.Text = titleElement.InnerText.Trim();

                result.Tags.Add(item);
            }

            return result;
        }

        /// <summary>
        /// トレンドタグを取得。
        /// </summary>
        /// <returns></returns>
        public Task<RawSmileVideoTagListModel> LoadTrendTagListAsync()
        {
            var page = new PageLoader(Mediation, HttpUserAgentHost, SmileVideoMediationKey.tagTrend, ServiceType.SmileVideo);
            return page.GetResponseTextAsync(PageLoaderMethod.Post).ContinueWith(task => {
                page.Dispose();
                var response = task.Result;
                var result = ConvertTagListFromTrend(response.Result);
                return result;
            });
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="sort">未入力: コメントが新しい順,f: 投稿が新しい順,v: 再生が多い順,r: コメントが多い順,m: マイリストが多い順,l: 時間が長い順</param>
        /// <param name="order"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        Task<FeedSmileVideoModel> LoadTagFeedCoreAsync(string tag, string sort, string order, int pageNumber)
        {
            var page = new PageLoader(Mediation, HttpUserAgentHost, SmileVideoMediationKey.tagFeed, ServiceType.SmileVideo);
            page.ReplaceUriParameters["query"] = tag;
            page.ReplaceUriParameters["sort"] = sort;
            page.ReplaceUriParameters["order"] = order;
            page.ReplaceUriParameters["page"] = pageNumber == 0 ? string.Empty: page.ToString() ;

            return page.GetResponseTextAsync(PageLoaderMethod.Get).ContinueWith(t => {
                page.Dispose();
                var response = t.Result;

                if(!response.IsSuccess) {
                    return null;
                } else {
                    using(var stream = StreamUtility.ToUtf8Stream(response.Result)) {
                        return SerializeUtility.LoadXmlSerializeFromStream<FeedSmileVideoModel>(stream);
                    }
                }
            });
        }

        public Task<FeedSmileVideoModel> LoadTagFeedAsync(string tag, string sort, string order, int pageNumber)
        {
            return LoadTagFeedCoreAsync(tag, sort, order, pageNumber);
        }

        #endregion
    }
}
