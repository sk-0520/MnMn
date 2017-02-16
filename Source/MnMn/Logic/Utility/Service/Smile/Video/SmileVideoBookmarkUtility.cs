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
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Bookmark;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video
{
    /// <summary>
    /// ブックマーク関連の共通処理を提供。
    /// <para>非サービス管轄。</para>
    /// </summary>
    public static class SmileVideoBookmarkUtility
    {
        #region function

        /// <summary>
        /// <see cref="ConvertFlatBookmarkItems"/>の内部処理。
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        static List<SmileVideoBookmarkNodeViewModel> ConvertFlatBookmarkItemsCore(IEnumerable<SmileVideoBookmarkNodeViewModel> nodes, int level)
        {
            var result = new List<SmileVideoBookmarkNodeViewModel>();

            foreach(var node in nodes) {
                node.Level = level;
                result.Add(node);

                if(node.NodeItems.Any()) {
                    var items = ConvertFlatBookmarkItemsCore(node.NodeItems, level + 1);
                    result.AddRange(items);
                }
            }

            return result;
        }

        /// <summary>
        /// ノード構成のブックマークを平坦なブックマークのリストに変換する。
        /// <para>深度は<see cref="SmileVideoBookmarkNodeViewModel.Level"/>が示し、最上位が 0 となる。</para>
        /// <para>指しているものは<paramref name="nodes"/>と同じなんで変更は共有される。</para>
        /// </summary>
        /// <param name="nodes">ブックマーク。</param>
        /// <returns>平坦なブックマーク。</returns>
        public static IReadOnlyList<SmileVideoBookmarkNodeViewModel> ConvertFlatBookmarkItems(IEnumerable<SmileVideoBookmarkNodeViewModel> nodes)
        {
            var result = new List<SmileVideoBookmarkNodeViewModel>();

            foreach(var node in nodes) {
                node.Level = 0;
                result.Add(node);

                var list = ConvertFlatBookmarkItemsCore(node.NodeItems, 1);
                result.AddRange(list);
            }

            return result;
        }


        #endregion
    }
}
