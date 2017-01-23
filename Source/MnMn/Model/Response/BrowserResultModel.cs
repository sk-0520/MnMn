using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Response
{
    public class BrowserResultModel: ModelBase
    {
        public BrowserResultModel(bool cancel)
        {
            Cancel = cancel;
        }

        #region property

        public bool Cancel { get; }

        #endregion
    }
}
