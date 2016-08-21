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
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video;
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

        public bool Check(SmileVideoFinderFilteringParameterModel parameter)
        {
            switch(SubSetting.Target) {
                case SmileVideoFinderFilteringTarget.VideoId:
                    return Check(parameter.VideoId);

                case SmileVideoFinderFilteringTarget.Title:
                    return Check(parameter.Title);

                case SmileVideoFinderFilteringTarget.UserId:
                    return Check(parameter.UserId);

                case SmileVideoFinderFilteringTarget.UserName:
                    return Check(parameter.UserName);

                case SmileVideoFinderFilteringTarget.ChannelId:
                    return Check(parameter.ChannelId);

                case SmileVideoFinderFilteringTarget.ChannelName:
                    return Check(parameter.ChannelName);

                case SmileVideoFinderFilteringTarget.Description:
                    return Check(parameter.Description);

                case SmileVideoFinderFilteringTarget.Tag:
                    return parameter.Tags.Any(c => Check(c.TagName));

                default:
                    throw new NotImplementedException();
            }
        }

        #endregion
    }
}
