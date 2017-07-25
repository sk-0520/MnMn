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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video.HalfBakedApi;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw.Feed;
using HtmlAgilityPack;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.NewArrivals
{
    /// <summary>
    /// TODO: RSS一切関係なし！
    /// </summary>
    public class SmileVideoHotlistFinderViewModel: SmileVideoNewArrivalsFinderViewModel
    {
        public SmileVideoHotlistFinderViewModel(Mediator mediation)
            : base(mediation, SmileVideoMediationKey.hotlist)
        { }

        #region function

        #endregion

        #region SmileVideoNewArrivalsFinderViewModel

        protected override SmileVideoInformationFlags InformationFlags => SmileVideoInformationFlags.All;


        protected async override Task<FeedSmileVideoModel> LoadFeedAsync()
        {
            var hotList = new HotList(Mediation);

            var htmlDocument = await hotList.LoadPageHtmlDocument();
            if(htmlDocument == null) {
                FinderLoadState = SourceLoadState.Failure;
                return null;
            }

            FinderLoadState = SourceLoadState.SourceChecking;

            var feedModel = hotList.ConvertFeedModelFromPageHtml(htmlDocument);
            return feedModel;
        }

        #endregion
    }
}
