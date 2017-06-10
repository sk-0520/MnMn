using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model
{
    /// <summary>
    /// GC 対策用 <see cref="List{TValue}"/> のラッパー。
    /// <para>今時点で基本的に何もしない。</para>
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public sealed class EvaluatedSequence<TValue> : List<TValue>
    {
        public EvaluatedSequence()
            : base()
        { }

        public EvaluatedSequence(IEnumerable<TValue> items)
            : base(items)
        { }
    }
}
