using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Service.Smile;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video.Api.V1
{
    public class Search : SessionApiBase<SmileSessionViewModel>
    {
        public Search(Mediation mediation)
            : base(mediation, ServiceType.Smile)
        { }

        #region function

        public async Task<RawSmileVideoSearchModel> SearchAsync(string query, string searchType, string sort, string orderBy, int fromIndex, int getCount)
        {
            await LoginIfNotLoginAsync();

            using(var page = new PageLoader(Mediation, Session, SmileVideoMediationKey.search, ServiceType.SmileVideo)) {
                page.ReplaceUriParameters["query"] = query;
                page.ReplaceUriParameters["type"] = searchType;
                page.ReplaceUriParameters["page"] = fromIndex.ToString();
                page.ReplaceUriParameters["sort"] = sort;
                page.ReplaceUriParameters["order"] = orderBy;
                var response = await page.GetResponseTextAsync(PageLoaderMethod.Get);
                using(var stream = StreamUtility.ToUtf8Stream(response.Result)) {
                    return SerializeUtility.LoadJsonDataFromStream<RawSmileVideoSearchModel>(stream);
                }
            }
        }

        #endregion
    }
}
