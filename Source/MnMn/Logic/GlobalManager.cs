using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Model;
using Microsoft.IO;

namespace ContentTypeTextNet.MnMn.MnMn.Logic
{
    /// <summary>
    /// <para>シングルトン的な扱い。</para>
    /// </summary>
    public static class GlobalManager
    {
        #region property

        public static RecyclableMemoryStreamManager MemoryStream { get; } = new RecyclableMemoryStreamManager();

        public static SystemParameterModel SystemParameter { get; set; }

        #endregion
    }
}
