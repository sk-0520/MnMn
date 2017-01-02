using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.Library.Bridging.Attribute;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;

namespace ContentTypeTextNet.MnMn.MnMn.Attribute
{
    public class EnumResourceDisplayAttribute: DisplayTextAttributeBase
    {
        public EnumResourceDisplayAttribute(string name)
            : base(name)
        { }

        #region DisplayAttributeBase

        public override string Text
        {
            get
            {
                var res = global::ContentTypeTextNet.MnMn.MnMn.Properties.Resources.ResourceManager;
                return res.GetString(Value);
            }
        }

        #endregion

    }
}
