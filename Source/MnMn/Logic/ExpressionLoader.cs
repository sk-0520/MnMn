using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Logic
{
    public class ExpressionLoader
    {
        public ExpressionLoader(ExpressionsModel model)
        {
            Model = model;
        }

        #region property

        ExpressionsModel Model { get; }

        Cacher<KeyValuePair<string, string>, Expression> ExpressionCache { get; } = new Cacher<KeyValuePair<string, string>, Expression>();

        #endregion

        #region function

        ExpressionElementModel GetElement(string key)
        {
            return Model.Elements.FirstOrDefault(e => e.Key == key);
        }

        ExpressionItemModel GetItem(ExpressionElementModel element, string id)
        {
            if(id == string.Empty) {
                return element.Items.First();
            }

            return element.Items.First(i => i.Id == id);
        }

        Expression CreateExpression(string key, string id)
        {
            var element = GetElement(key);
            var item = GetItem(element, id);

            var result = new Expression(element, item);

            return result;
        }

        IExpression GetExpressionCore(string key, string id)
        {
            // valuetupleのためにnugetとかイヤなんすけど。。。
            var pair = new KeyValuePair<string, string>(key, id);
            return ExpressionCache.Get(pair, () => CreateExpression(key, id));
        }

        public IExpression GetExpression(string key)
        {
            return GetExpressionCore(key, string.Empty);
        }

        public IExpression GetExpression(string key, string id)
        {
            if(string.IsNullOrEmpty(id)) {
                throw new ArgumentException(nameof(id));
            }

            return GetExpressionCore(key, null);
        }

        #endregion

    }
}
