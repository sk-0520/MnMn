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
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Define.NicoNico.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.NicoNico.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.NicoNico.Video.Raw;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Control.NicoNico.Video
{
    public class VideoInformationViewModel: SingleModelWrapperViewModelBase<RawVideoThumbModel>
    {
        #region variable

        VideoInformationLoad _videoLoad;

        #endregion

        public VideoInformationViewModel(Mediation mediation, RawVideoThumbModel model)
            : base(model)
        {
            Mediation = mediation;
        }

        #region property

        Mediation Mediation { get; set; }

        public VideoInformationLoad VideoLoad
        {
            get { return this._videoLoad; }
            set { SetVariableValue(ref this._videoLoad, value); }
        }

        /// <summary>
        /// 見たことがある。
        /// </summary>
        public bool Visited { get;set;}
        /// <summary>
        /// キャッシュ済み。
        /// </summary>
        public bool Cached { get; set; }

        public Uri ThumbnailUri { get { return RawValueUtility.ConvertUri(Model.ThumbnailUrl); } }
        public DateTime FirstRetrieve { get { return RawValueUtility.ConvertDateTime(Model.FirstRetrieve); } }
        public TimeSpan Length { get { return GetthumbinfoUtility.ConvertTimeSpan(Model.Length); } }
        public MovieType MovieType { get { return GetthumbinfoUtility.ConvertMovieType(Model.MovieType); } }
        public long SizeHigh { get { return RawValueUtility.ConvertLong(Model.SizeHigh); } }
        public long SizeLow{ get { return RawValueUtility.ConvertLong(Model.SizeLow); } }
        public int ViewCounter { get { return RawValueUtility.ConvertInteger(Model.ViewCounter); } }
        public int CommentNum { get { return RawValueUtility.ConvertInteger(Model.CommentNum); } }
        public int MylistCounter { get { return RawValueUtility.ConvertInteger(Model.MylistCounter); } }
        public Uri WatchUrl { get { return RawValueUtility.ConvertUri(Model.WatchUrl); } }
        public ThumbType ThumbType { get { return GetthumbinfoUtility.ConvertThumbType(Model.ThumbType); } }
        public bool Embeddable { get { return GetthumbinfoUtility.IsEmbeddable(Model.Embeddable); } }
        public bool LivePlay { get { return GetthumbinfoUtility.IsLivePlay(Model.NoLivePlay); } }
        public Uri UserIconUrl { get { return RawValueUtility.ConvertUri(Model.UserIconUrl); } }
        
        #endregion
    }
}
