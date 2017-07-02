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
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Video.Parameter
{
    public class SmileVideoProcessCheckItLaterParameterModel: SmileVideoProcessParameterModelBase
    {
        public SmileVideoProcessCheckItLaterParameterModel(SmileVideoVideoItemModel videoItem, SmileVideoCheckItLaterFrom checkItLaterFrom)
            : this(videoItem, checkItLaterFrom, false)
        { }

        public SmileVideoProcessCheckItLaterParameterModel(SmileVideoVideoItemModel videoItem, SmileVideoCheckItLaterFrom checkItLaterFrom, bool isForce)
            : base(SmileVideoProcess.CheckItLater)
        {
            VideoItem = videoItem;
            IsForce = isForce;
            CheckItLaterFrom = checkItLaterFrom;
        }

        #region property

        public SmileVideoVideoItemModel VideoItem { get; }

        public bool IsForce { get; }

        public SmileVideoCheckItLaterFrom CheckItLaterFrom { get; }

        #endregion
    }
}
