using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.Library.Bridging.Model.ProcessLink;
using ContentTypeTextNet.MnMn.MnMn.IF;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.ProcessLinker
{
    public abstract class ProcessLinkChildHostBase : IProcessLinkChildHost
    {
        public ProcessLinkChildHostBase(Mediation mediation)
        {
            Mediation = mediation;
        }

        #region property

        protected Mediation Mediation { get; }

        #endregion

        #region IProcessLinkChildHost

        public abstract ProcessLinkResultModel Execute(ServiceType serviceType, string key, string value);

        #endregion
    }
}
