using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Parameter
{
    public class SmileOpenChannelIdParameterModel: ShowParameterModelBase
    {
        #region property

        public string ChannelId { get; set; }
        public bool IsLoginUser { get; set; }
        public bool AddHistory { get; set; } = true;

        #endregion
    }
}
