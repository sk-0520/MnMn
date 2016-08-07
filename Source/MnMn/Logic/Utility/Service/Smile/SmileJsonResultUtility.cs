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
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile;
using Newtonsoft.Json.Linq;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile
{
    public class SmileJsonResultUtility
    {
        public static SmileJsonResultState ConvertResultStatus(JObject json)
        {
            var status = json.SelectToken("status");
            if(status.Value<string>() == "ok") {
                return SmileJsonResultState.Success;
            }
            var error = json.SelectToken("error");
            var code = error.Value<string>("code");
            var map = new Dictionary<string, SmileJsonResultState>() {
                { "EXIST", SmileJsonResultState.Exists },
                { "PARAMERROR", SmileJsonResultState.ParameterError },
            };
            var pair = map.FirstOrDefault(p => string.Compare(p.Key, code, true) == 0);
            if(!string.IsNullOrEmpty(pair.Key)) {
                return pair.Value;
            }

            return SmileJsonResultState.Unknown;
        }
    }
}
