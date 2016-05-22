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
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Service.Smile;
using Newtonsoft.Json.Linq;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video.Api.V1
{
    public class Tag: SessionApiBase<SmileSessionViewModel>
    {
        public Tag(Mediation mediation)
            : base(mediation, ServiceType.Smile)
        { }

        #region function

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
                result.Domain = Constants.CurrentLanguageCode;
                result.Tags.Add(item);
            }

            return result;
        }


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

        #endregion
    }
}
