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
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Parameter;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Request
{
    public abstract class ProcessRequestModelBase: RequestModel
    {
        public ProcessRequestModelBase(ServiceType serviceType, ProcessParameterModelBase parameter)
            : base(RequestKind.Process, serviceType)
        {
            ParameterBase = parameter;
        }

        #region property

        public ProcessParameterModelBase ParameterBase { get; }

        #endregion
    }

    public abstract class ProcessRequestModelBase<TParameter>: ProcessRequestModelBase
        where TParameter : ProcessParameterModelBase
    {
        public ProcessRequestModelBase(ServiceType serviceType, TParameter parameter)
            : base(serviceType, parameter)
        {
            Parameter = (TParameter)ParameterBase;
        }

        #region property

        public TParameter Parameter { get; }

        #endregion
    }
}
