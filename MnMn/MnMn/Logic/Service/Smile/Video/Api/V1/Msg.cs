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
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;
using ContentTypeTextNet.MnMn.MnMn.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Service.Smile;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video.Api.V1
{
    /// <summary>
    /// セッションが必要なのか不明。
    /// </summary>
    public class Msg: SessionApiBase<SmileSessionViewModel>
    {
        public Msg(Mediation mediation, SmileSessionViewModel session)
            : base(mediation, session)
        { }

        #region function

        public static RawSmileVideoMsgPacketModel Load(Stream stream)
        {
            return SerializeUtility.LoadXmlSerializeFromStream<RawSmileVideoMsgPacketModel>(stream);
        }

        public async Task<RawSmileVideoMsgPacketModel> GetAsync(Uri msgServer, string threadId, string userId, int getCount, int rangeHeadMinutes, int rangeTailMinutes, int rangeGetCount, int rangeGetAllCount, RawSmileVideoGetthreadkeyModel threadkeyModel)
        {
            using(var page = new PageLoader(Mediation, Session, SmileVideoMediationKey.msg, Define.ServiceType.SmileVideo)) {
                //page.ParameterType = ParameterType.Mapping;
                page.ReplaceUriParameters["msg-uri"] = msgServer.OriginalString;
                page.ReplaceRequestParameters["thread-id"] = threadId;
                page.ReplaceRequestParameters["user-id"] = userId;
                page.ReplaceRequestParameters["res_from"] = $"-{Math.Abs(getCount)}";
                if(rangeHeadMinutes < rangeTailMinutes && 0 < rangeGetCount) {
                    page.ReplaceRequestParameters["time-size"] = $"{rangeHeadMinutes}-{rangeTailMinutes}:{rangeGetCount}";
                    page.ReplaceRequestParameters["all-size"] = $"{Math.Abs(rangeGetAllCount)}";
                }
                if(threadkeyModel != null) {
                    page.ReplaceRequestParameters["threadkey"] = threadkeyModel.Threadkey;
                    page.ReplaceRequestParameters["force_184"] = threadkeyModel.Force184;
                }
                
                var rawMessage = await page.GetResponseTextAsync(Define.PageLoaderMethod.Post);
                Debug.WriteLine(rawMessage.Result);
                using(var stream = new MemoryStream(Encoding.UTF8.GetBytes(rawMessage.Result))) {
                    var result = Load(stream);
                    return result;
                }
            }

            #endregion
        }
    }
}
