﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.Library.Bridging.IF.ReadOnly;
using ContentTypeTextNet.MnMn.Library.Bridging.Model.ProcessLink;
using ContentTypeTextNet.MnMn.Library.Bridging.Model.ProcessLinker;

namespace ContentTypeTextNet.MnMn.Library.Bridging.IF.ProcessLink
{
    [ServiceContract(Namespace = "http://content-type-text.net/mnmn/ProcessLinker")]
    public interface IProcessLink
    {
        #region function

        /// <summary>
        /// ホストへ接続の挨拶を行う。
        /// <para>WCFより上位で行われる約束事。</para>
        /// <para>名前付きパイプでまぁ使い道ないわな。</para>
        /// </summary>
        /// <param name="clientName">クライアント名称</param>
        /// <returns>接続可能な場合セッション情報が返る。接続できない場合は null が返る。</returns>
        [OperationContract]
        ProcessLinkSessionModel Connect(string clientName);

        /// <summary>
        /// サービスに対して何かしらの処理を行う。
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [OperationContract]
        ProcessLinkResultModel Execute(ServiceType serviceType, string key, string value);

        #endregion
    }
}
