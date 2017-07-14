using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Video
{
    public class SmileVideoOtherDefineRequestModel : OtherDefineRequestModelBase
    {
        public SmileVideoOtherDefineRequestModel(SmileVideoOtherDefineKind otherDefineKind)
            :base(ContentTypeTextNet.MnMn.Library.Bridging.Define.ServiceType.SmileVideo)
        {
            OtherDefineKind = otherDefineKind;
        }

        #region property

        public SmileVideoOtherDefineKind OtherDefineKind { get; }

        #endregion
    }
}
