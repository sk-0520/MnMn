using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.MnMn.Logic
{
    public abstract class MediationCustomBase: MediationBase
    {
        protected MediationCustomBase(Mediation mediation, string uriListPath, string uriParametersPath, string requestParametersPath, string requestMappingsPath)
            : base(uriListPath, uriParametersPath, requestParametersPath, requestMappingsPath)
        {
            Mediation = mediation;
        }

        #region property

        protected Mediation Mediation { get; private set; }

        #endregion
    }
}
