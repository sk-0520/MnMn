﻿/*
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
using System.Windows.Input;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw.Feed;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.NewArrivals
{
    public class SmileVideoNewArrivalsFinderViewModel: SmileVideoFeedFinderViewModelBase
    {
        public SmileVideoNewArrivalsFinderViewModel(Mediator mediator, string key)
            : base(mediator, 0)
        {
            Key = key;

            var titleMap = new StringsModel() {
                { SmileVideoMediatorKey.newarrival, global::ContentTypeTextNet.MnMn.MnMn.Properties.Resources.String_Service_Smile_SmileVideo_NewArrivals_NewArrival_Title },
                { SmileVideoMediatorKey.recent, global::ContentTypeTextNet.MnMn.MnMn.Properties.Resources.String_Service_Smile_SmileVideo_NewArrivals_Recent_Title },
                { SmileVideoMediatorKey.hotlist, global::ContentTypeTextNet.MnMn.MnMn.Properties.Resources.String_Service_Smile_SmileVideo_NewArrivals_Hotlist_Title },
                { SmileVideoMediatorKey.recommendation, global::ContentTypeTextNet.MnMn.MnMn.Properties.Resources.String_Service_Smile_SmileVideo_NewArrivals_Recommendations_Title },
            };
            Title = titleMap[Key];
        }

        #region property

        public string Key { get; }

        public string Title { get; }

        public virtual bool NeedSession { get { return false; } }

        #endregion

        #region command

        #endregion

        #region  function

        #endregion

        #region SmileVideoFeedFinderViewModelBase

        protected override SmileVideoInformationFlags InformationFlags => SmileVideoInformationFlags.Length;

        protected override Task<FeedSmileVideoModel> LoadFeedAsync()
        {
            var newArrival = new Logic.Service.Smile.Video.Api.V1.NewArrivals(Mediator);

            switch(Key) {
                case SmileVideoMediatorKey.newarrival:
                    return newArrival.LoadNewVideoAsync();

                case SmileVideoMediatorKey.recent:
                    return newArrival.LoadNewCommentAsync();

                default:
                    throw new NotImplementedException();
            }
        }

        #endregion
    }
}
