using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.Library.Bridging.IF.ReadOnly;

namespace ContentTypeTextNet.MnMn.Library.Bridging.Model.CodeExecutor
{
    public class CodeInitializeModel: ModelBase, IReadOnlyCodeInitialize
    {
        #region IReadOnlyCodeInitialize

        public string DomainName { get; set; }

        public string Identifier { get; set; }

        public string Sequence { get; set; }

        #endregion
    }
}
