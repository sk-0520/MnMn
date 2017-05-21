﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model
{
    [Serializable, XmlRoot("expressions")]
    public class ExpressionsModel: ModelBase
    {
        #region property

        [XmlElement("element")]
        public CollectionModel<ExpressionElementModel> Elements { get; set; } = new CollectionModel<ExpressionElementModel>();

        #endregion
    }
}