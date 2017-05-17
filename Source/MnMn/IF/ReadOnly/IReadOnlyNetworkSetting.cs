﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly
{
    public interface IReadOnlyNetworkSetting
    {
        #region property

        IReadOnlyUserAgent LogicUserAgent { get; }
        IReadOnlyUserAgent BrowserUserAgent { get; }

        #endregion
    }
}
