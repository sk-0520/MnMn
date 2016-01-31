using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.MnMn.Define
{
    /// <summary>
    /// 読込状態。
    /// <para>読み込んだものがどういったものかまでは状態表示しない。</para>
    /// </summary>
    public enum LoadState
    {
        None,
        Loading,
        Loaded,
        /// <summary>
        /// ログイン失敗。
        /// </summary>
        Failure,
    }
}
