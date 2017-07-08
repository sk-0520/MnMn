using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video
{
    public class SmileVideoCheckItLaterFromModel: ModelBase, IReadOnlySmileVideoCheckItLaterFrom
    {
        #region IReadOnlySmileVideoCheckItLaterFrom

        public string FromId { get; set; }
        public string FromName { get; set; }

        #endregion
    }
}
