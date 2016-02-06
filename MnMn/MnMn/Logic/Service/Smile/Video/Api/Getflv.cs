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
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic.Attribute;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Service.Smile;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video.Api
{
    /// <summary>
    /// NOTE: 気持ち後回し
    /// </summary>
    public class Getflv: SessionApiBase
    {
        public Getflv(Mediation mediation, SmileSessionViewModel session)
            : base(mediation, session)
        { }

        #region property
        #endregion

        #region function

        public async Task<RawSmileVideoGetflvModel> GetAsync(Uri uri)
        {
            await LoginIfNotLoginAsync();

            using(var page = new PageScraping(Mediation, SessionBase, SmileVideoMediationKey.getflvNormal, Define.ServiceType.SmileVideo)) {
                page.ForceUri = uri;

                var response = await page.GetResponseTextAsync(Define.HttpMethod.Get);
                var result = RawValueUtility.ConvertNameModelFromWWWFormData< RawSmileVideoGetflvModel>(response.Result);
                return result;
            }
        }

        public Task<RawSmileVideoGetflvModel> GetAsync(string videoId)
        {
            var map = new StringsModel() {
                { "video-id", videoId },
            };
            var srcUri = Mediation.GetUri(SmileVideoMediationKey.getflvNormal, map, Define.ServiceType.SmileVideo);
            var convertedUri = Mediation.ConvertUri(srcUri, Define.ServiceType.SmileVideo);
            var uri = new Uri(convertedUri);
            return GetAsync(uri);
        }

        #endregion
    }
}
