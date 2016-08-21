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
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video
{
    /// <summary>
    /// ファインダーフィルタの必要データ。
    /// <para>引数でやってたけど多い。</para>
    /// </summary>
    public struct SmileVideoFinderFilteringParameterModel
    {
        #region property

        public string VideoId { get; set; }
        public string Title { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string ChannelId { get; set; }
        public string ChannelName { get; set; }
        public string Description { get; set; }
        public IEnumerable<SmileVideoTagViewModel> Tags { get; set; }

        #endregion
    }
}
