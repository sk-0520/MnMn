/*
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
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Library.SharedLibrary.Attribute;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model
{
    /// <summary>
    /// ウィンドウ情報。
    /// </summary>
    [DataContract]
    public class WindowStatusModel: ModelBase, IDeepClone
    {
        #region property

        /// <summary>
        /// X座標。
        /// </summary>
        [DataMember, IsDeepClone]
        public double Left { get; set; }
        /// <summary>
        /// Y座標。
        /// </summary>
        [DataMember, IsDeepClone]
        public double Top { get; set; }
        /// <summary>
        /// 横幅。
        /// </summary>
        [DataMember, IsDeepClone]
        public double Width { get; set; }
        /// <summary>
        /// 高さ。
        /// </summary>
        [DataMember, IsDeepClone]
        public double Height { get; set; }

        /// <summary>
        /// 最前面。
        /// </summary>
        [DataMember, IsDeepClone]
        public bool Topmost { get; set; }

        #endregion

        #region IDeepClone

        public IDeepClone DeepClone()
        {
            return DeepCloneUtility.Copy(this);
        }

        #endregion
    }
}
