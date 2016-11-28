using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.MnMn.Data
{
    public struct ExecuteData
    {
        public ExecuteData(string applicationPath, IEnumerable<string> arguments)
        {
            ApplicationPath = applicationPath;

            if(arguments == null) {
                Arguments = new List<string>();
            } else {
                Arguments = arguments.ToList();
            }
        }

        #region property

        public string ApplicationPath { get; }

        public IReadOnlyList<string> Arguments { get; }

        #endregion
    }
}
