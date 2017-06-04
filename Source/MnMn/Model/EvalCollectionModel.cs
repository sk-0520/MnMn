using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model
{
    public class EvalCollectionModel<TValue> : CollectionModel<TValue>
    {
        public EvalCollectionModel()
            : base()
        { }

        public EvalCollectionModel(IEnumerable<TValue> items)
            : base(items)
        { }
    }
}
