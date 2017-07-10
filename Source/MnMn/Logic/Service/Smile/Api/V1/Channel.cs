using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Service.Smile;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Api.V1
{
    /// <summary>
    /// チャンネル系。
    /// <para>一応セッション噛ませてるけど使わない方針。</para>
    /// </summary>
    public class Channel : SessionApiBase<SmileSessionViewModel>
    {
        public Channel(Mediation mediation)
            : base(mediation, ServiceType.Smile)
        { }

        #region function



        #endregion
    }
}
