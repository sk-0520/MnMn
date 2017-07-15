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
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
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
    public class Getthreadkey: SessionApiBase<SmileSessionViewModel>
    {
        public Getthreadkey(Mediation mediation) 
            : base(mediation, ServiceType.Smile)
        {
        }

        #region function

        public async Task<RawSmileVideoGetthreadkeyModel> LoadAsync(string threadId)
        {
            using(var page = new PageLoader(Mediation, Session, SmileVideoMediationKey.getthreadkey, ServiceType.SmileVideo)) {
                page.ReplaceUriParameters["thread-id"] = threadId;
                var response = await page.GetResponseTextAsync(Define.PageLoaderMethod.Get);
                if(!response.IsSuccess) {
                    return null;
                }

                var rawText = response.Result;
                var result = RawValueUtility.ConvertNameModelFromWWWFormData<RawSmileVideoGetthreadkeyModel>(rawText);
                result.Raw = rawText;
                return result;
            }
        }

        #endregion
    }
}
