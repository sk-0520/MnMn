using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model
{
    /// <summary>
    /// GC 対策用 List のラッパー。
    /// <para>今時点で基本的に何もしない。</para>
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public sealed class EvalSequence<TValue> : List<TValue>
    {
        public EvalSequence()
            : base()
        { }

        public EvalSequence(IEnumerable<TValue> items)
            : base(items)
        { }
    }
}
