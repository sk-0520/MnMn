using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;
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

        #endregion
    }
}
