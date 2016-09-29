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
    public enum SmileVideoSortType
    {
        [EnumResourceDisplay(nameof(Properties.Resources.String_App_Define_Service_Smile_Video_SmileVideoSortType_Number))]
        Number,
        [EnumResourceDisplay(nameof(Properties.Resources.String_App_Define_Service_Smile_Video_SmileVideoSortType_Title))]
        Title,
        [EnumResourceDisplay(nameof(Properties.Resources.String_App_Define_Service_Smile_Video_SmileVideoSortType_Length))]
        Length,
        [EnumResourceDisplay(nameof(Properties.Resources.String_App_Define_Service_Smile_Video_SmileVideoSortType_FirstRetrieve))]
        FirstRetrieve,
        [EnumResourceDisplay(nameof(Properties.Resources.String_App_Define_Service_Smile_Video_SmileVideoSortType_ViewCount))]
        ViewCount,
        [EnumResourceDisplay(nameof(Properties.Resources.String_App_Define_Service_Smile_Video_SmileVideoSortType_CommentCount))]
        CommentCount,
        [EnumResourceDisplay(nameof(Properties.Resources.String_App_Define_Service_Smile_Video_SmileVideoSortType_MyListCount))]
        MyListCount,
    }
}
