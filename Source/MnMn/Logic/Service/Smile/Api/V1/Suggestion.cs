using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Raw;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Api.V1
{
    public class Suggestion : ApiBase
    {
        public Suggestion(Mediation mediation)
            : base(mediation)
        { }

        #region function

        public Task<SmileSuggestionCompleteCandidateModel> LoadCompleteAsync(string word)
        {
            var page = new PageLoader(Mediation, HttpUserAgentHost, SmileMediationKey.suggestionComplete, ServiceType.Smile);
            page.ReplaceUriParameters["word"] = word;

            return page.GetResponseTextAsync(PageLoaderMethod.Get).ContinueWith(t => {
                page.Dispose();
                var response = t.Result;

                using(var stream = StreamUtility.ToUtf8Stream(response.Result)) {
                    return SerializeUtility.LoadJsonDataFromStream<SmileSuggestionCompleteCandidateModel>(stream);
                }
            });
        }

        #endregion
    }
}
