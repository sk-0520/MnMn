using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;

namespace ContentTypeTextNet.MnMn.MnMn.Model.WebNavigatorBridge
{
    /// <summary>
    /// <para>キー自体は<see cref="ContentTypeTextNet.MnMn.MnMn.Define.WebNavigatorContextMenuKey"/>で定義してるはず。</para>
    /// </summary>
    [Serializable]
    public class WebNavigatorContextMenuItemModel: DefinedElementModel
    {
        #region property

        /// <summary>
        /// 受け付けるサービス。
        /// <para><see cref="ContentTypeTextNet.MnMn.MnMn.View.Controls.WebNavigator.ServiceType"/>が渡される。</para>
        /// <para>基本的に<see cref="ServiceType.All"/>でOK。</para>
        /// </summary>
        [XmlAttribute("allow-service")]
        public ServiceType AllowService { get; set; }

        /// <summary>
        /// 処理するサービス。
        /// <para><see cref="ServiceType.All"/>はダメ。</para>
        /// </summary>
        [XmlAttribute("send-service")]
        public ServiceType SendService { get; set; }

        #endregion
    }
}
