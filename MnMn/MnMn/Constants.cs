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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.MnMn
{
    /// <summary>
    /// 各種定数。
    /// </summary>
    public class Constants
    {

        #region proeprty

        public static string AssemblyPath => Assembly.GetExecutingAssembly().Location;
        public static string AssemblyParentDirectoryPath => Path.GetDirectoryName(AssemblyPath);

        public static string EtcDirectoryPath => Path.Combine(AssemblyParentDirectoryPath, "etc");

        public static string DefineDirectoryPath => Path.Combine(EtcDirectoryPath, "define");

        public static string DefineNicoNicoDirectoryPath => Path.Combine(DefineDirectoryPath, "niconico");

        public static string NicoNicoUriListPath => Path.Combine(DefineNicoNicoDirectoryPath, "uri-list.xml");
        public static string NicoNicoUriParametersListPath => Path.Combine(DefineNicoNicoDirectoryPath, "uri-params.xml");
        public static string NicoNicoRequestParametersListPath => Path.Combine(DefineNicoNicoDirectoryPath, "request-params.xml");

        public static string DefineNicoNicoVideoDirectoryPath => Path.Combine(DefineNicoNicoDirectoryPath, "video");

        public static string NicoNicoVideoRankingPath => Path.Combine(DefineNicoNicoVideoDirectoryPath, "ranking.xml");

        #endregion
    }
}
