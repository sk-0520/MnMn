﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.WebNavigatorBridge
{
    public interface IReadOnlyWebNavigatorContextMenu
    {
        #region property

        IReadOnlyList<IReadOnlyWebNavigatorContextMenuItem> Items { get; }

        #endregion
    }
}
