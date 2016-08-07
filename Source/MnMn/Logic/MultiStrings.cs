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
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Logic
{
    /// <summary>
    /// 複数文字列をサポートする。
    /// <para>パラメータ周りで単一しか考慮できていなかったことによるドロドロ互換。</para>
    /// <para>制御コードなら多分大丈夫だろう、大丈夫であってくれの思い。</para>
    /// </summary>
    public class MultiStrings
    {
        #region define 

        public const char defaultSeparator = '\u001A';

        #endregion

        public MultiStrings(IEnumerable<string> strings)
        {
            Values.InitializeRange(strings);
        }

        public MultiStrings(string s)
            : this(s.Split(defaultSeparator))
        { }


        #region property

        public CollectionModel<string> Values { get; } = new CollectionModel<string>();

        #endregion

        #region object

        public override string ToString()
        {
            return string.Join(new string(defaultSeparator, 1), Values);
        }

        #endregion
    }
}
