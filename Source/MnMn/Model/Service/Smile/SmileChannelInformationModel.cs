using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile
{
    [DataContract]
    public class SmileChannelInformationModel: ModelBase
    {
        #region property

        [DataMember]
        public string ChannelId { get; set; }

        [DataMember]
        public string ChannelCode { get; set; }

        [DataMember]
        public string ChannelName { get; set; }

        /// <summary>
        /// TODO: 暫定処理。
        /// </summary>
        [IgnoreDataMember]
        public bool HasPostVideo { get; } = true;

        #endregion
    }
}
