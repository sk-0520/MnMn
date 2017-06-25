using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.WebNavigatorBridge
{
    public interface IReadOnlyWebNavigatorElementConditionTag
    {
        #region property

        string TagNamePattern { get; }

        string Attribute { get; }

        string ValuePattern { get; }

        #endregion
    }
}
