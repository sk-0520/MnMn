﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly
{
    public interface IReadOnlyWords
    {
        #region property

        IReadOnlyDictionary<string, string> Words { get; }

        #endregion
    }
}
