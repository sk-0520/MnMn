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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Raw;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;
using Newtonsoft.Json.Linq;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile
{
    public static class SmileMyListUtility
    {
        public static bool IsSuccessResponse(RawSmileAccountMyListGroupModel rawModel)
        {
            return string.Compare(rawModel.Status.Trim(), "ok", true) == 0;
        }

        public static SmileMyListResult ConvertResultStatus(JObject json)
        {
            var status = json.SelectToken("status");
            if(status.Value<string>() == "ok") {
                return SmileMyListResult.Success;
            }
            var error = json.SelectToken("error");
            var code = error.Value<string>("code");
            var map = new Dictionary<string, SmileMyListResult>() {
                { "EXIST", SmileMyListResult.Exists },
                { "PARAMERROR", SmileMyListResult.ParameterError },
            };
            var pair = map.FirstOrDefault(p => string.Compare(p.Key, code, true) == 0);
            if(!string.IsNullOrEmpty(pair.Key)) {
                return pair.Value;
            }

            return SmileMyListResult.Unknown;
        }

        public static string GetMyListId(SmileMyListResultModel resultModel)
        {
            CheckUtility.Enforce(resultModel.Result == SmileMyListResult.Success);
            return resultModel.Json.GetValue("id").Value<string>();
        }

        public static IEnumerable<Color> GetColorsFromExtends(IReadOnlyDictionary<string, string> extends)
        {
            var colorCodes = extends
                .Where(p => p.Key == "color")
                .Select(p => p.Value)
            ;

            foreach(var colorCode in colorCodes) {
                yield return (Color)ColorConverter.ConvertFromString(colorCode);
            }
        }
    }
}
