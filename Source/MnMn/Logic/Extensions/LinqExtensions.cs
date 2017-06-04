using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public static IList<TSource> ToSimpleSequence<TSource>(this IEnumerable<TSource> source)
        {
            return source.ToList();
        }

        #endregion
    }
}
