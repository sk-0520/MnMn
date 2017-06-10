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
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video
{
    public class SmileVideoTagViewModel: SingleModelWrapperViewModelBase<RawSmileVideoTagItemModel>
    {
        #region variable

        bool _existPedia;

        #endregion

        public SmileVideoTagViewModel(Mediation mediation, RawSmileVideoTagItemModel model)
            : base(model)
        {
            Mediation = mediation;
        }

        #region property

        Mediation Mediation { get; }

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
                var videoId = GetVideoIdFromTagName();
                if(string.IsNullOrWhiteSpace(videoId)) {
                    throw new InvalidOperationException($"{nameof(GetVideoIdFromTagName)}: {TagName}");
                }

                return videoId;
            }
        }

        #endregion

        #region function

        string GetVideoIdFromTagName()
        {
            if(!string.IsNullOrWhiteSpace(TagName)) {
                var inputValue = TagName.Trim();
                object outputValue;
                if(Mediation.ConvertValue(out outputValue, typeof(string), SmileMediationKey.inputGetVideoId, inputValue, typeof(string), ServiceType.Smile)) {
                    return (string)outputValue;
                }
            }

            return string.Empty;
        }

        #endregion
    }
}
