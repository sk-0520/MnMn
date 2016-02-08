﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video
{
    [Serializable, XmlRoot("search")]
    public class SmileVideoSearchModel: ModelBase
    {
        #region property

        [XmlAttribute("service")]
        public string Service { get; set; }

        [XmlArray("method"), XmlArrayItem("element")]
        public CollectionModel<SmileVideoElementModel> Methods { get; set; } = new CollectionModel<SmileVideoElementModel>();
        [XmlArray("sort"), XmlArrayItem("element")]
        public CollectionModel<SmileVideoElementModel> Sort { get; set; } = new CollectionModel<SmileVideoElementModel>();
        [XmlArray("type"), XmlArrayItem("element")]
        public CollectionModel<SmileVideoElementModel> Type { get; set; } = new CollectionModel<SmileVideoElementModel>();
        [XmlArray("result"), XmlArrayItem("field")]
        public CollectionModel<string> Results { get; set; } = new CollectionModel<string>();

        #endregion
    }
}

