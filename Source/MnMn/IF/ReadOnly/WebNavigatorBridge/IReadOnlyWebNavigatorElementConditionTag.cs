﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.IF;

namespace ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.WebNavigatorBridge
{
    public interface IReadOnlyWebNavigatorElementConditionTag: IModel
    {
        #region property

        string TagNamePattern { get; }

        string Attribute { get; }

        string ValuePattern { get; }

        #endregion
    }
}
