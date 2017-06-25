using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.IF;

namespace ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.WebNavigatorBridge
{
    public interface IReadOnlyWebNavigatorElementConditionItem: IModel
    {
        #region property

        /// <summary>
        /// メニュー項目を表示するか。
        /// <para><see cref="BaseUriPattern"/>が有効な場合に常時表示する意図で使用する</para>
        /// </summary>
        bool IsVisible { get; set; }

        /// <summary>
        /// この項目が特定のURIをもとにしているか。
        /// </summary>
        string BaseUriPattern { get; set; }

        string TagNamePattern { get; set; }

        /// <summary>
        /// 全ての条件に一致するれば有効となる。
        /// </summary>
        IReadOnlyList<IReadOnlyWebNavigatorElementConditionTag> TargetItems { get; }

        /// <summary>
        /// 最終的に使用されるパラメータ。
        /// </summary>
        IReadOnlyWebNavigatorElementConditionParameter Parameter { get; }

        #endregion
    }
}
