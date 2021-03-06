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
using ContentTypeTextNet.Library.SharedLibrary.Attribute;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model
{
    [Serializable]
    public class DefinedImageModel: ModelBase, IDeepClone
    {
        #region property

        /// <summary>
        /// 画像ファイルパス。
        /// </summary>
        [XmlAttribute("source")]
        [IsDeepClone]
        public string Source { get; set; }

        /// <summary>
        /// <see cref="Source"/>のパス指定方法。
        /// </summary>
        [XmlAttribute("kind")]
        [IsDeepClone]
        public string DefinedImageSourceKind { get; set; }

        #endregion

        #region IDeepClone

        public IDeepClone DeepClone()
        {
            return DeepCloneUtility.Copy(this);
        }

        #endregion
    }
}
