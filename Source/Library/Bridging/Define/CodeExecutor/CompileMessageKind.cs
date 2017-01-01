using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.Library.Bridging.Define.CodeExecutor
{
    public enum CompileMessageKind
    {
        /// <summary>
        /// コンパイル前の処理。
        /// </summary>
        PreProcessor,
        /// <summary>
        /// コンパイル処理。
        /// </summary>
        Compile,
        Warning,
        Error,
    }
}
