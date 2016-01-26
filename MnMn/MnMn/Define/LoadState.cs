using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.MnMn.Define
{
    public enum LoadState
    {
        None,
        Loading,
        Loaded,
        /// <summary>
        /// ログインチェック中。
        /// </summary>
        Check,
        Completed,
        /// <summary>
        /// ログイン失敗。
        /// </summary>
        Failure,
    }
}
