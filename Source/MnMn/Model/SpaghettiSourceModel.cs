using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.Library.SpaghettiAssembly.Define;
using Gecko.DOM.Xml;

namespace ContentTypeTextNet.MnMn.MnMn.Model
{
    [Serializable, XmlRoot("spaghetti")]
    public class SpaghettiSourceModel:ModelBase
    {
        #region property

        [XmlElement("code")]
        public CodeLanguage Code { get; set; }

        [XmlIgnore]
        public string Source { get; set; }

        [XmlElement("source")]
        [XmlText]
        public XmlNode[] CDataSource
        {
            get
            {
                var dummy = new System.Xml.XmlDocument();
                return new XmlNode[] { dummy.CreateCDataSection(Source) };
            }
            set
            {
                if(value == null) {
                    Source = null;
                    return;
                }

                if(value.Length != 1) {
                    throw new InvalidOperationException($"Invalid array length {value.Length}");
                }

                Source = value[0].Value;
            }
        }


        #endregion
    }
}
