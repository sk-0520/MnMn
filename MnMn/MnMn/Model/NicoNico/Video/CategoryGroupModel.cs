﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.NicoNico.Video
{
    /// <summary>
    /// カテゴリ一覧を包括。
    /// </summary>
    [Serializable]
    public class CategoryGroupModel: ModelBase
    {
        #region 

        /// <summary>
        /// カテゴリ一覧。
        /// </summary>
        [XmlElement("category")]
        public CollectionModel<ElementModel> Categories { get; set; } = new CollectionModel<ElementModel>();

        /// <summary>
        /// カテゴリは一つだけか。
        /// </summary>
        public bool IsSingleCategory =>  Categories.Count == 1;

        #endregion
    }
}
