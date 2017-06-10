using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;

namespace ContentTypeTextNet.MnMn.MnMn.Data
{
    public struct ExecuteData
    {
        public ExecuteData(string applicationPath, IEnumerable<string> arguments)
        {
            ApplicationPath = applicationPath;

            if(arguments == null) {
                Arguments = new CollectionModel<string>();
            } else {
                Arguments = arguments.ToEvaluatedSequence();
            }
        }

        #region property

        public string ApplicationPath { get; }

        public IReadOnlyList<string> Arguments { get; }

        #endregion
    }
}
