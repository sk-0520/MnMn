using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.Library.Bridging.Define.CodeExecutor;
using ContentTypeTextNet.MnMn.Library.Bridging.Model;
using ContentTypeTextNet.MnMn.Library.Bridging.Model.CodeExecutor;
using Gecko.DOM.Xml;

namespace ContentTypeTextNet.MnMn.MnMn.Model
{
    [Serializable, XmlRoot("SpaghettiCode")]
    public class SpaghettiSourceModel:ModelBase
    {
        #region property

        public CodeLanguage CodeLanguage { get; set; }

        public bool SkipNext { get; set; }

        public CompileParameterModel Parameter { get; set; } = new CompileParameterModel();

        /// <summary>
        /// 使用する名前空間。
        /// <para>コードで直接使うと重複判定がしんどい。</para>
        /// </summary>
        public CollectionModel<string> NameSpace { get; set; } = new CollectionModel<string>();

        [XmlIgnore]
        public string Code { get; set; }

        [XmlElement("Code")]
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
