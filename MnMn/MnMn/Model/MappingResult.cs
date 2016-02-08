using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model
{
    public class MappingResult: ModelBase
    {
        public string ContentType { get; set; }
        public StringsModel Header { get; } = new StringsModel();
        public string Result { get; set; }
    }
}
