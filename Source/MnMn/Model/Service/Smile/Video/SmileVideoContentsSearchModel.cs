﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.IF.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video
{
    [Serializable]
    public class SmileVideoContentsSearchModel: ModelBase, ISmileVideoSearchDefine
    {
        #region property

        [XmlAttribute("service")]
        public string Service { get; set; }

        [XmlArray("result"), XmlArrayItem("field")]
        public CollectionModel<string> Results { get; set; } = new CollectionModel<string>();

        #endregion

        #region ISmileVideoSearchDefine

        [XmlAttribute("max-index")]
        public int MaximumIndex { get; set; }
        [XmlAttribute("max-count")]
        public int MaximumCount { get; set; }

        [XmlArray("method"), XmlArrayItem("element")]
        public CollectionModel<DefinedElementModel> Methods { get; set; } = new CollectionModel<DefinedElementModel>();
        [XmlArray("sort"), XmlArrayItem("element")]
        public CollectionModel<DefinedElementModel> Sort { get; set; } = new CollectionModel<DefinedElementModel>();
        [XmlArray("type"), XmlArrayItem("element")]
        public CollectionModel<DefinedElementModel> Type { get; set; } = new CollectionModel<DefinedElementModel>();

        #endregion
    }
}
