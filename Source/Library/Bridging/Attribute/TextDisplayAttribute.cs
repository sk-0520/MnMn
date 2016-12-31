using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;

namespace ContentTypeTextNet.MnMn.Library.Bridging.Attribute
{
    public class TextDisplayAttribute: DisplayTextAttributeBase
    {
        public TextDisplayAttribute(string text)
            : base(text)
        { }

        #region DisplayAttributeBase

        public override string Text { get { return Value; } }

        #endregion
    }
}
