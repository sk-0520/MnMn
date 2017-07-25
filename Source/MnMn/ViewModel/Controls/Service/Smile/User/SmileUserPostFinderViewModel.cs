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
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw.Feed;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.User
{
    public class SmileUserPostFinderViewModel: SmileVideoFeedFinderViewModelBase
    {
        public SmileUserPostFinderViewModel(Mediator mediator, string userId)
            : base(mediator, 0)
        {
            UserId = userId;
        }

        #region property

        string UserId { get; }

        #endregion

        #region SmilePostVideoFinderViewModel

        protected override SmileVideoInformationFlags InformationFlags => SmileVideoInformationFlags.Length | SmileVideoInformationFlags.FirstRetrieve;

        protected override Task<FeedSmileVideoModel> LoadFeedAsync()
        {
            var user = new ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.HalfBakedApi.User(Mediation);
            return user.LoadPostVideoAsync(UserId);
        }

        #endregion
    }
}
