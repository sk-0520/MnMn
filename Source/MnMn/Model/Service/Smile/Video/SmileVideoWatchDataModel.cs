using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video
{
    public class SmileVideoWatchDataModel: ModelBase
    {
        #region property

        public RawSmileVideoWatchDataModel RawData { get; set; }

        public string HtmlSource { get; set; }

        #endregion
    }
}
