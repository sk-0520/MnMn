/*
This file is part of MnMn.

MnMn is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

MnMn is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with MnMn.  If not, see<http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile;
using Newtonsoft.Json.Linq;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile
{
    public class SmileJsonResultModel: CheckResultModel<JObject>
    {
        SmileJsonResultModel(JObject json, SmileJsonResultState resultState)
            : base(resultState == SmileJsonResultState.Success, json, resultState, null)
        { }

        public SmileJsonResultModel(JObject json)
            : this(json, SmileJsonResultUtility.ConvertResultStatus(json))
        { }

        #region property

        public SmileJsonResultState Status { get { return (SmileJsonResultState)Detail; } }

        #endregion

        #region function

        public static SmileJsonResultModel FailureLoadToken()
        {
            return new SmileJsonResultModel(null, SmileJsonResultState.Failure);
        }


        #endregion
    }
}
