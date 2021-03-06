﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.Setting
{
    public interface IReadOnlyNetworkSetting
    {
        #region property

        IReadOnlyUserAgent LogicUserAgent { get; }
        IReadOnlyNetworkProxy LogicProxy { get; }

        IReadOnlyUserAgent BrowserUserAgent { get; }
        IReadOnlyNetworkProxy BrowserProxy { get; }

        #endregion
    }
}
