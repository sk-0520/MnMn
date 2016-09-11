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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Define;

namespace ContentTypeTextNet.MnMn.MnMn.Model.MultiCommandParameter.Service.Smile.Video
{
    public class SmileVideoOpenVideoCommandParameterModel: MultiCommandParameterModelBase
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="values">
        /// <list type="list">
        ///     <item>
        ///         <term>0</term>
        ///         <description><see cref="ExecuteOrOpenMode"/></description>
        ///     </item>
        ///     <item>
        ///         <term>1</term>
        ///         <description>bool: 新規ウィンドウで開くか</description>
        ///     </item>
        /// </list>
        /// </param>
        /// <param name="targetType"></param>
        /// <param name="culture"></param>
        public SmileVideoOpenVideoCommandParameterModel(object[] values, Type targetType, CultureInfo culture) : base(values, targetType, culture)
        {
            var paramCount = 2;
            if(values.Length != paramCount) {
                throw new ArgumentException($"{nameof(values)}.{nameof(values)} != {paramCount}");
            }

            OpenMode = (ExecuteOrOpenMode)values[0];
            OpenPlayerInNewWindow = (bool)values[1];
        }

        #region property

        public ExecuteOrOpenMode OpenMode { get; }

        public bool OpenPlayerInNewWindow { get; }

        #endregion
    }
}
