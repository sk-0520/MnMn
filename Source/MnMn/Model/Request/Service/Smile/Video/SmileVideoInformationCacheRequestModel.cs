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
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Video.Parameter;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Raw;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw.Feed;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Video
{
    public class SmileVideoInformationCacheRequestModel: CustomCacheDataRequestModelBase
    {
        public SmileVideoInformationCacheRequestModel(SmileVideoInformationCacheParameterModel parameter)
            : base(Define.ServiceType.SmileVideo, parameter)
        {
            CacheParameter = (SmileVideoInformationCacheParameterModel)Parameter;
        }

        #region property

        SmileVideoInformationCacheParameterModel CacheParameter { get; }

        public SmileVideoVideoInformationSource InformationSource
        {
            get { return CacheParameter.InformationSource; }
        }

        public string VideoId
        {
            get { return CacheParameter.VideoId; }
        }
        public CacheSpan ThumbCacheSpan
        {
            get { return CacheParameter.ThumbCacheSpan; }
        }

        public RawSmileVideoThumbModel Thumb
        {
            get { return CacheParameter.Thumb; }
        }

        public RawSmileContentsSearchItemModel ContentsSearch
        {
            get { return CacheParameter.ContentsSearch; }
        }

        public FeedSmileVideoItemModel Feed
        {
            get { return CacheParameter.Feed; }
        }
        public SmileVideoInformationFlags InformationFlags
        {
            get { return CacheParameter.InformationFlags; }
        }



        #endregion
    }
}
