﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Live;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Live.Raw;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Service.Smile;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Live.Api
{
    public class GetPlayerStatus: SessionApiBase<SmileSessionViewModel>
    {
        public GetPlayerStatus(Mediation mediation)
            : base(mediation, ServiceType.Smile)
        { }

        #region function

        public static RawSmileLiveGetPlayerStatusModel ConvertFromGetPlayerStatusText(string getPlayerStatusText)
        {
            using(var stream = StreamUtility.ToUtf8Stream(getPlayerStatusText)) {
                return SerializeUtility.LoadXmlSerializeFromStream<RawSmileLiveGetPlayerStatusModel>(stream);
            }
        }

        Task<RawSmileLiveGetPlayerStatusModel> LoadAsyncCore(string liveId)
        {
            var userAgent = Session.CreateHttpUserAgent();

            var page = new PageLoader(Mediation, Session, SmileLiveMediationKey.getPlayerStatus, ServiceType.Smile);
            page.ReplaceUriParameters["live-id"] = liveId;

            return page.GetResponseTextAsync(PageLoaderMethod.Get).ContinueWith(t => {
                var response = t.Result;
                t.Dispose();
                page.Dispose();

                if(!response.IsSuccess) {
                    return default(RawSmileLiveGetPlayerStatusModel);
                } else {
                    var result = ConvertFromGetPlayerStatusText(response.Result);
                    return result;
                }
            });
        }

        public async Task<RawSmileLiveGetPlayerStatusModel> LoadAsync(string liveId)
        {
            await Session.LoginIfNotLoginAsync();

            return await LoadAsyncCore(liveId);
        }

        #endregion
    }
}
