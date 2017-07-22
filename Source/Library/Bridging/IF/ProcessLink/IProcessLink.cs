using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.Library.Bridging.IF.ReadOnly;

namespace ContentTypeTextNet.MnMn.Library.Bridging.IF.ProcessLink
{
    [ServiceContract(Namespace = "http://content-type-text.net/mnmn/ProcessLinker")]
    public interface IProcessLink
    {
        #region function

        /// <summary>
        /// ホストへ接続の挨拶を行う。
        /// <para>WCFより上位で行われる約束事。</para>
        /// </summary>
        /// <param name="clientName">クライアント名称</param>
        /// <returns>接続可能な場合セッション情報が返る。接続できない場合は null が返る。</returns>
        [OperationContract]
        IReadOnlyProcessLinkSession Connect(string clientName);

        #endregion
    }
}
