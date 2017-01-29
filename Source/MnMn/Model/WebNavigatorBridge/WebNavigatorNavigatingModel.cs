﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.WebNavigatorBridge
{
    [Serializable]
    public class WebNavigatorNavigatingModel: ModelBase
    {
        #region property

        [XmlElement("element")]
        public CollectionModel<WebNavigatorNavigatingItemModel> Items { get; set; } = new CollectionModel<WebNavigatorNavigatingItemModel>();

        #endregion
    }
}
