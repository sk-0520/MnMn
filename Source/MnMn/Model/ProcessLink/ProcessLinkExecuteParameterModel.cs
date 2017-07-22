using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.ProcessLink;

namespace ContentTypeTextNet.MnMn.MnMn.Model.ProcessLink
{
    public class ProcessLinkExecuteParameterModel : IReadOnlyProcessLinkExecuteParameter
    {
        #region IReadOnlyProcessLinkExecuteParameter

        public ServiceType ServiceType { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        #endregion
    }
}
