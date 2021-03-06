﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly
{
    public interface IReadOnlyAcceptVersion
    {
        #region property

        Version ForceAcceptVersion { get;  }
        IReadOnlyList<IReadOnlyDefinedElement> Items { get; }

        #endregion
    }
}
