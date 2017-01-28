using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.MnMn.Data.WebNavigatorBridge
{
    public class SimpleHtmlElement
    {
        #region property

        public string Name { get; set; }

        public IDictionary<string, string> Attributes { get; set; }

        #endregion

        #region Object

        public override string ToString()
        {
            return Name;
        }

        #endregion
    }
}
