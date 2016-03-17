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
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Setting
{
    public class SmileVideoFilteringEditItemViewModel: SingleModelWrapperViewModelBase<SmileVideoFilteringSettingModel>
    {
        #region variable

        FilteringType _editFilteringType;
        SmileVideoFilteringTarget _editFilteringTarget;
        string _editSource;
        bool _editIgnoreCase;
        bool _editIgnoreWidth;

        #endregion

        public SmileVideoFilteringEditItemViewModel(SmileVideoFilteringSettingModel model)
            : base(model)
        {
            EditFilteringType = model.FilteringType;
            EditTarget = model.Target;
            EditSource = model.Source;
            EditIgnoreCase = model.IgnoreCase;
            EditIgnoreWidth = model.IgnoreWidth;
        }

        #region property

        public FilteringType FilteringType
        {
            get { return Model.FilteringType; }
            set { SetModelValue(value); }
        }
        public SmileVideoFilteringTarget Target
        {
            get { return Model.Target; }
            set { SetModelValue(value); }
        }
        public string Source
        {
            get { return Model.Source; }
            set { SetModelValue(value); }
        }
        public bool IgnoreCase
        {
            get { return Model.IgnoreCase; }
            set { SetModelValue(value); }
        }
        public bool IgnoreWidth
        {
            get { return Model.IgnoreWidth; }
            set { SetModelValue(value); }
        }

        public FilteringType EditFilteringType
        {
            get { return this._editFilteringType; }
            set { SetVariableValue(ref this._editFilteringType, value); }
        }
        public SmileVideoFilteringTarget EditTarget
        {
            get { return this._editFilteringTarget; }
            set { SetVariableValue(ref this._editFilteringTarget, value); }
        }
        public string EditSource
        {
            get { return this._editSource; }
            set { SetVariableValue(ref this._editSource, value); }
        }
        public bool EditIgnoreCase
        {
            get { return this._editIgnoreCase; }
            set { SetVariableValue(ref this._editIgnoreCase, value); }
        }
        public bool EditIgnoreWidth
        {
            get { return this._editIgnoreWidth; }
            set { SetVariableValue(ref this._editIgnoreWidth, value); }
        }

        #endregion

        #region property

        internal void Update()
        {
            FilteringType = EditFilteringType;
            Target = EditTarget;
            Source = EditSource;
            IgnoreCase = EditIgnoreCase;
            IgnoreWidth = EditIgnoreWidth;
        }

        #endregion
    }
}
