/*
This file is part of MnMn.

MnMn is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

MnMn is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with MnMn.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.Setting;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Service
{
    public abstract class ApiBase: DisposeFinalizeBase
    {
        public ApiBase(Mediation mediation)
        {
            Mediation = mediation;

            NetworkSetting = Mediation.GetNetworkSetting();

            HttpUserAgentHost = new HttpUserAgentHost(NetworkSetting, Mediation.Logger);
        }

        #region property

        protected Mediation Mediation { get; }
        protected IReadOnlyNetworkSetting NetworkSetting { get; }

        /// <summary>
        /// セッション操作を行うか。
        /// </summary>
        public virtual bool SessionSupport
        {
            get { return false; }
            set { throw new NotSupportedException(); }
        }

        protected HttpUserAgentHost HttpUserAgentHost { get; set; }

        #endregion

        #region DisposeFinalizeBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(HttpUserAgentHost != null) {
                    HttpUserAgentHost.Dispose();
                    HttpUserAgentHost = null;
                }
            }
            base.Dispose(disposing);
        }

        #endregion

    }
}
