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
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Utility
{
    /// <summary>
    /// ファイル名共通処理。
    /// </summary>
    public static class FileNameUtility
    {
        static string CreateFileNameCore(string name, string roll, string extension)
        {
            return $"{name}{(roll == null ? string.Empty : "-" + roll)}.{extension}";
        }
        /// <summary>
        /// ファイル名を生成。
        /// </summary>
        /// <param name="name">ファイル名。</param>
        /// <param name="roll">役割。</param>
        /// <param name="extension">拡張子。</param>
        /// <returns></returns>
        public static string CreateFileName(string name, string roll, string extension)
        {
            CheckUtility.EnforceNotNullAndNotEmpty(roll);
            return CreateFileNameCore(name, roll, extension);
        }
        /// <summary>
        /// ファイル名を生成。
        /// </summary>
        /// <param name="name">ファイル名。</param>
        /// <param name="extension">拡張子。</param>
        /// <returns></returns>
        public static string CreateFileName(string name, string extension)
        {
            return CreateFileNameCore(name, null, extension);
        }

        /// <summary>
        /// 一時ファイル用拡張子の作成
        /// </summary>
        /// <returns></returns>
        public static string GetTemporaryExtension(string role)
        {
            return "." + Constants.GetNowTimestampFileName() + "." + role + "." + Constants.ExtensionTemporaryFile;
        }


    }
}
