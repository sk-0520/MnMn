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

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video
{
    public class SmileVideoFiltering: Filtering
    {
        public SmileVideoFiltering(SmileVideoFilteringItemSettingModel setting) 
            : base(setting)
        {
            SubSetting = setting;
        }

        #region property

        SmileVideoFilteringItemSettingModel SubSetting { get; }

        #endregion

        #region function

        public bool Check(string comment, string userId, IEnumerable<string> command)
        {
            switch(SubSetting.Target) {
                case SmileVideoFilteringTarget.Comment:
                    return Check(comment);

                case SmileVideoFilteringTarget.UserId:
                    return Check(userId);

                case SmileVideoFilteringTarget.Command:
                    return command.Any(c => Check(c));

                default:
                    throw new NotImplementedException();
            }
        }

        #endregion
    }
}
