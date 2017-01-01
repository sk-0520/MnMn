using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.Library.Bridging.Model.CodeExecutor
{
    public class CodeInitializeModel: ModelBase
    {
        #region property

        public string DomainName { get; set; }

        public string Identifier { get; set; }

        public string Sequence { get; set; }

        #endregion
    }
}
