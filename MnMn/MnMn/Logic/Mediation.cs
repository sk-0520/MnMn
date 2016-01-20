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
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.Logic.NicoNico;

namespace ContentTypeTextNet.MnMn.MnMn.Logic
{
    /// <summary>
    /// データ連携等々の橋渡し。
    /// </summary>
    public class Mediation: MediationBase
    {
        public Mediation()
            : base()
        { }

        #region property

            /// <summary>
            /// ニコニコ関係。
            /// </summary>
            NicoNicoMediation NicoNico { get; } = new NicoNicoMediation();

        #endregion

        #region function



        #endregion

        #region MediationBase

        public override string GetUri(string key, IReadOnlyDictionary<string, string> replaceMap, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.NicoNico:
                case ServiceType.NicoNicoVideo:
                    return NicoNico.GetUri(key, replaceMap, serviceType);

                default:
                    ThrowNotSupportGetUri(key, replaceMap, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override string ConvertUri(string uri, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.NicoNico:
                case ServiceType.NicoNicoVideo:
                    return NicoNico.ConvertUri(uri, serviceType);

                default:
                    ThrowNotSupportConvertUri(uri, serviceType);
                    throw new NotImplementedException();
            }
        }


        #endregion
    }
}
