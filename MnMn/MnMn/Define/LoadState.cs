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
        /// <summary>
        /// 何もしてない。
        /// </summary>
        None,
        /// <summary>
        /// 準備中。
        /// </summary>
        Preparation,
        /// <summary>
        /// 読み込み中。
        /// </summary>
        Loading,
        /// <summary>
        /// 読み込んだ。
        /// </summary>
        Loaded,
        /// <summary>
        /// 読み込み失敗。
        /// </summary>
        Failure,
    }
}
