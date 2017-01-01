using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.Library.Bridging.Model;
using ContentTypeTextNet.MnMn.Library.SpaghettiAssembly.Define;
using Gecko.DOM.Xml;

namespace ContentTypeTextNet.MnMn.MnMn.Model
{
    [Serializable, XmlRoot("spaghetti")]
    public class SpaghettiSourceModel:ModelBase
    {
        #region property

        [XmlElement("code-language")]
        public CodeLanguage CodeLanguage { get; set; }

        public CompileParameterModel Parameter { get; set; } = new CompileParameterModel();

        [XmlIgnore]
        public string Code { get; set; }

        [XmlElement("code")]
        [XmlText]
        public XmlNode[] CDataCode
        {
            get
            {
                var dummy = new System.Xml.XmlDocument();
                return new XmlNode[] { dummy.CreateCDataSection(Code) };
            }
            set
            {
                if(value == null) {
                    Code = null;
                    return;
                }

                if(value.Length != 1) {
                    throw new InvalidOperationException($"Invalid array length {value.Length}");
                }

                Code = value[0].Value;
            }
        }


        #endregion
    }
}
