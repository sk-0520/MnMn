using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly;
using ContentTypeTextNet.MnMn.MnMn.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Logic
{
    internal class Expression: IReadOnlyExpression
    {
        #region variable

        Regex _regex;

        #endregion

        public Expression(ExpressionElementModel element, ExpressionItemModel item)
        {
            Element = element;
            Item = item;
        }

        #region property

        ExpressionElementModel Element { get; }
        ExpressionItemModel Item { get; }

        #endregion

        #region IExpression

        public string Key => Element.Key;

        public string Id => Item.Id;

        public ExpressionItemKind Kind => Item.Kind;

        public Regex Regex
        {
            get
            {
                if(Kind != ExpressionItemKind.Regex) {
                    throw new InvalidOperationException($"Kind == {Kind}");
                }

                if(this._regex == null) {
                    this._regex = new Regex(Item.Data.Text, Item.RegexOptions);
                }

                return this._regex;
            }
        }

        public string XPath
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
