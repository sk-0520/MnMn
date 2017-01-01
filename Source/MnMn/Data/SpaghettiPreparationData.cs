using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.Library.Bridging.IF.CodeExecutor;
using ContentTypeTextNet.MnMn.MnMn.Define;

namespace ContentTypeTextNet.MnMn.MnMn.Model
{
    public class SpaghettiPreparationData
    {
        #region proeprty

        /// <summary>
        /// スクリプトの状態。
        /// </summary>
        public ScriptState State { get; set; }

        /// <summary>
        /// スクリプト用ファイル。
        /// </summary>
        public FileInfo File { get; set; }

        /// <summary>
        /// お堅い実行環境。
        /// </summary>
        public ICodeExecutor CodeExecutor { get; set; }

        /// <summary>
        /// 後続処理を無視するか。
        /// </summary>
        public bool SkipNext { get; set; }

        #endregion
    }
}
