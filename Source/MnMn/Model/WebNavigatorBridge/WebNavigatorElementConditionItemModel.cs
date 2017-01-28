using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.WebNavigatorBridge
{
    [Serializable]
    public class WebNavigatorElementConditionItemModel: ModelBase
    {
        #region property

        /// <summary>
        /// メニュー項目を表示するか。
        /// <para><see cref="BaseUriPattern"/>が有効な場合に常時表示する意図で使用する</para>
        /// </summary>
        [XmlAttribute("visible")]
        public bool IsVisible { get; set; }

        /// <summary>
        /// この項目が特定のURIをもとにしているか。
        /// </summary>
        [XmlElement("base")]
        public string BaseUriPattern { get; set; }

        [XmlElement("tag")]
        public string TagNamePattern { get; set; }

        /// <summary>
        /// 全ての条件に一致するれば有効となる。
        /// </summary>
        [XmlArray("targets"), XmlArrayItem("target")]
        public CollectionModel<WebNavigatorElementConditionTagModel> TargetItems { get; set; } = new CollectionModel<WebNavigatorElementConditionTagModel>();

        /// <summary>
        /// 最終的に使用されるパラメータ。
        /// </summary>
        [XmlElement("parameter")]
        public WebNavigatorElementConditionParameterModel Parameter { get; set; } = new WebNavigatorElementConditionParameterModel();

        #endregion
    }
}
