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
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.ViewModel;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Service
{
    public abstract class SessionApiBase: ApiBase
    {
        public SessionApiBase(Mediator mediator, ServiceType sessionServiceType)
            : base(mediator)
        {
            SessionServiceType = sessionServiceType;
            SessionBase = Mediator.GetResultFromRequest<SessionViewModelBase>(new RequestModel(RequestKind.Session, SessionServiceType));
        }

        #region property

        public SessionViewModelBase SessionBase { get; }

        public ServiceType SessionServiceType { get; }

        #endregion

        #region function

        protected async Task LoginIfNotLoginAsync()
        {
            if(SessionSupport) {
                await SessionBase.LoginIfNotLoginAsync();
            }
        }

        #endregion

        #region ApiBase

        public override bool SessionSupport { get; set; } = true;

        #endregion
    }

    public class SessionApiBase<TSessionViewModel>: SessionApiBase
        where TSessionViewModel : SessionViewModelBase
    {
        public SessionApiBase(Mediator mediator, ServiceType sessionServiceType)
           : base(mediator, sessionServiceType)
        {
            Session = (TSessionViewModel)SessionBase;
        }

        #region property

        public TSessionViewModel Session { get; }

        #endregion
    }
}
