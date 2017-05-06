using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Logic;

namespace ContentTypeTextNet.MnMn.MnMn.Model
{
    internal class WebNavigatorTagModel : ModelBase
    {
        #region property

        public ServiceType ServiceType { get;set;}
        public Mediation Mediation { get; set; }

        #endregion
    }
}
