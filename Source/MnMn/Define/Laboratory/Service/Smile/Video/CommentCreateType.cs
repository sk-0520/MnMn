using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Attribute;

namespace ContentTypeTextNet.MnMn.MnMn.Define.Laboratory.Service.Smile.Video
{
    public enum CommentCreateType
    {
        [EnumResourceDisplay(nameof(Properties.Resources.String_App_Define_Laboratory_Service_Smile_Video_CommentCreateType_Sequence))]
        Sequence,
        [EnumResourceDisplay(nameof(Properties.Resources.String_App_Define_Laboratory_Service_Smile_Video_CommentCreateType_Random))]
        Random,
    }
}
