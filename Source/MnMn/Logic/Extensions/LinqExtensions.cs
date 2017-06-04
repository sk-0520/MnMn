using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Extensions
{
    public static class LinqExtensions
    {
        #region function

        /// <summary>
        /// <see cref="IEnumerable{T}.ToList"/>の代用。
        /// <para>速度は求めてない、GCに関していい感じにできればそれでいい。</para>
        /// <para>将来的に何かそれっぽいクラスをかませるかもしれない。</para>
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static EvalCollectionModel<TSource> ToEvalSequence<TSource>(this IEnumerable<TSource> source)
        {
            return new EvalCollectionModel<TSource>(source);
        }

        #endregion
    }
}
