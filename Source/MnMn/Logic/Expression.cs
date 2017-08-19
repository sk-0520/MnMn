using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly;
using ContentTypeTextNet.MnMn.MnMn.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Logic
{
    internal class Expression: IReadOnlyExpression
    {
        #region variable

        string _word;
        Regex _regex;
        string _xpath;

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

        #region function

        string GetSimpleValue()
        {
            return Item.Data.Text
                .SplitLines()
                .Select(s => s.Trim())
                .FirstOrDefault(s => 0 < s.Length)
                ?? string.Empty
            ;
        }

        #endregion

        #region IExpression

        public string Key => Element.Key;

        public string Id => Item.Id;

        public ExpressionItemKind Kind => Item.Kind;

        public string Word
        {
            get
            {
                if(Kind != ExpressionItemKind.Word) {
                    throw new InvalidOperationException($"Kind == {Kind}");
                }

                if(this._word == null) {
                    this._word = GetSimpleValue();
                }

                return this._word;
            }
        }

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
                if(Kind != ExpressionItemKind.XPath) {
                    throw new InvalidOperationException($"Kind == {Kind}");
                }

                if(this._xpath == null) {
                    this._xpath = GetSimpleValue();
                }

                return this._xpath;
            }
        }

        #endregion
    }
}
