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
using ContentTypeTextNet.MnMn.MnMn.Define.Exceptions.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video.Api.V1;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile;
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

        object checker = new object();

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
            if(SmileIdUtility.NeedCorrectionVideoId(videoId)) {
                return LoadFromVideoIdAsync(videoId, Constants.ServiceSmileVideoThumbCacheSpan).ContinueWith(t => {
                    if(t.IsFaulted) {
                        Mediation.Logger.Error($"number videid: convert error {videoId}");
                        return videoId;
                    }
                    var information = t.Result;
                    Mediation.Logger.Debug($"number videid: {videoId} to {information.VideoId}");
                    information.DecrementReference();
                    return information.VideoId;
                }).Result;
            }

            return videoId;
        }

        SmileVideoInformationViewModel GetVideoInformationFromCorrectionId(string safeVideoId, SmileVideoInformationViewModel baseInformation)
        {
            return Get(safeVideoId, () => {
                if(baseInformation.VideoId == safeVideoId) {
                    return baseInformation;
                } else {
                    // 動画IDが異なる(生の nnnn 形式と 動画情報の xxnnnn 形式)ことが分かれば動画情報取得処理が動いているためキャッシュ上の動画情報を返す
                    var correctionInformation = LoadFromVideoIdAsync(baseInformation.VideoId, default(CacheSpan)).Result;
                    return correctionInformation;
                }
            });
        }

        public Task<SmileVideoInformationViewModel> LoadFromVideoIdAsync(string videoId, CacheSpan thumbCacheSpan)
        {
            var usingVideoId = videoId;

            SmileVideoInformationViewModel result;
            if(TryGetValue(usingVideoId, out result)) {
                result.IncrementReference();
                return Task.FromResult(result);
            }

            return SmileVideoInformationUtility.LoadGetthumbinfoAsync(Mediation, usingVideoId, thumbCacheSpan).ContinueWith(t => {
                var rawGetthumbinfo = t.Result;

                if(!SmileVideoGetthumbinfoUtility.IsSuccessResponse(rawGetthumbinfo)) {
                    throw new SmileVideoGetthumbinfoFailureException(videoId, rawGetthumbinfo);
                }

                var createdInformation = new SmileVideoInformationViewModel(Mediation, rawGetthumbinfo.Thumb, UnOrdered);
                lock(checker) {
                    if(TryGetValue(usingVideoId, out result)) {
                        result.IncrementReference();
                        return result;
                    }
                    createdInformation.IncrementReference();
                    Add(usingVideoId, createdInformation);
                    return createdInformation;
                }
            });
        }

        public SmileVideoInformationViewModel Get(RawSmileVideoThumbModel thumb)
        {
            var usingVideoId = GetSafeVideoId(thumb.VideoId);

            // こいつの動画IDは xxnnnn 形式前提だから補正しない(前提が崩れたらちょっとしんどいね！)
            var result = Get(usingVideoId, () => new SmileVideoInformationViewModel(Mediation, thumb, UnOrdered));
            result.IncrementReference();

            return result;
        }

        public SmileVideoInformationViewModel Get(RawSmileContentsSearchItemModel search)
        {
            var usingVideoId = GetSafeVideoId(search.ContentId);

            var result = GetVideoInformationFromCorrectionId(usingVideoId, new SmileVideoInformationViewModel(Mediation, search, UnOrdered));

            result.IncrementReference();

            return result;
        }

        public SmileVideoInformationViewModel Get(FeedSmileVideoItemModel feed, SmileVideoInformationFlags informationFlags)
        {
            //TODO: 動画ID取得意外と面倒なのね
            var tempResult = new SmileVideoInformationViewModel(Mediation, feed, UnOrdered, informationFlags);
            var usingVideoId = GetSafeVideoId(tempResult.VideoId);
            var result = GetVideoInformationFromCorrectionId(usingVideoId, tempResult);
            result.IncrementReference();

            return result;
        }

        #endregion
    }
}
