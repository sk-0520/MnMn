using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.Library.Bridging.IF.ReadOnly
{
    public interface IReadOnlyLinkSession
    {
        #region property

        /// <summary>
        /// クライアント名。
        /// </summary>
        string ClientName { get; set; }

        /// <summary>
        /// サーバーから発行されるクライアント識別ID。
        /// </summary>
        string ClientId { get; set; }


        #endregion
    }
}
