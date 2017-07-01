using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.WebNavigatorBridge
{
    public interface IReadOnlyWebNavigatorContextMenuItem: IReadOnlyWebNavigatorDefinedElement
    {
        #region property

        bool IsSeparator { get; }

        /// <summary>
        /// 表示・使用条件。
        /// <para>上から順に一番最初に合致したものが使用される。</para>
        /// </summary>
        IReadOnlyList<IReadOnlyWebNavigatorElementConditionItem> Conditions { get; }

        #endregion
    }
}
