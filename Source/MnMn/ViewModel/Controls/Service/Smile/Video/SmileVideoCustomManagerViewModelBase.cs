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
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Service.Smile;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video
{
    public abstract class SmileVideoCustomManagerViewModelBase: ManagerViewModelBase
    {
        #region variable

        SmileVideoFinderViewModelBase _selectedItem;

        #endregion

        public SmileVideoCustomManagerViewModelBase(Mediator mediator)
            : base(mediator)
        {
            var settingResult = Mediation.Request(new RequestModel(RequestKind.Setting, ServiceType.SmileVideo));
            Setting = (SmileVideoSettingModel)settingResult.Result;

            Session = Mediation.GetResultFromRequest<SmileSessionViewModel>(new RequestModel(RequestKind.Session, ServiceType.Smile));
        }

        #region property

        protected SmileVideoSettingModel Setting { get; }

        public SmileSessionViewModel Session { get; }

        public virtual SmileVideoFinderViewModelBase SelectedItem
        {
            get { return this._selectedItem; }
            set
            {
                var prevItem = this._selectedItem;
                if(SetVariableValue(ref this._selectedItem, value)) {
                    if(prevItem != null && this._selectedItem.FinderLoadState == SourceLoadState.None) {
                        this._selectedItem.LoadDefaultCacheAsync();
                    }
                }
            }
        }

        #endregion

    }
}
