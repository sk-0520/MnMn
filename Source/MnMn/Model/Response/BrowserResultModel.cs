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

        /// <summary>
        /// 後続のデフォルト処理をキャンセルするか。
        /// </summary>
        public bool Cancel { get; }

        #endregion
    }
}
