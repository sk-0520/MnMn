using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Extensions
{
    public static class IReadOnlyDefinedElementExtensions
    {
        #region function

        /// <summary>
        /// <see cref="IReadOnlyDefinedElement.Extends"/> から指定されたキーの値を指定された型に変換する。
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="element"></param>
        /// <param name="extendKey">確実に存在すること。</param>
        /// <param name="parser"></param>
        /// <returns></returns>
        public static TResult GetValueInExtends<TResult>(this IReadOnlyDefinedElement element, string extendKey, Func<string, TResult> parser)
        {
            if(element == null) {
                throw new ArgumentNullException(nameof(element));
            }

            var rawValue = element.Extends[extendKey];
            var result = parser(rawValue);

            return result;
        }

        public static bool GetBooleanInExtends(this IReadOnlyDefinedElement element, string extendKey)
        {
            return GetValueInExtends(element, extendKey, s => bool.Parse(s));
        }

        #endregion
    }
}
