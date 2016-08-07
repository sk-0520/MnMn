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
        //[EnumResourceDisplay(nameof(Properties.Resources.String_App_Define_Service_Smile_Video_SmileVideoSortType_Length))]
        //Length,
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
