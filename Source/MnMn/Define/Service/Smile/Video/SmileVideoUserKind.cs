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
using ContentTypeTextNet.MnMn.MnMn.Attribute;

namespace ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video
{
    /// <summary>
    /// ユーザー種別。
    /// </summary>
    public enum SmileVideoUserKind
    {
        [EnumResourceDisplay(nameof(Properties.Resources.String_App_Define_Service_Smile_Video_SmileVideoUserKind_Noraml))]
        Noraml,
        [EnumResourceDisplay(nameof(Properties.Resources.String_App_Define_Service_Smile_Video_SmileVideoUserKind_Premium))]
        Premium,
        [EnumResourceDisplay(nameof(Properties.Resources.String_App_Define_Service_Smile_Video_SmileVideoUserKind_Alert))]
        Alert,
        [EnumResourceDisplay(nameof(Properties.Resources.String_App_Define_Service_Smile_Video_SmileVideoUserKind_Real))]
        Real,
        [EnumResourceDisplay(nameof(Properties.Resources.String_App_Define_Service_Smile_Video_SmileVideoUserKind_Official))]
        Official,
    }
}
