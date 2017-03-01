using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Request.Parameter
{
    public class AppLoggingParameterModel: ProcessParameterModelBase
    {
        #region property

        /// <summary>
        /// 複製したログを取得するか。
        /// </summary>
        public bool GetClone { get; set; }

        #endregion
    }
}
