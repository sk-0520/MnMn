using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;

namespace ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.WebNavigatorBridge
{
    public interface IReadOnlyWebNavigatorDefinedElement: IReadOnlyDefinedElement
    {
        #region property

        /// <summary>
        /// 受け付けるサービス。
        /// <para><see cref="ContentTypeTextNet.MnMn.MnMn.View.Controls.WebNavigator.ServiceType"/>が渡される。</para>
        /// <para>基本的に<see cref="ServiceType.All"/>でOK。</para>
        /// </summary>
        ServiceType AllowService { get; }

        /// <summary>
        /// 処理するサービス。
        /// <para><see cref="ServiceType.All"/>はダメ。</para>
        /// </summary>
        ServiceType SendService { get; }

        #endregion
    }
}
