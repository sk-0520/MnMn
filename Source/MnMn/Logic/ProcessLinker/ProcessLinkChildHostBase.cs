﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.Library.Bridging.Model.ProcessLink;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.ProcessLink;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.ProcessLink
{
    public abstract class ProcessLinkChildHostBase : IProcessLinkChildHost
    {
        public ProcessLinkChildHostBase(Mediator mediator)
        {
            Mediator = mediator;
        }

        #region property

        protected Mediator Mediator { get; }

        #endregion

        #region function

        protected static string GetExceptionString(IReadOnlyProcessLinkExecuteParameter parameter)
        {
            return $"{nameof(parameter.ServiceType)}: {parameter.ServiceType}, {nameof(parameter.Key)}: {parameter.Key}, {nameof(parameter.Value)}: {parameter.Value}";
        }

        #endregion

        #region IProcessLinkChildHost

        public abstract Task<ProcessLinkResultModel> Execute(IReadOnlyProcessLinkExecuteParameter parameter);

        #endregion
    }
}
