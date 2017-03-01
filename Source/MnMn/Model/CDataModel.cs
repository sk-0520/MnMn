using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model
{
    [Serializable]
    public sealed class CDataModel: ModelBase
    {
        #region property

        [XmlIgnore]
        public string Text { get; set; }

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
