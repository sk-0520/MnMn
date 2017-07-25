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
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
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
    public class SmileVideoRecommendationsFinderViewModel: SmileVideoNewArrivalsFinderViewModel
    {
        public SmileVideoRecommendationsFinderViewModel(Mediator mediator)
            : base(mediator, SmileVideoMediationKey.recommendation)
        { }

        #region function

        #endregion

        #region SmileVideoNewArrivalsFinderViewModel

        protected override SmileVideoInformationFlags InformationFlags => SmileVideoInformationFlags.All;

        public override bool NeedSession { get { return true; } }

        private FeedSmileVideoItemModel ConvertItemFromRaw(RawSmileVideoRecommendItemModel raw)
        {
            var result = new FeedSmileVideoItemModel();

            result.Title = raw.TitleShort;
            //TODO: うおおお
            result.Link = "/" + raw.Id;

            var detailModel = new RawSmileVideoFeedDetailModel();
            detailModel.ThumbnailUrl = raw.ThumbnailUrl;
            detailModel.MylistCounter = raw.MylistCounter;
            detailModel.ViewCounter = raw.ViewCounter;
            detailModel.CommentNum = raw.NumRes;
            detailModel.FirstRetrieve = RawValueUtility.ConvertUnixTime(raw.FirstRetrieve).ToString("s");
            detailModel.Length = raw.Length;

            result.Description = SmileVideoFeedUtility.ConvertDescriptionFromFeedDetailModel(detailModel);

            return result;
        }

        protected async override Task<FeedSmileVideoModel> LoadFeedAsync()
        {
            var rec = new Recommendations(Mediator);

            var rawData = await rec.LoadAsync();
            if(!SmileVideoRecommendationsUtility.IsSuccessResponse(rawData)) {
                return null;
            }
            var result = new FeedSmileVideoModel();
            var items = rawData.Items.Select(i => ConvertItemFromRaw(i));
            result.Channel.Items.AddRange(items);

            var isContinue = true;
            do {
                rawData = await rec.LoadNextAsync(rawData.Information.Page);
                items = rawData.Items.Select(i => ConvertItemFromRaw(i));
                result.Channel.Items.AddRange(items);
                isContinue = !RawValueUtility.ConvertBoolean(rawData.Information.EndOfRecommend);
                if(isContinue) {
                    Thread.Sleep(Constants.ServiceSmileVideoRecommendationsPageContinueWaitTime);
                }
            } while(isContinue);

            return result;
        }

        #endregion
    }
}
