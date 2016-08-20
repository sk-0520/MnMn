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
using ContentTypeTextNet.MnMn.MnMn.Model.Setting;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video
{
    public class SmileVideoFinderFiltering: Filtering
    {
        public SmileVideoFinderFiltering(SmileVideoFinderFilteringItemSettingModel setting)
            : base(setting)
        {
            SubSetting = setting;
        }

        #region property

        SmileVideoFinderFilteringItemSettingModel SubSetting { get; }

        #endregion

        #region function

        public bool Check(string videoId, string title, string userId, string userName, string channelId, string channelName, string description, IEnumerable<SmileVideoTagViewModel> tags)
        {
            switch(SubSetting.Target) {
                case SmileVideoFinderFilteringTarget.VideoId:
                    return Check(videoId);

                case SmileVideoFinderFilteringTarget.Title:
                    return Check(title);

                case SmileVideoFinderFilteringTarget.UserId:
                    return Check(userId);

                case SmileVideoFinderFilteringTarget.UserName:
                    return Check(userName);

                case SmileVideoFinderFilteringTarget.ChannelId:
                    return Check(channelId);

                case SmileVideoFinderFilteringTarget.ChannelName:
                    return Check(channelName);

                case SmileVideoFinderFilteringTarget.Description:
                    return Check(description);

                case SmileVideoFinderFilteringTarget.Tag:
                    return tags.Any(c => Check(c.TagName));

                default:
                    throw new NotImplementedException();
            }
        }

        #endregion
    }
}
