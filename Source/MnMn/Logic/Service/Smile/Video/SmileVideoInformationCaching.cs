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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video.Api.V1;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Raw;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw.Feed;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video
{
    /// <summary>
    /// 動画IDとそれに紐付く情報を保持。
    /// </summary>
    public class SmileVideoInformationCaching: Caching<string, SmileVideoInformationViewModel>
    {
        #region define

        const int UnOrdered = -1;

        #endregion

        public SmileVideoInformationCaching(Mediation mediation)
            : base(true)
        {
            Mediation = mediation;
        }

        #region property

        Mediation Mediation { get; }

        #endregion

        #region function

        string GetSafeVideoId(string videoId)
        {
            return videoId;
        }

        public Task<SmileVideoInformationViewModel> LoadFromVideoIdAsync(string videoId, CacheSpan thumbCacheSpan)
        {
            var usingVideoId = GetSafeVideoId(videoId);

            SmileVideoInformationViewModel result;
            if(TryGetValue(usingVideoId, out result)) {
                return Task.FromResult(result);
            }

            return SmileVideoInformationUtility.LoadGetthumbinfoAsync(Mediation, usingVideoId, thumbCacheSpan).ContinueWith(t => {
                var rawGetthumbinfo = t.Result;
                result = new SmileVideoInformationViewModel(Mediation, rawGetthumbinfo.Thumb, UnOrdered);
                Add(usingVideoId, result);
                return result;
            });
        }

        public SmileVideoInformationViewModel Get(RawSmileVideoThumbModel thumb)
        {
            var usingVideoId = GetSafeVideoId(thumb.VideoId);

            return Get(usingVideoId, () => new SmileVideoInformationViewModel(Mediation, thumb, UnOrdered));
        }

        public SmileVideoInformationViewModel Get(RawSmileContentsSearchItemModel search)
        {
            var usingVideoId = GetSafeVideoId(search.ContentId);

            return Get(usingVideoId, () => new SmileVideoInformationViewModel(Mediation, search, UnOrdered));
        }

        public SmileVideoInformationViewModel Get(FeedSmileVideoItemModel feed, SmileVideoInformationFlags informationFlags)
        {
            //TODO: 動画ID取得意外と面倒なのね
            var tempResult = new SmileVideoInformationViewModel(Mediation, feed, UnOrdered, informationFlags);
            var usingVideoId = GetSafeVideoId(tempResult.VideoId);
            return Get(usingVideoId, () => tempResult);
        }

        #endregion
    }
}
