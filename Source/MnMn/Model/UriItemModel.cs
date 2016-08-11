﻿/*
This file is part of MnMn.

MnMn is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

MnMn is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with MnMn.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.Define;

namespace ContentTypeTextNet.MnMn.MnMn.Model
{
    /// <summary>
    /// URI定義。
    /// </summary>
    [Serializable]
    public class UriItemModel: ModelBase
    {
        #region property
        /// <summary>
        /// URIの検索キー。
        /// </summary>
        [XmlAttribute("key")]
        public string Key { get; set; }
        /// <summary>
        /// 加工前URI。
        /// <para>完全なURIではない。</para>
        /// </summary>
        [XmlAttribute("uri")]
        public string Uri { get; set; }
        /// <summary>
        /// URI加工タイプ。
        /// </summary>
        [XmlAttribute("param")]
        public UriParameterType UriParameterType { get; set; }

        /// <summary>
        /// 要求時のパラメータ種別。
        /// </summary>
        [XmlAttribute("request-type")]
        public ParameterType RequestParameterType { get; set; }

        #endregion
    }
}