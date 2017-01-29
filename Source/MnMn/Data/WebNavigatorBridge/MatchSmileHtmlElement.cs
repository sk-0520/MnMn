using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.MnMn.Data.WebNavigatorBridge
{
    public struct MatchSmileHtmlElement
    {
        public MatchSmileHtmlElement(bool isHit, SimpleHtmlElement element, Match match)
        {
            IsHit = isHit;
            Element = element;
            Match = match;
        }

        #region property

        public bool IsHit { get; }
        public SimpleHtmlElement Element { get; }
        public Match Match { get; }

        #endregion
    }
}
