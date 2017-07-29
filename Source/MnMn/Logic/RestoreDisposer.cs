using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Logic;

namespace ContentTypeTextNet.MnMn.MnMn.Logic
{
    public class RestoreDisposer: DisposeFinalizeBase
    {
        // 内部呼び出し用
        RestoreDisposer()
        { }

        public RestoreDisposer(Action action)
        {
            if(action == null) {
                throw new ArgumentNullException(nameof(action));
            }

            Action = action;
        }

        #region property

        Action Action { get; set; }

        #endregion

        #region function

        public static RestoreDisposer Empty => new RestoreDisposer();

        #endregion

        #region DisposeFinalizeBase

        protected override void Dispose(bool disposing)
        {
            if(Action != null) {
                Action();
                Action = null;
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}
