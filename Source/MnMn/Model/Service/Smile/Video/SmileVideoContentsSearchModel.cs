﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video
{
    [Serializable]
    public class SmileVideoContentsSearchModel: ModelBase
    {
        #region property

        [XmlArray("method"), XmlArrayItem("element")]
        public CollectionModel<DefinedElementModel> Methods { get; set; } = new CollectionModel<DefinedElementModel>();
        [XmlArray("sort"), XmlArrayItem("element")]
        public CollectionModel<DefinedElementModel> Sort { get; set; } = new CollectionModel<DefinedElementModel>();
        [XmlArray("type"), XmlArrayItem("element")]
        public CollectionModel<DefinedElementModel> Type { get; set; } = new CollectionModel<DefinedElementModel>();
        [XmlArray("result"), XmlArrayItem("field")]
        public CollectionModel<string> Results { get; set; } = new CollectionModel<string>();

        [XmlAttribute("max-index")]
        public int MaximumIndex { get; set; }
        [XmlAttribute("max-count")]
        public int MaximumCount { get; set; }

        #endregion
    }
}
