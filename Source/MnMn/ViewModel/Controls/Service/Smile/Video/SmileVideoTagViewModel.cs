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
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video
{
    public class SmileVideoTagViewModel: SingleModelWrapperViewModelBase<RawSmileVideoTagItemModel>
    {
        #region variable

        bool _existPedia;

        #endregion

        public SmileVideoTagViewModel(Mediator mediator, RawSmileVideoTagItemModel model)
            : base(model)
        {
            Mediator = mediator;
        }

        #region property

        Mediator Mediator { get; }

        public string TagName
        {
            get { return Model.Text; }
        }

        public bool IsLocked
        {
            get { return SmileVideoGetthumbinfoUtility.IsLocked(Model); }
        }

        public bool IsCategory
        {
            get { return SmileVideoGetthumbinfoUtility.IsCategory(Model); }
        }

        public bool ExistPedia
        {
            get { return this._existPedia; }
            set { SetVariableValue(ref this._existPedia, value); }
        }

        public bool HasVideoId
        {
            get { return !string.IsNullOrWhiteSpace(GetVideoIdFromTagName()); }
        }

        public string VideoIdFromTagName
        {
            get
            {
                return GetVideoIdFromTagName();
            }
        }

        public bool IsRankingCategory
        {
            get
            {
                var rankingDefine = Mediator.GetResultFromRequest<IReadOnlySmileVideoRanking>(new RequestModel(RequestKind.RankingDefine, ServiceType.SmileVideo));
                return rankingDefine.Items.Any(i => i.Categories.Any(c => c.Words.Values.Any(s => string.Equals(s, TagName, StringComparison.InvariantCultureIgnoreCase))));
            }
        }

        #endregion

        #region function

        string GetVideoIdFromTagName()
        {
            if(!string.IsNullOrWhiteSpace(TagName)) {
                var inputValue = TagName.Trim();
                var videoId = SmileIdUtility.GetVideoId(inputValue, Mediator);
                if(!string.IsNullOrWhiteSpace(videoId)) {
                    return videoId;
                }
            }

            return string.Empty;
        }

        #endregion
    }
}
