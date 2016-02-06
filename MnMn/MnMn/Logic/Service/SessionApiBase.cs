﻿/*
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
using ContentTypeTextNet.MnMn.MnMn.ViewModel;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Service
{
    public abstract class SessionApiBase: ApiBase
    {
        public SessionApiBase(Mediation mediation, SessionViewModelBase sessionBase)
            : base(mediation)
        {
            SessionBase = sessionBase;
        }

        #region property

        public SessionViewModelBase SessionBase { get; }
        /// <summary>
        /// この期間内であればログイン済みと判断する。
        /// </summary>
        public CacheSpan CacheSpan { get; }

        #endregion

        #region function

        protected async Task LoginIfNotLoginAsync()
        {
            if(SessionSupport) {
                if(!await SessionBase.CheckLoginAsync()) {
                    await SessionBase.LoginAsync();
                }
            }
        }

        #endregion
    }

    public class SessionApiBase<TSessionViewModel>: SessionApiBase
        where TSessionViewModel : SessionViewModelBase
    {
        public SessionApiBase(Mediation mediation, TSessionViewModel sessionBase)
           : base(mediation, sessionBase)
        {
            Session = sessionBase;
        }

        #region property

        public TSessionViewModel Session { get; }

        #endregion
    }
}