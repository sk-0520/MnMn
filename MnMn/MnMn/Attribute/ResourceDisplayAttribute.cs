using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Define;

namespace ContentTypeTextNet.MnMn.MnMn.Attribute
{
    public class ResourceDisplayAttribute: DisplayAttributeBase
    {
        public ResourceDisplayAttribute(string name)
            : base(DisplayKind.Resource, name)
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
