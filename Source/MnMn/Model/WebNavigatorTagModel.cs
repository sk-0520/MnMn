using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly;
using ContentTypeTextNet.MnMn.MnMn.Logic;

namespace ContentTypeTextNet.MnMn.MnMn.Model
{
    internal class WebNavigatorTagModel : ModelBase, IReadOnlyWebNavigatorTag
    {
        #region IReadOnlyWebNavigatorTag

        public ServiceType ServiceType { get;set;}
        public Mediator Mediation { get; set; }

        #endregion
    }
}
