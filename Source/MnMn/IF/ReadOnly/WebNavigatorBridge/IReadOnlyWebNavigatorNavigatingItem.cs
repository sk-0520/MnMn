﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.WebNavigatorBridge
{
    public interface IReadOnlyWebNavigatorNavigatingItem: IReadOnlyWebNavigatorDefinedElement
    {
        #region property

        IReadOnlyList<IReadOnlyWebNavigatorPageCondition> Conditions { get; }

        #endregion
    }
}
