using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly;

namespace ContentTypeTextNet.MnMn.MnMn.Model
{
    [Serializable]
    public sealed class CDataModel: ModelBase, IReadOnlyCData
    {
        #region property

        /// <summary>
        /// ユーザーコードから設定する CData の中身。
        /// </summary>
        [XmlIgnore]
        public string Text { get; set; }

        /// <summary>
        /// シリアライズ処理で使用する CData の中身。
        /// <para>ユーザーコードで使用しないこと。</para>
        /// </summary>
        [XmlText]
        public XmlNode[] CDataText
        {
            get
            {
                var dummy = new XmlDocument();
                return new XmlNode[] { dummy.CreateCDataSection(Text) };
            }
            set
            {
                if(value == null) {
                    Text = null;
                    return;
                }

                Text = value[0].Value;
            }
        }

        #endregion
    }
}
