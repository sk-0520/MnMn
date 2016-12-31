using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;

namespace ContentTypeTextNet.MnMn.Library.Bridging.Attribute
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public abstract class DisplayTextAttributeBase: System.Attribute
    {
        protected DisplayTextAttributeBase(string value)
        {
            Value = value;
        }

        #region property

        public string Value { get; }

        public abstract string Text { get; }

        #endregion
    }
}
